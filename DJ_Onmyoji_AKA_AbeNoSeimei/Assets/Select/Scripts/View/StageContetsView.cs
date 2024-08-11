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
    public class StageContetsView : MonoBehaviour, IStageContetsView
    {
        /// <summary>子要素のトランスフォーム/summary>
        [SerializeField] private RectTransform[] anchoredPositionsInChild;
        /// <summary>子要素のトランスフォーム</summary>
        public RectTransform[] AnchoredPositionsInChild => anchoredPositionsInChild;
        /// <summary>子要素のトランスフォーム/summary>
        [SerializeField] private RectTransform[] anchoredPositionsInChild1;
        /// <summary>子要素のトランスフォーム</summary>
        public RectTransform[] AnchoredPositionsInChild1 => anchoredPositionsInChild1;
        /// <summary>ロゴステージのビュー</summary>
        [SerializeField] private LogoStageView[] logoStageViews;

        private void Reset()
        {
            List<RectTransform> rectTransforms = new List<RectTransform>();
            foreach (Transform item in transform)
                rectTransforms.Add(item as RectTransform);
            anchoredPositionsInChild = rectTransforms.ToArray();
            logoStageViews = GetComponentsInChildren<LogoStageView>();
        }

        public bool RenderTargetMark(int index)
        {
            return logoStageViews[index].RenderTargetMark();
        }
    }

    /// <summary>
    /// ステージコンテンツ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IStageContetsView
    {
        /// <summary>
        /// 選択済みマークを表示
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>成功／失敗</returns>
        public bool RenderTargetMark(int index);
    }
}
