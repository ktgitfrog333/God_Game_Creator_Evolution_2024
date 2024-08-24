using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Template;
using Title.Common;
using Universal.Bean;
using Universal.Common;

namespace Title.Test
{
    /// <summary>
    /// テスト用
    /// タイトルシーンのリソース管理
    /// </summary>
    public class TestTitleResourcesAccessory : MonoBehaviour
    {
        [SerializeField] private int[] inputSystemConfig;
        [SerializeField] private int[] inputMainSceneStagesCleared;
        [SerializeField] private int[] inputSystemCommonCash;
        [SerializeField] private int testMode;

        private void Start()
        {
            new TemplateResourcesAccessory();
        }

        public void OnClicked()
        {
            switch (testMode)
            {
                case 0:
                    TestCase_0();
                    break;
                case 1:
                    TestCase_1();
                    break;
                case 2:
                    TestCase_2();
                    break;
                case 3:
                    TestCase_3();
                    break;
                case 4:
                    TestCase_4();
                    break;
                case 5:
                    TestCase_5();
                    break;
                case 6:
                    TestCase_6();
                    break;
                case 7:
                    TestCase_7();
                    break;
                default:
                    Debug.LogError("例外ケース");
                    break;
            }
        }

        private void TestCase_4()
        {
            var temp = new TemplateResourcesAccessory();
            var bean = new UserBean();
            Debug.Log(temp.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, bean) ? "OK" : "NG");
        }

        private void TestCase_5()
        {
            var temp = new TemplateResourcesAccessory();
            var bean = new UserBean();
            bean.sceneId = 2;
            Debug.Log(temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA).sceneId == bean.sceneId ? "OK" : "NG");
        }

        private void TestCase_6()
        {
            var temp = new TemplateResourcesAccessory();
            var bean = new UserBean();
            bean.sceneId = 1;
            Debug.Log(temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, EnumLoadMode.Default).sceneId == bean.sceneId ? "OK" : "NG");
        }

        private void TestCase_7()
        {
            var temp = new TemplateResourcesAccessory();
            var bean = new UserBean();
            bean.sceneId = 1;
            Debug.Log(temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, EnumLoadMode.All).sceneId == bean.sceneId ? "OK" : "NG");
        }

        private void TestCase_0()
        {
            Debug.Log("---OnClicked---");
            var tTResources = new TemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tTResources.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, EnumLoadMode.Default);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            Debug.Log(datas.audioVolumeIndex);
            Debug.Log(datas.bgmVolumeIndex);
            Debug.Log(datas.seVolumeIndex);
            Debug.Log(datas.vibrationEnableIndex);
            Debug.Log("---SaveResourcesCSVOfSystemConfig---");
            //var configMap = new Dictionary<EnumSystemConfig, int>();
            var idx = 0;
            datas.audioVolumeIndex = inputSystemConfig[idx++];
            datas.bgmVolumeIndex = inputSystemConfig[idx++];
            datas.seVolumeIndex = inputSystemConfig[idx++];
            datas.vibrationEnableIndex = inputSystemConfig[idx++];
            if (!tTResources.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, datas))
                Debug.LogError("CSV保存呼び出しの失敗");
        }

        private void TestCase_1()
        {
            Debug.Log("---OnClicked---");
            var tTResources = new TemplateResourcesAccessory();
            Debug.Log("---LoadSaveDatasCSV---");
            var datas = tTResources.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            for (var i = 0; i < datas.state.Length; i++)
            {
                Debug.Log(datas.state[i]);
            }
            Debug.Log("---SaveDatasCSVOfMainSceneStagesCleared---");
            datas.state[1] = inputMainSceneStagesCleared[1];
            datas.state[2] = inputMainSceneStagesCleared[2];
            if (!tTResources.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, datas))
                Debug.LogError("CSV保存呼び出しの失敗");
        }

        private void TestCase_2()
        {
            Debug.Log("---OnClicked---");
            var tTResources = new TemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tTResources.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, EnumLoadMode.Default);
            Debug.Log("---SaveDatasCSVOfMainSceneStagesCleared---");
            if (!tTResources.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, datas))
                Debug.LogError("CSV保存呼び出しの失敗");
        }

        public void TestCase_3()
        {
            Debug.Log("---OnClicked---");
            var tSResources = new TemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tSResources.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            Debug.Log("---SaveResourcesCSVOfSystemCommonCash---");
            //var configMap = new Dictionary<EnumSystemConfig, int>();
            var idx = 0;
            datas.sceneId = inputSystemCommonCash[idx++];
            if (!tSResources.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, datas))
                Debug.LogError("CSV保存呼び出しの失敗");
        }
    }
}
