using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Accessory;
using Universal.Common;
using Universal.Bean;

namespace Universal.Template
{
    /// <summary>
    /// リソースアクセスのテンプレート
    /// </summary>
    public class TemplateResourcesAccessory
    {
        /// <summary>
        /// リソースアクセスのテンプレート
        /// コンストラクタ
        /// </summary>
        public TemplateResourcesAccessory()
        {
            new ResourcesAccessory().Initialize();
        }

        /// <summary>
        /// JSONデータからユーザー情報を取得します
        /// </summary>
        /// <param name="resourcesLoadName">リソースJSONファイル名</param>
        /// <returns>ユーザー情報</returns>
        public UserBean LoadSaveDatasJsonOfUserBean(string resourcesLoadName, EnumLoadMode enumLoadMode=EnumLoadMode.Continue)
        {
            return new ResourcesAccessory().LoadSaveDatasJsonOfUserBean(resourcesLoadName, enumLoadMode);
        }

        public AdminBean LoadSaveDatasJsonOfAdminBean(string resourcesLoadName)
        {
            return new ResourcesAccessory().LoadSaveDatasJsonOfAdminBean(resourcesLoadName);
        }

        /// <summary>
        /// ユーザー情報をJSONデータとして保存します
        /// </summary>
        public bool SaveDatasJsonOfUserBean(string resourcesLoadName, UserBean userBean)
        {
            return new ResourcesAccessory().SaveDatasJsonOfUserBean(resourcesLoadName, userBean);
        }

        public bool SaveDatasJsonOfAdminBean(string resourcesLoadName, AdminBean adminBean)
        {
            return new ResourcesAccessory().SaveDatasJsonOfAdminBean(resourcesLoadName, adminBean);
        }

        /// <summary>
        /// シーンの状態を更新します
        /// </summary>
        public UserBean UpdateSceneStates(UserBean continues, UserBean defaults)
        {
            return new ResourcesAccessory().UpdateSceneStates(continues, defaults);
        }

        /// <summary>
        /// オーディオと振動の設定を更新します
        /// </summary>
        public UserBean UpdateAudioAndVibration(UserBean continues, UserBean defaults)
        {
            return new ResourcesAccessory().UpdateAudioAndVibration(continues, defaults);
        }
    }
}
