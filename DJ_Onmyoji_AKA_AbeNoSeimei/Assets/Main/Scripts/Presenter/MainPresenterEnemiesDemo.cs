using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;
using UniRx;

namespace Main.Presenter
{
    public class MainPresenterEnemiesDemo : MonoBehaviour
    {
        [SerializeField] private EnemyEventSystemModel enemyEventSystemModel;
        [SerializeField] private SpawnSoulMoneyModel spawnSoulMoneyModel;

        private void Reset()
        {
            enemyEventSystemModel = GameObject.Find("EnemyEventSystem").GetComponent<EnemyEventSystemModel>();
            spawnSoulMoneyModel = GameObject.Find("SpawnSoulMoney").GetComponent<SpawnSoulMoneyModel>();
        }

        // Start is called before the first frame update
        void Start()
        {
            enemyEventSystemModel.OnEnemyDead.Subscribe(enemyModel =>
            {
                if (!spawnSoulMoneyModel.InstanceCloneObjects(enemyModel.transform.position, enemyModel.EnemiesProp))
                    Debug.LogError("InstanceCloneObjects");
            });
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
