using Main.Model;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class EnemiesSpawnTutorialModelTest : MonoBehaviour
    {
        [SerializeField] private EnemiesSpawnTutorialModel enemiesSpawnTutorialModel;
        [SerializeField] private WrapTurretModel wrapTurretModel;
        [SerializeField] private DanceTurretModel danceTurretModel;
        [SerializeField] private GraffitiTurretModel graffitiTurretModel;
        [SerializeField] private OnmyoTurretModel[] onmyoTurretModels;
        [SerializeField] private int killedEnemyCount;
        [SerializeField] private Transform playerPrefab;

        private void Reset()
        {
            enemiesSpawnTutorialModel = GameObject.FindObjectOfType<EnemiesSpawnTutorialModel>();
        }

        private void Start()
        {
            Instantiate(playerPrefab, transform.position, Quaternion.identity, transform.parent);
        }

        private void Update()
        {
            if (wrapTurretModel == null)
                wrapTurretModel = GameObject.FindObjectOfType<WrapTurretModel>();
            if (danceTurretModel == null)
                danceTurretModel = GameObject.FindObjectOfType<DanceTurretModel>();
            if (graffitiTurretModel == null)
                graffitiTurretModel = GameObject.FindObjectOfType<GraffitiTurretModel>();
            if (onmyoTurretModels == null ||
                onmyoTurretModels.Length < 1)
                onmyoTurretModels = GameObject.FindObjectsOfType<OnmyoTurretModel>();
        }

        private void OnGUI()
        {
            // ボタンの配置やサイズを決定（x, y, width, height）
            if (GUI.Button(new Rect(10, 10, 100, 50), "Demo1"))
            {
                wrapTurretModel.SetAutoInstanceMode(false);
                danceTurretModel.SetAutoInstanceMode(false);
                graffitiTurretModel.SetAutoInstanceMode(false);
                foreach (var model in onmyoTurretModels)
                    model.SetAutoInstanceMode(false);

                enemiesSpawnTutorialModel.InstanceTamachans(killedEnemyCount);
            }

            if (GUI.Button(new Rect(10, 70, 100, 50), "Demo2"))
            {
                wrapTurretModel.SetAutoInstanceMode(false);
                danceTurretModel.SetAutoInstanceMode(false);
                graffitiTurretModel.SetAutoInstanceMode(false);
                foreach (var model in onmyoTurretModels)
                    model.SetAutoInstanceMode(false);

                enemiesSpawnTutorialModel.InstanceAuraTamachans(killedEnemyCount);
            }

            if (GUI.Button(new Rect(10, 130, 100, 50), "Empty"))
            {
            }
        }
    }
}
