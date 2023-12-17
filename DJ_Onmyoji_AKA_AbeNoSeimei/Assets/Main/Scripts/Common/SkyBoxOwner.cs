using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Template;
using Universal.Common;

namespace Main.Common
{
    /// <summary>
    /// SkyBoxのオーナー
    /// </summary>
    public class SkyBoxOwner : MonoBehaviour, IMainGameManager
    {
        /// <summary>ステージごとのSkybox</summary>
        [SerializeField] private Material[] skyboxs;

        public void OnStart()
        {
            var temp = new TemplateResourcesAccessory();
            // ステージIDの取得
            // ステージ共通設定の取得
            var userDatas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            var adminDatas = temp.LoadSaveDatasJsonOfAdminBean(ConstResorcesNames.ADMIN_DATA);

            // Skyboxの設定
            RenderSettings.skybox = skyboxs[adminDatas.skyBoxs[userDatas.sceneId - 1] - 1];
        }
    }
}
