using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Select.View
{
    /// <summary>
    /// ステージコンテンツ
    /// ビュー
    /// </summary>
    public class StageContetsView : MonoBehaviour
    {
        /// <summary>子要素のトランスフォーム/summary>
        [SerializeField] private RectTransform[] anchoredPositionsInChild;
        /// <summary>子要素のトランスフォーム</summary>
        public RectTransform[] AnchoredPositionsInChild => anchoredPositionsInChild;
        /// <summary>子要素のトランスフォーム/summary>
        [SerializeField] private RectTransform[] anchoredPositionsInChild1;
        /// <summary>子要素のトランスフォーム</summary>
        public RectTransform[] AnchoredPositionsInChild1 => anchoredPositionsInChild1;

        private void Reset()
        {
            List<RectTransform> rectTransforms = new List<RectTransform>();
            foreach (Transform item in transform)
                rectTransforms.Add(item as RectTransform);
            anchoredPositionsInChild = rectTransforms.ToArray();
        }
    }
}
