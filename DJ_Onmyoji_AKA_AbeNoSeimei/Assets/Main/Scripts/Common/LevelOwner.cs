using Universal.Template;
using Universal.Common;
using Universal.Bean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Main.Common
{
    /// <summary>
    /// レベルのオーナー
    /// </summary>
    public class LevelOwner : MonoBehaviour, IMainGameManager
    {
        /// <summary>レベルの親オブジェクト</summary>
        [SerializeField] private Transform level;
        /// <summary>各ステージのプレハブ</summary>
        [SerializeField] private GameObject[] levelPrefabs;
        /// <summary>レベルがインスタンス済みか</summary>
        private readonly BoolReactiveProperty _isInstanced = new BoolReactiveProperty();
        /// <summary>レベルがインスタンス済みか</summary>
        public IReactiveProperty<bool> IsInstanced => _isInstanced;
        /// <summary>インスタンス済みレベル</summary>
        private Transform _instancedLevel;
        /// <summary>インスタンス済みレベル</summary>
        public Transform InstancedLevel => _instancedLevel;

        private void Reset()
        {
            level = GameObject.Find("Level").transform;
        }

        public void OnStart()
        {
            var disableLevels = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_LEVEL);
            if (0 < disableLevels.Length)
            {
                Debug.LogWarning("完成版ではレベルのプレハブをヒエラルキーから削除して下さい");
                foreach (var level in disableLevels)
                {
                    Debug.LogWarning($"レベル:[{level.name}]を無効化しました");
                    level.SetActive(false);
                }
            }
            // シーンIDを取得してステージをLevelオブジェクトの子要素としてインスタンスする
            var temp = new TemplateResourcesAccessory();
            // ステージIDの取得
            var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);

            _instancedLevel = Instantiate(levelPrefabs[datas.sceneId], Vector3.zero, Quaternion.identity, level).transform;
            if (_instancedLevel != null)
                _isInstanced.Value = true;
        }
    }
}
