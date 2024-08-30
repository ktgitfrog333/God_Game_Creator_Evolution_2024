using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Select.View
{
    /// <summary>
    /// エリアTMP
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AreaTMPView : MonoBehaviour, IAreaTMPView
    {
        /// <summary>TMP</summary>
        [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;
        /// <summary>選択可能カラー</summary>
        [SerializeField] private Color targetColor;

        private void Reset()
        {
            m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        public bool RenderTargetMark()
        {
            try
            {
                m_TextMeshProUGUI.color = targetColor;

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
    /// エリアTMP
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IAreaTMPView
    {
        /// <summary>
        /// 選択済みマークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderTargetMark();
    }
}
