using System.Collections;
using System.Collections.Generic;
using Universal.Template;
using Universal.Common;
using Universal.Bean;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Title.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour, ITitleGameManager
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "SelectScene";

        public void OnStart()
        {
            new TemplateResourcesAccessory();
        }

        /// <summary>
        /// ステージクリア済みデータの削除
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool DestroyMainSceneStagesState()
        {
            try
            {
                var temp = new TemplateResourcesAccessory();
                var bean = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
                var beanDefault = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, EnumLoadMode.Default);
                var beanUpdate = temp.UpdateSceneStates(bean, beanDefault);
                if (beanUpdate == null)
                    throw new System.Exception("シーン更新の失敗");
                if (!temp.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, beanUpdate))
                    throw new System.Exception("Json保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 全ステージの選択を有効にする
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool AllReleasedMainSceneStagesState()
        {
            try
            {
                var temp = new TemplateResourcesAccessory();
                var bean = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
                var beanAll = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, EnumLoadMode.All);
                var beanUpdate = temp.UpdateSceneStates(bean, beanAll);
                if (beanUpdate == null)
                    throw new System.Exception("シーン更新の失敗");
                if (!temp.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, beanUpdate))
                    throw new System.Exception("Json保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// シーン読み込み
        /// </summary>
        public void LoadNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
