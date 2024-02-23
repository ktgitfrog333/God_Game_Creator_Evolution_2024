using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Main.Utility
{
    /// <summary>
    /// Mainのユーティリティ
    /// ビュー
    /// </summary>
    public class MainViewUtility : IMainViewUtility
    {
        /// <summary>デフォルト表示</summary>
        private readonly float DEFAULT = 1f;
        /// <summary>ループアニメーションSeqence管理</summary>
        private struct LoopAnimations
        {
            /// <summary>スケーリングするDOTweenアニメーション再生</summary>
            public Sequence scaling;
        }
        /// <summary>ループアニメーションSeqence管理</summary>
        private LoopAnimations _loopAnimations = new LoopAnimations();

        public IEnumerator PlayFadeAnimation<T>(System.IObserver<bool> observer, EnumFadeState state, float duration, T component) where T : Component
        {
            // componentがImageかSpriteRendererかをチェック
            if (component is Image image)
            {
                // Imageの場合の処理
                image.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 0f : 1f, duration)
                    .SetUpdate(true)
                    .OnComplete(() => observer.OnNext(true));
            }
            else if (component is SpriteRenderer spriteRenderer)
            {
                // SpriteRendererの場合の処理
                spriteRenderer.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 1f : 0f, duration)
                    .From(state.Equals(EnumFadeState.Open) ? 0f : 1f)
                    .SetUpdate(true)
                    .OnComplete(() => observer.OnNext(true));
            }
            else
            {
                throw new System.ArgumentException("Unsupported component type. Only Image and SpriteRenderer are supported.", nameof(component));
            }

            yield return null;
        }

        public bool PlayScalingLoopAnimation(float[] durations, float[] scales, Transform transform)
        {
            try
            {
                if (_loopAnimations.scaling == null)
                {
                    transform.localScale = Vector3.one * scales[1];
                    _loopAnimations.scaling = DOTween.Sequence()
                        .Append(transform.DOScale(scales[0], durations[0]))
                        .Append(transform.DOScale(scales[1], durations[1]))
                        .SetLoops(-1, LoopType.Restart);
                    _loopAnimations.scaling.Play();
                }
                else
                {
                    transform.localScale = Vector3.one * scales[1];
                    _loopAnimations.scaling.Restart();
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null)
        {
            try
            {
                if (maskAngle < 0f || 1f < maskAngle)
                    throw new System.Exception("不正な値セット:0fから1fの値を設定して下さい");
                else if (0f < maskAngle && transform == null)
                    throw new System.Exception("不正な値セット:maskAngle設定時はtransformも設定が必要です");

                float baseAmount = GetRate(timeSec, limitTimeSecMax);
                float calc = System.Math.Max(0f, maskAngle);
                image.fillAmount = baseAmount * (1f - calc);
                if (transform != null)
                    transform.eulerAngles = new Vector3(0f, 0f, 360f) * (calc * .5f);
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetAnchorOfImage(RectTransform rectTransform, float timeSec, float limitTimeSecMax, Vector2 anchorPosMin, Vector2 anchorPosMax)
        {
            try
            {
                var basePos = GetRate(timeSec, limitTimeSecMax);
                var movePos = new Vector2(rectTransform.anchoredPosition.x, (1f - basePos) * (anchorPosMax.y + anchorPosMin.y));
                rectTransform.anchoredPosition = movePos;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 割合を取得
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>割合</returns>
        private float GetRate(float timeSec, float limitTimeSecMax)
        {
            return limitTimeSecMax == 0f ? DEFAULT : (timeSec / limitTimeSecMax);
        }

        public bool SetColorOfImage(float onmyoStateValue, Image image, Color32[] colors)
        {
            try
            {
                Color32 colorToSet;
                if (onmyoStateValue <= -1)
                {
                    colorToSet = colors[1];
                }
                else if (onmyoStateValue >= 1)
                {
                    colorToSet = colors[0];
                }
                else
                {
                    float blend = (onmyoStateValue + 1) / 2; // -1から1の値を0から1に変換
                    colorToSet = Color32.Lerp(colors[1], colors[0], blend);
                }
                image.color = colorToSet;
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
    /// Mainのユーティリティ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IMainViewUtility
    {
        /// <summary>
        /// ImageのFillAmountをセットする
        /// </summary>
        /// <param name="image">イメージ</param>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <param name="maskAngle">マスクする角度の割合（0f~1f）</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null);
        /// <summary>
        /// Imageのアンカーをセットする
        /// </summary>
        /// <param name="rectTransform">Rectトランスフォーム</param>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <param name="anchorPosMin">アンカー位置（最小）</param>
        /// <param name="anchorPosMax">アンカー位置（最大）</param>
        /// <returns>成功／失敗</returns>
        public bool SetAnchorOfImage(RectTransform rectTransform, float timeSec, float limitTimeSecMax, Vector2 anchorPosMin, Vector2 anchorPosMax);
        /// <summary>
        /// ImageのColor32をセットする
        /// </summary>
        /// <param name="onmyoStateValue">陰陽（昼夜）の状態</param>
        /// <param name="image">イメージ</param>
        /// <param name="colors">カラー</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorOfImage(float onmyoStateValue, Image image, Color32[] colors);
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <param name="duration">終了時間</param>
        /// <param name="component">コンポーネント</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation<T>(System.IObserver<bool> observer, EnumFadeState state, float duration, T component) where T : Component;
        /// <summary>
        /// スケーリングするDOTweenアニメーション再生
        /// </summary>
        /// <param name="durations">終了時間</param>
        /// <param name="scales">スケールのパターン</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool PlayScalingLoopAnimation(float[] durations, float[] scales, Transform transform);
    }
}
