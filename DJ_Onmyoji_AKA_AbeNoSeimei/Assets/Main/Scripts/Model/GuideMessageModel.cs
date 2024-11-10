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
        public IReactiveProperty<GuideMessageID> CalledGuideMessageID => throw new System.NotImplementedException();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
