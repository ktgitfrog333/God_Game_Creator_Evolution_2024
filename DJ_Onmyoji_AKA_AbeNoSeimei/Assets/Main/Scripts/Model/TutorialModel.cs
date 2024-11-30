using System.Collections;
using System.Collections.Generic;
using Title.Utility;
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
        private readonly BoolReactiveProperty _isTutorialMode = new BoolReactiveProperty();
        /// <summary>チュートリアルモードであるか</summary>
        public IReactiveProperty<bool> IsTutorialMode => _isTutorialMode;
        /// <summary>FungusのFlowchartを管理のモデル</summary>
        [SerializeField] private GuideMessageModel guideMessageModel;
        /// <summary>FungusのFlowchartを管理のモデル</summary>
        public GuideMessageModel GuideMessageModel => guideMessageModel;
        /// <summary>読み込み完了</summary>
        private readonly BoolReactiveProperty _isCompleted = new BoolReactiveProperty();
        /// <summary>読み込み完了</summary>
        public IReactiveProperty<bool> IsCompleted => _isCompleted;

        private void Reset()
        {
            guideMessageModel = GameObject.Find("Flowchart").GetComponent<GuideMessageModel>();
        }

        private void OnEnable()
        {
            guideMessageModel.gameObject.SetActive(true);
        }

        private void Start()
        {
            // ユーザデータ取得
            var utility = new TitleCommonUtility();
            var currentSceneId = utility.UserDataSingleton.UserBeanReloaded.sceneId;
            // シーンIDが8ならチュートリアルモード
            _isTutorialMode.Value = currentSceneId == 8;
            _isCompleted.Value = true;
        }

        private void OnDisable()
        {
            if (guideMessageModel != null)
                guideMessageModel.gameObject.SetActive(false);
        }
    }
}
