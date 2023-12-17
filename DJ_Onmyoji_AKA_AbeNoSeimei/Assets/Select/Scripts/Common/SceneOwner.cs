using Universal.Template;
using Universal.Common;
using Universal.Bean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Select.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour, ISelectGameManager, ISceneOwner
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "MainScene";
        /// <summary>前のシーン名</summary>
        [SerializeField] private string backSceneName = "TitleScene";

        public void OnStart()
        {
            new TemplateResourcesAccessory();
        }

        public UserBean GetSaveDatas()
        {
            try
            {
                var temp = new TemplateResourcesAccessory();
                var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
                if (datas == null)
                    throw new System.Exception("リソース読み込みの失敗");

                return datas;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public bool SetSaveDatas(UserBean userBean)
        {
            try
            {
                var temp = new TemplateResourcesAccessory();
                if (!temp.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, userBean))
                    throw new System.Exception("Json保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void LoadTitleScene()
        {
            SceneManager.LoadScene(backSceneName);
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    /// <summary>
    /// シーンオーナー
    /// インターフェース
    /// </summary>
    public interface ISceneOwner
    {
        /// <summary>
        /// ユーザーデータを取得
        /// </summary>
        /// <returns>ユーザーデータ</returns>
        public UserBean GetSaveDatas();
        /// <summary>
        /// ユーザーデータを更新
        /// </summary>
        /// <param name="userBean">ユーザーデータ</param>
        /// <returns>成功／失敗</returns>
        public bool SetSaveDatas(UserBean userBean);
        /// <summary>
        /// タイトルシーンをロード
        /// </summary>
        public void LoadTitleScene();
        /// <summary>
        /// メインシーンをロード
        /// </summary>
        public void LoadMainScene();
    }
}
