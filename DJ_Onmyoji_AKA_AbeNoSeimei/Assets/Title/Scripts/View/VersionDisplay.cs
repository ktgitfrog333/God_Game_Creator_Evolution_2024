using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshProを使用する場合

namespace Title.View
{
    /// <summary>
    /// バージョン情報表示テキスト
    /// </summary>
    public class VersionDisplay : MonoBehaviour
    {
        /// <summary>バージョン表示テキスト</summary>
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private void Reset()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            textMeshProUGUI.text = $"Ver {Application.version}";
        }
    }
}
