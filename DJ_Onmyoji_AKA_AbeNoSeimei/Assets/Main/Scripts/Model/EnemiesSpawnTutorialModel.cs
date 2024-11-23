using Main.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Universal.Bean;
using System.Linq;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// チュートリアル用の敵スポーン制御
    /// モデル
    /// </summary>
    public class EnemiesSpawnTutorialModel : SpawnModel, IEnemiesSpawnTutorialModel
    {
        /// <summary>チュートリアル用の敵スポーン制御の構造体</summary>
        [SerializeField]
        private EnemiesSpawnTutorialStruct[] enemiesSpawnTutorialStructs = new EnemiesSpawnTutorialStruct[]
        {
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 0,
                spawnIdx = 0,
                enemiesID = EnemiesID.EN0000,
                instancePosition = new Vector2(4.42f, 3.02f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 0,
                spawnIdx = 1,
                enemiesID = EnemiesID.EN0000,
                instancePosition = new Vector2(7.23999977f, -2.52999997f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 0,
                spawnIdx = 2,
                enemiesID = EnemiesID.EN0000,
                instancePosition = new Vector2(-3.69000006f, -2.75f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 0,
                enemiesID = EnemiesID.EN0000_W,
                instancePosition = new Vector2(3.73000002f, 2.75999999f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 1,
                enemiesID = EnemiesID.EN0000_W,
                instancePosition = new Vector2(-8.18999958f, -0.479999989f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 2,
                enemiesID = EnemiesID.EN0000_W,
                instancePosition = new Vector2(9.68000031f, -2.1099999f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 3,
                enemiesID = EnemiesID.EN0000_D,
                instancePosition = new Vector2(3.16000009f, 0.610000014f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 4,
                enemiesID = EnemiesID.EN0000_D,
                instancePosition = new Vector2(-2.74000001f, 1.5f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 5,
                enemiesID = EnemiesID.EN0000_D,
                instancePosition = new Vector2(-2.4000001f, -2.50999999f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 6,
                enemiesID = EnemiesID.EN0000_G,
                instancePosition = new Vector2(4.86999989f, -2.11999989f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 7,
                enemiesID = EnemiesID.EN0000_G,
                instancePosition = new Vector2(-5.07999992f, -1.63f),
            },
            new EnemiesSpawnTutorialStruct()
            {
                categoryID = 1,
                spawnIdx = 8,
                enemiesID = EnemiesID.EN0000_G,
                instancePosition = new Vector2(8.80000019f, -0.0500000007f),
            },
        };
        /// <summary>トランスフォーム</summary>
        private Transform _target;

        protected override void Start()
        {
            _poolModel = _spawnUtility.FindOrInstantiateForGetObjectsPoolModel(objectsPoolPrefab);
            Observable.FromCoroutine<Transform>(observer => WaitForTarget(observer))
                .Subscribe(x => _target = x)
                .AddTo(gameObject);
        }

        protected override bool InstanceCloneObjects(float instanceRateTimeSec, ObjectsPoolModel objectsPoolModel)
        {
            throw new System.NotImplementedException();
        }

        public bool InstanceTamachans(int killedEnemyCount)
        {
            try
            {
                var spawnUtility = new SpawnUtility();
                var enemiesSpawnTutorialStruct = enemiesSpawnTutorialStructs.FirstOrDefault(q => q.categoryID == 0 &&
                    q.spawnIdx == killedEnemyCount);
                if (!spawnUtility.ManageEnemiesSpawnTutorial(enemiesSpawnTutorialStruct,
                    _poolModel,
                    _target))
                    Debug.LogError("ManageEnemiesSpawnTutorial");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool InstanceAuraTamachans(int killedEnemyCount)
        {
            try
            {
                var spawnUtility = new SpawnUtility();
                var enemiesSpawnTutorialStruct = enemiesSpawnTutorialStructs.FirstOrDefault(q => q.categoryID == 1 &&
                    q.spawnIdx == killedEnemyCount);
                if (!spawnUtility.ManageEnemiesSpawnTutorial(enemiesSpawnTutorialStruct,
                    _poolModel,
                    _target))
                    Debug.LogError("ManageEnemiesSpawnTutorial");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// チュートリアル用の敵スポーン制御
    /// インターフェース
    /// </summary>
    public interface IEnemiesSpawnTutorialModel
    {
        /// <summary>
        /// たまちゃん3体を生成
        /// </summary>
        /// <param name="killedEnemyCount">敵の撃破数（何番目に出すか）</param>
        /// <returns>成功／失敗</returns>
        public bool InstanceTamachans(int killedEnemyCount);
        /// <summary>
        /// オーラ持ちたまちゃん3体を生成
        /// </summary>
        /// <param name="killedEnemyCount">敵の撃破数（何番目に出すか）</param>
        /// <returns>成功／失敗</returns>
        public bool InstanceAuraTamachans(int killedEnemyCount);
    }

    /// <summary>
    /// チュートリアル用の敵スポーン制御
    /// 構造体
    /// </summary>
    [System.Serializable]
    public struct EnemiesSpawnTutorialStruct
    {
        /// <summary>カテゴリID</summary>
        public int categoryID;
        /// <summary>何番目にスポーンするか</summary>
        public int spawnIdx;
        /// <summary>生成する位置</summary>
        public Vector2 instancePosition;
        /// <summary>生成する敵ID</summary>
        public EnemiesID enemiesID;
    }
}
