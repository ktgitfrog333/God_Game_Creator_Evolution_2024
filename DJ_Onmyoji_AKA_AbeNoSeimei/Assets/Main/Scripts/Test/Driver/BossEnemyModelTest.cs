using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Model;
using Main.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Main.Test.Driver
{
    public class BossEnemyModelTest : MonoBehaviour
    {
        [SerializeField] private BossEnemyModel bossEnemyModel;
        /// <summary>オブジェクトプールのプレハブ</summary>
        [SerializeField] private Transform objectsPoolPrefab;
        /// <summary>キング青行灯のプロパティ</summary>
        [SerializeField] private KingAoandonProp kingAoandonProp = new KingAoandonProp()
        {
            bossDirectionPhase = new IntReactiveProperty(),
            bossActionPhase = new IntReactiveProperty(),
            // 最初は攻撃を行わせたくない
            clearCount = new IntReactiveProperty(-1),
            targetClearedCountMax = 100,
            intervalTimeSec = 30f,
            instanceRateTimeSec = 10f,
            instanceDistanceMax = 100f,
            instanceCountRange = new int[]
            {
                4,
                32,
            },
            instanceDurations = new float[]
            {
                1f,
            },
        };
        private void Reset()
        {
            bossEnemyModel = GameObject.Find("BossEnemy01KingAoandon").GetComponent<BossEnemyModel>();
        }
        private void OnGUI()
        {
            if (GUI.Button(new Rect(20,40,80,20), $"BossEnemyModelTest"))
            {
                SpawnUtility spawnUtility = new SpawnUtility();
                ObjectsPoolModel objectsPoolModel = spawnUtility.FindOrInstantiateForGetObjectsPoolModel(objectsPoolPrefab);
                var prop = kingAoandonProp;
                Observable.FromCoroutine<bool>(observer => ((IBossEnemyModelTest)bossEnemyModel).InstanceEnemies(observer, objectsPoolModel, prop, bossEnemyModel.Transform.position))
                    .Subscribe(x => {})
                    .AddTo(gameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_PLAYER))
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x => kingAoandonProp.lastTarget = x.transform);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

    public interface IBossEnemyModelTest
    {
        public IEnumerator InstanceEnemies(System.IObserver<bool> observer, ObjectsPoolModel objectsPoolModel, KingAoandonProp kingAoandonProp, Vector3 instancePosition);
    }
}
