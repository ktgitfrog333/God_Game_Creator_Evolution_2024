using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Common;

namespace Select.Utility
{
    /// <summary>
    /// 共通のユーティリティ
    /// </summary>
    public class SelectCommonUtility
    {
        /// <summary>管理者データ</summary>
        private AdminDataSingleton _adminDataSingleton;
        /// <summary>管理者データ</summary>
        public AdminDataSingleton AdminDataSingleton => _adminDataSingleton != null ? _adminDataSingleton : GetAdminDataSingleton();
        /// <summary>管理者データ</summary>
        private AdminDataSingleton GetAdminDataSingleton()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            return adminDataSingleton;
        }
        /// <summary>ユーザデータ</summary>
        private UserDataSingleton _userDataSingleton;
        /// <summary>ユーザデータ</summary>
        public UserDataSingleton UserDataSingleton => _userDataSingleton != null ? _userDataSingleton : GetUserDataSingleton();
        /// <summary>ユーザデータ</summary>
        private UserDataSingleton GetUserDataSingleton()
        {
            var singleton = UserDataSingleton.Instance != null ?
                UserDataSingleton.Instance :
                new GameObject(ConstGameObjectNames.GAMEOBJECT_NAME_USERDATA_SINGLETON).AddComponent<UserDataSingleton>()
                    .GetComponent<UserDataSingleton>();
            return singleton;
        }
    }
}
