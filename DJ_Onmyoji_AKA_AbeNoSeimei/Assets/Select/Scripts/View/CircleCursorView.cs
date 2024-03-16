using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Select.View
{
    /// <summary>
    /// サークルカーソル
    /// ビュー
    /// </summary>
    public class CircleCursorView : MonoBehaviour, ICircleCursorView
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public RectTransform RectTransform => _transform != null ? _transform as RectTransform : (_transform = transform) as RectTransform;

        public bool SetAnchorPosition(Vector2 position)
        {
            try
            {
                RectTransform.position = position;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSizeDelta(Vector2 sizeDelta)
        {
            try
            {
                RectTransform.sizeDelta = sizeDelta;

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
    /// サークルカーソル
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ICircleCursorView : IRectTransformView
    {
        /// <summary>
        /// アンカー位置をセット
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns>成功／失敗</returns>
        public bool SetAnchorPosition(Vector2 position);
    }

    /// <summary>
    /// Rectトランスフォーム
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IRectTransformView
    {
        /// <summary>
        /// サイズをセット
        /// </summary>
        /// <param name="sizeDelta">サイズ</param>
        /// <returns>成功／失敗</returns>
        public bool SetSizeDelta(Vector2 sizeDelta);
    }
}
