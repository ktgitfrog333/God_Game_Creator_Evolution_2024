using System.Collections;
using System.Collections.Generic;
using Main.Model;
using Main.Utility;
using UniRx;
using UnityEngine;
using Universal.Utility;

namespace Main.View
{
    /// <summary>
    /// 魂の経験値
    /// ビュー
    /// </summary>
    public class SoulMoneyView : MonoBehaviour, ISoulMoneyView
    {
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { 1.5f, 1.5f };
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>ボディスプライトのビュー</summary>
        [SerializeField] private BodySpriteView bodySpriteView;
        /// <summary>獲得済みか</summary>
        public IReactiveProperty<bool> IsGeted { get; private set; } = new BoolReactiveProperty();
        /// <summary>敵のプロパティ</summary>
        private EnemiesProp _enemiesProp;
        /// <summary>敵のプロパティ</summary>
        public EnemiesProp EnemiesProp => _enemiesProp;

        private void Reset()
        {
            bodySpriteView = GetComponentInChildren<BodySpriteView>();
        }

        private void OnEnable()
        {
            IsGeted.Value = false;
            StartCoroutine(GeneralUtility.ActionsAfterDelay(durations[0],
                () => Observable.FromCoroutine<bool>(observer => bodySpriteView.PlayFadeAnimation(observer, Common.EnumFadeState.Close, durations[1]))
                    .Subscribe(_ =>
                    {
                        IsGeted.Value = true;
                        gameObject.SetActive(false);
                    })
                    .AddTo(gameObject)));
        }

        public bool SetEnemiesProp(EnemiesProp enemiesProp)
        {
            try
            {
                _enemiesProp = enemiesProp;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// 魂の経験値
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ISoulMoneyView
    {
        /// <summary>
        /// 敵のプロパティをセット
        /// </summary>
        /// <param name="enemiesProp">敵のプロパティ</param>
        /// <returns>成功／失敗</returns>
        public bool SetEnemiesProp(EnemiesProp enemiesProp);
    }
}
