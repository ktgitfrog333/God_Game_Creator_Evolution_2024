using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Utility;
using UnityEngine;
using UniRx;

namespace Main.View
{
    /// <summary>
    /// ボディスプライト
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BodySpriteView : MonoBehaviour, IBodySpriteView
    {
        /// <summary>スプライトレンダラー</summary>
        [SerializeField] private SpriteRenderer spriteRenderer;
        /// <summary>ユーティリティ</summary>
        private MainViewUtility _utility = new MainViewUtility();
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state, float duration)
        {
            Observable.FromCoroutine<bool>(observer => _utility.PlayFadeAnimation(observer, state, duration, spriteRenderer))
                .Subscribe(x => observer.OnNext(x))
                .AddTo(gameObject);

            yield return null;
        }

        public bool PlayScalingLoopAnimation(float[] durations, float[] scales)
        {
            return _utility.PlayScalingLoopAnimation(durations, scales, Transform);
        }
    }

    /// <summary>
    /// ボディスプライト
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IBodySpriteView
    {
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <param name="duration">終了時間</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation(System.IObserver<bool> observer, EnumFadeState state, float duration);
        /// <summary>
        /// スケーリングするDOTweenアニメーション再生
        /// </summary>
        /// <param name="durations">終了時間</param>
        /// <param name="scales">スケールのパターン</param>
        /// <returns>成功／失敗</returns>
        public bool PlayScalingLoopAnimation(float[] durations, float[] scales);
    }
}
