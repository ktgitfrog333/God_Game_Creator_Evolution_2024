using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// カウントダウンタイマーの情報に合わせてUIを変化させる
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ClearCountdownTimerTextView : MonoBehaviour, IClearCountdownTimerTextView
    {
        /// <summary>テキストUI</summary>
        [SerializeField] private TextMeshProUGUI textMeshPro;
        /// <summary>デフォルト表示</summary>
        private readonly string DEFAULT = "empty";

        public bool SetTextImport(float timeSec, float limitTimeSecMax)
        {
            try
            {
                textMeshPro.text = limitTimeSecMax == 0f ? DEFAULT : $"{limitTimeSecMax * (timeSec / limitTimeSecMax)}";

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
    }

    /// <summary>
    /// カウントダウンタイマーの情報に合わせてUIを変化させる
    /// ビュー
    /// </summary>
    public interface IClearCountdownTimerTextView
    {
        /// <summary>
        /// テキストをセットする
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetTextImport(float timeSec, float limitTimeSecMax);
    }
}
