using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Main.Common;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// フェーダーグループ
    /// ビュー
    /// </summary>
    public class FadersGroupView : MonoBehaviour, IFadersGroupView
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>Rectトランスフォーム</summary>
        public RectTransform RectTransform => _transform != null ? (RectTransform)_transform : (RectTransform)(_transform = transform);
        /// <summary>UIメニューを閉じる範囲</summary>
        [SerializeField, Range(0f, 1f)] private float uiClosedRangeLevel = .8f;
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { 1f, .5f };
        /// <summary>アニメーション再生中</summary>
        private bool _isPlaying;
        /// <summary>シークエンス</summary>
        private Sequence _sequence;
        /// <summary>開始位置と終了位置の配列</summary>
        [SerializeField] private Vector2[] betweenPoses;

        private void Reset()
        {
            betweenPoses = new Vector2[]
            {
                (transform as RectTransform).anchoredPosition,
                new Vector2((transform as RectTransform).anchoredPosition.x, (transform as RectTransform).anchoredPosition.y -(transform as RectTransform).rect.height * uiClosedRangeLevel)
            };
        }

        public IEnumerator PlayMoveAnchorsBasedOnHeight(System.IObserver<bool> observer, EnumFadeState state)
        {
            if (_isPlaying)
            {
                if (_sequence != null && _sequence.IsActive() && !_sequence.IsComplete())
                    _sequence.Restart();
            }
            else
            {
                _isPlaying = true;
                switch (state)
                {
                    case EnumFadeState.Open:
                        _sequence = DOTween.Sequence()
                            .Append(RectTransform.DOAnchorPos(betweenPoses[0], 0f))
                            .AppendInterval(durations[0])
                            .Append(RectTransform.DOAnchorPos(betweenPoses[1], durations[1])
                            .OnComplete(() =>
                            {
                                // ここでオブジェクトの破棄をチェック
                                if (this == null || gameObject == null)
                                    return;

                                _isPlaying = false;
                                observer.OnNext(true);
                            }));

                        break;
                    case EnumFadeState.Close:
                        // 処理無し
                        break;
                    default:
                        // 処理無し
                        break;
                }
            }

            yield return null;
        }

        public IEnumerator PlayMoveAnchorsHeight(System.IObserver<bool> observer)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator PlayMoveAnchorsBased(System.IObserver<bool> observer)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// フェーダーグループのビュー
    /// インターフェース
    /// </summary>
    public interface IFadersGroupView
    {
        /// <summary>
        /// 高さに応じてアンカーを設定
        /// </summary>
        /// <param name="observer">オブサーバー</param>
        /// <param name="state">フェードステータス</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayMoveAnchorsBasedOnHeight(System.IObserver<bool> observer, EnumFadeState state);
        /// <summary>
        /// 高さに応じてアンカーを設定（上のみ）
        /// </summary>
        /// <param name="observer">オブサーバー</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayMoveAnchorsHeight(System.IObserver<bool> observer);
        /// <summary>
        /// 高さに応じてアンカーを設定（元に戻るのみ）
        /// </summary>
        /// <param name="observer">オブサーバー</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayMoveAnchorsBased(System.IObserver<bool> observer);
    }
}
