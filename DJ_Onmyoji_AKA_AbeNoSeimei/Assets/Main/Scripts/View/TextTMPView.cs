using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// TextMeshProの表示制御
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextTMPView : MonoBehaviour, ITextTMPView
    {
        /// <summary>TextMeshPro - Text(UI)</summary>
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        /// <summary>有効状態のカラー</summary>
        [SerializeField] private Color32 enabledColor = new Color32(255, 255, 255, 255);
        /// <summary>無効状態のカラー</summary>
        [SerializeField] private Color32 disabledColor = new Color32(95, 95, 95, 255);

        private void Reset()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        public bool SetColorToEnabled()
        {
            try
            {
                textMeshProUGUI.color = enabledColor;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetColorToDisabled()
        {
            try
            {
                textMeshProUGUI.color = disabledColor;

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
    /// TextMeshProの表示制御
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ITextTMPView
    {
        /// <summary>
        /// テキストカラーをセット
        /// 有効状態
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetColorToEnabled();
        /// <summary>
        /// テキストカラーをセット
        /// 無効状態
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetColorToDisabled();
    }
}
