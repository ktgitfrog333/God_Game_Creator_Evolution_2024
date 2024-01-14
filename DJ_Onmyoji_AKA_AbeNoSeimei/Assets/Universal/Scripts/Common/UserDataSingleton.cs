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
    public class UserDataSingleton : MonoBehaviour
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
    }
}
