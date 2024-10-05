using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Main.Common;
using Main.Utility;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// フェードイメージ
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class FadeImageView : MonoBehaviour, IFadeImageView
    {
        /// <summary>終了時間</summary>
        [SerializeField] private float duration = 2.0f;
        /// <summary>イメージ</summary>
        [SerializeField] protected Image image;
        /// <summary>（ループを使用する場合）ループ回数</summary>
        [SerializeField] int loops;
        /// <summary>ユーティリティ</summary>
        private MainViewUtility _utility = new MainViewUtility();

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state)
        {
            Observable.FromCoroutine<bool>(observer => _utility.PlayFadeAnimation(observer, state, duration, image))
                .Subscribe(x => observer.OnNext(x))
                .AddTo(gameObject);

            yield return null;
        }

        public bool SetFade(EnumFadeState state)
        {
            return _utility.SetFade(state, image);
        }

        public IEnumerator PlayFadeLoopsYoyoAnimation(System.IObserver<bool> observer, EnumFadeState state)
        {
            Observable.FromCoroutine<bool>(observer => _utility.PlayFadeLoopsYoyoAnimation(observer, state, duration, loops, image))
                .Subscribe(x => observer.OnNext(x))
                .AddTo(gameObject);

            yield return null;
        }
    }

    /// <summary>
    /// ビュー
    /// フェードイメージ
    /// インターフェース
    /// </summary>
    public interface IFadeImageView
    {
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state);
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// ヨーヨー（行って戻ってくる）のループモード
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeLoopsYoyoAnimation(System.IObserver<bool> observer, EnumFadeState state);
        /// <summary>
        /// フェードステータスをセット
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool SetFade(EnumFadeState state);
    }
}
