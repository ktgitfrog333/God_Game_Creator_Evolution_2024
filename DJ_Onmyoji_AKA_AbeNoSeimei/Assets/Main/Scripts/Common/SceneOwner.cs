using Universal.Template;
using Universal.Common;
using Universal.Bean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour, IMainGameManager, ISceneOwner
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "MainScene";
        /// <summary>前のシーン名</summary>
        [SerializeField] private string backSceneName = "SelectScene";

        public void OnStart()
        {
            new TemplateResourcesAccessory();
        }

        /// <summary>
        /// ユーザーデータを取得
        /// </summary>
        /// <returns>ユーザーデータ</returns>
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

        /// <summary>
        /// シーンIDをインクリメント（次のステージ番号）
        /// </summary>
        /// <param name="userBean">ユーザーデータ</param>
        /// <returns>更新後のユーザーデータ</returns>
        public UserBean CountUpSceneId(UserBean userBean)
        {
            try
            {
                var registedValue = userBean;
                var id = registedValue.sceneId;
                registedValue.sceneId = ++id;

                return registedValue;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// ユーザーデータを更新
        /// </summary>
        /// <param name="userBean">ユーザーデータ</param>
        /// <returns>成功／失敗</returns>
        public bool SetSaveDatas(UserBean userBean)
        {
            try
            {
                var temp = new TemplateResourcesAccessory();
                if (userBean == null)
                    throw new System.Exception("設定データがnull");
                if (!temp.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, userBean))
                    Debug.LogError("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// メインシーンをロード
        /// </summary>
        public void LoadMainScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        /// <summary>
        /// セレクトシーンをロード
        /// </summary>
        public void LoadSelectScene()
        {
            SceneManager.LoadScene(backSceneName);
        }
    }

    public interface ISceneOwner
    {
    }
}
