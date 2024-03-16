using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Select.View
{
    /// <summary>
    /// 線を引く用のトランスフォーム
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class LineRectTransformView : MonoBehaviour, ILineRectTransformView
    {
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>Rectトランスフォーム</summary>
        public RectTransform RectTransform => _transform != null ? _transform as RectTransform : (_transform = transform) as RectTransform;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        public bool RenderLineFromPointBetweenToPoint(Vector2 anchoredPositionFrom, Vector2 anchoredPositionTo)
        {
            try
            {
                Vector2 dir = (anchoredPositionTo - anchoredPositionFrom).normalized;
                float distance = Vector2.Distance(anchoredPositionFrom, anchoredPositionTo);
                
                RectTransform.sizeDelta = new Vector2(distance, RectTransform.sizeDelta.y);
                RectTransform.pivot = new Vector2(0, 0.5f);
                RectTransform.position = anchoredPositionFrom;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                RectTransform.rotation = Quaternion.Euler(0, 0, angle);

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
    /// 線を引く用のトランスフォーム
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ILineRectTransformView
    {
        /// <summary>
        /// ステージコンテンツからターゲットポインツへ線を描画
        /// </summary>
        /// <param name="anchoredPositionFrom">元の位置</param>
        /// <param name="anchoredPositionTo">先の位置</param>
        /// <returns>成功／失敗</returns>
        public bool RenderLineFromPointBetweenToPoint(Vector2 anchoredPositionFrom, Vector2 anchoredPositionTo);
    }
}
