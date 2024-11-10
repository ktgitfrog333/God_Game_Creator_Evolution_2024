using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// チュートリアル
    /// モデル
    /// </summary>
    public class TutorialModel : MonoBehaviour
    {
        /// <summary>チュートリアルモードであるか</summary>
        public IReactiveProperty<bool> IsTutorialMode => throw new System.NotImplementedException();
        /// <summary>FungusのFlowchartを管理のモデル</summary>
        [SerializeField] private GuideMessageModel guideMessageModel;
        /// <summary>FungusのFlowchartを管理のモデル</summary>
        public GuideMessageModel GuideMessageModel => guideMessageModel;

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
