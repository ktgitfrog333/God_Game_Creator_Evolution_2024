using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Select.View
{
    /// <summary>
    /// 日本地図のビュー
    /// </summary>
    public class MapOfJapanView : MonoBehaviour, IStageContetsView
    {
        /// <summary>ロゴステージのビュー</summary>
        [SerializeField] private LogoStageView[] logoStageViews;

        private void Reset()
        {
            logoStageViews = GetComponentsInChildren<LogoStageView>();
        }

        public bool RenderTargetMark(int index)
        {
            return logoStageViews[index].RenderTargetMark();
        }
    }
}
