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

        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null)
        {
            try
            {
                if (maskAngle < 0f || 1f < maskAngle)
                    throw new System.Exception("不正な値セット:0fから1fの値を設定して下さい");
                else if (0f < maskAngle && transform == null)
                    throw new System.Exception("不正な値セット:maskAngle設定時はtransformも設定が必要です");

                float baseAmount = limitTimeSecMax == 0f ? DEFAULT : (timeSec / limitTimeSecMax);
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
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <param name="duration">終了時間</param>
        /// <param name="component">コンポーネント</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation<T>(System.IObserver<bool> observer, EnumFadeState state, float duration, T component) where T : Component;
    }
}
