using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Bean;
using Universal.Template;

namespace Universal.Common
{
    /// <summary>
    /// 管理者データ
    /// シングルトン
    /// </summary>
    public class AdminDataSingleton : MonoBehaviour
    {
        private static AdminDataSingleton instance;

        private AdminDataSingleton() {}
        private AdminBean _adminBean;
        public AdminBean AdminBean => _adminBean != null ?
            _adminBean :
            _adminBean = new TemplateResourcesAccessory().LoadSaveDatasJsonOfAdminBean(ConstResorcesNames.ADMIN_DATA);

        public static AdminDataSingleton Instance => instance;

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
