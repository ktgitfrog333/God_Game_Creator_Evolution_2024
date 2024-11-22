using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// FungusのFlowchartを管理
    /// モデル
    /// </summary>
    public class GuideMessageModel : MonoBehaviour
    {
        /// <summary>呼び出されたガイドメッセージID</summary>
        private readonly IReactiveProperty<GuideMessageID> _calledGuideMessageID = new ReactiveProperty<GuideMessageID>();
        /// <summary>呼び出されたガイドメッセージID</summary>
        public IReactiveProperty<GuideMessageID> CalledGuideMessageID => _calledGuideMessageID;

        public void CallGuideMessageID(GuideMessageID guideMessageID)
        {
            _calledGuideMessageID.Value = guideMessageID;
        }
    }
}
