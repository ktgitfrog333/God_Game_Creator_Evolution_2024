using System.Collections;
using System.Collections.Generic;
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

        public bool SetAnchorsBasedOnHeight(EnumFadeState state)
        {
            try
            {
                switch (state)
                {
                    case EnumFadeState.Open:
                        RectTransform.anchoredPosition = Vector2.zero;
                        break;
                    case EnumFadeState.Close:
                        RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, -RectTransform.rect.height * uiClosedRangeLevel);
                        break;
                }

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
    /// フェーダーグループのビュー
    /// インターフェース
    /// </summary>
    public interface IFadersGroupView
    {
        /// <summary>
        /// 高さに応じてアンカーを設定
        /// </summary>
        /// <param name="state">フェードステータス</param>
        /// <returns>成功／失敗</returns>
        public bool SetAnchorsBasedOnHeight(EnumFadeState state);
    }
}
