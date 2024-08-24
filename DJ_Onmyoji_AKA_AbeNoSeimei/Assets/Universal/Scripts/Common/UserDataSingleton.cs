using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Bean;
using Universal.Template;

namespace Universal.Common
{
    /// <summary>
    /// ユーザデータ
    /// シングルトン
    /// </summary>
    public class UserDataSingleton : MonoBehaviour, IUserDataSingleton
    {
        private static UserDataSingleton instance;

        private UserDataSingleton() {}
        private UserBean _userBean;
        public UserBean UserBean => _userBean != null ?
            _userBean :
            _userBean = new TemplateResourcesAccessory().LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);

        public static UserDataSingleton Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                // デモ版はシーンロードする度にファイルを読み込ませたいためDestroyする
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool SetAndSaveUserBean(UserBean userBean)
        {
            try
            {
                _userBean = userBean;
                if (!new TemplateResourcesAccessory().SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, _userBean))
                    throw new System.Exception("SaveDatasJsonOfUserBean");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// ユーザデータ
    /// シングルトン
    /// インターフェース
    /// </summary>
    public interface IUserDataSingleton
    {
        /// <summary>
        /// ユーザデータのセットとセーブ
        /// </summary>
        /// <param name="userBean">ユーザデータ</param>
        /// <returns>成功／失敗</returns>
        public bool SetAndSaveUserBean(UserBean userBean);
    }
}
