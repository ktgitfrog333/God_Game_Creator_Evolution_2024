using Main.Common;
using Main.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// レベル背景
    /// ビュー
    /// </summary>
    public class LevelBackgroundView : MonoBehaviour, ILevelBackgroundView
    {
        /// <summary>レベル背景（昼／夜）用のビュー</summary>
        [SerializeField] private FadeLevelBackgroundView[] fadeLevelBackgroundViews;
        /// <summary>昼切り替えの基準値</summary>
        [SerializeField] private float switchDayLevel = .1f;
        /// <summary>夜切り替えの基準値</summary>
        [SerializeField] private float switchNightLevel = -.1f;
        /// <summary>昼</summary>
        private const string GAMEOBJECT_NAME_FADEIMAGEDAY = "FadeImageDay";
        /// <summary>夜</summary>
        private const string GAMEOBJECT_NAME_FADEIMAGENIGHT = "FadeImageNight";

        private void Reset()
        {
            fadeLevelBackgroundViews = GetComponentsInChildren<FadeLevelBackgroundView>();
        }

        public IEnumerator SwitchLayerAndPlayFadeAnimation(System.IObserver<bool> observer, float onmyoStateValue)
        {
            if (switchDayLevel <= onmyoStateValue &&
                switchNightLevel < onmyoStateValue &&
                !IsSwitched(SunMoonState.Daytime, fadeLevelBackgroundViews))
            {
                Observable.FromCoroutine<bool>(observer => SetAsLastSiblingAndPlayFadeAnimation(observer, fadeLevelBackgroundViews, GAMEOBJECT_NAME_FADEIMAGEDAY))
                    .Subscribe(_ => observer.OnNext(true))
                    .AddTo(gameObject);
                MainGameManager.Instance.AudioOwner.SwitchClipDay();
            }
            else if (onmyoStateValue <= switchNightLevel &&
                !IsSwitched(SunMoonState.Night, fadeLevelBackgroundViews))
            {
                Observable.FromCoroutine<bool>(observer => SetAsLastSiblingAndPlayFadeAnimation(observer, fadeLevelBackgroundViews, GAMEOBJECT_NAME_FADEIMAGENIGHT))
                    .Subscribe(_ => observer.OnNext(true))
                    .AddTo(gameObject);
                MainGameManager.Instance.AudioOwner.SwitchClipNight();
            }

            yield return null;
        }

        /// <summary>
        /// 子要素への並び替えとフェードアニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="fadeLevelBackgroundViews">レベル背景（昼／夜）用のビュー</param>
        /// <param name="gameObjectName">ゲームオブジェクト名</param>
        /// <returns>コルーチン</returns>
        private IEnumerator SetAsLastSiblingAndPlayFadeAnimation(System.IObserver<bool> observer, FadeLevelBackgroundView[] fadeLevelBackgroundViews, string gameObjectName)
        {
            var view = fadeLevelBackgroundViews.Where(q => q.name.Equals(gameObjectName))
                .ToArray()[0];
            // 変化させる方を手前へ動かす
            view.transform.SetAsLastSibling();
            // 透明度を0にする
            if (!view.SetFade(EnumFadeState.Open))
                Debug.LogError("SetFade");
            // フェードを入れる
            Observable.FromCoroutine<bool>(observer => view.PlayFadeAnimation(observer, EnumFadeState.Close))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);

            yield return null;
        }

        /// <summary>
        /// 切り替え済みか
        /// </summary>
        /// <param name="sunMoonState">対象の昼／夜の状態</param>
        /// <param name="fadeLevelBackgroundViews">フェードレベル背景ビューの配列</param>
        /// <returns>切り替え済みか</returns>
        private bool IsSwitched(SunMoonState sunMoonState, FadeLevelBackgroundView[] fadeLevelBackgroundViews)
        {
            switch (sunMoonState)
            {
                case SunMoonState.Daytime:
                    return CheckIfActive(fadeLevelBackgroundViews, GAMEOBJECT_NAME_FADEIMAGEDAY);
                case SunMoonState.Night:
                    return CheckIfActive(fadeLevelBackgroundViews, GAMEOBJECT_NAME_FADEIMAGENIGHT);
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(sunMoonState), sunMoonState, null);
            }
        }

        /// <summary>
        /// 指定されたゲームオブジェクト名のビューがアクティブかどうかをチェック
        /// </summary>
        /// <param name="views">ビューの配列</param>
        /// <param name="gameObjectName">ゲームオブジェクト名</param>
        /// <returns>アクティブならtrue、そうでなければfalse</returns>
        private bool CheckIfActive(FadeLevelBackgroundView[] views, string gameObjectName)
        {
            var targetView = views.FirstOrDefault(q => q.name.Equals(gameObjectName));
            if (targetView == null)
            {
                Debug.LogError($"View with name {gameObjectName} not found.");
                return false;
            }
            return targetView.transform.GetSiblingIndex() == targetView.transform.parent.childCount - 1;
        }
    }

    /// <summary>
    /// レベル背景
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ILevelBackgroundView
    {
        /// <summary>
        /// レイヤー切り替えとフェードアニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="onmyoStateValue">陰陽（昼夜）の状態</param>
        /// <returns>コルーチン</returns>
        public IEnumerator SwitchLayerAndPlayFadeAnimation(System.IObserver<bool> observer, float onmyoStateValue);
    }
}
