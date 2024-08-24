using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ページ
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class PageView : MonoBehaviour, IPageView
    {
        /// <summary>キャンバスグループ</summary>
        [SerializeField] private CanvasGroup canvasGroup;

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public bool SetVisible(bool isEnabled)
        {
            try
            {
                canvasGroup.alpha = isEnabled ? 1f : 0f;
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
    /// ビュー
    /// ページ
    /// インターフェース
    /// </summary>
    public interface IPageView
    {
        /// <summary>
        /// アルファ値を設定
        /// </summary>
        /// <param name="isEnabled">有効か</param>
        /// <returns>成功／失敗</returns>
        public bool SetVisible(bool isEnabled);
    }
}
