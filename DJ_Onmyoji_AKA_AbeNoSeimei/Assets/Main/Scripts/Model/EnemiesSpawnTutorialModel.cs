using Main.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Universal.Bean;
using System.Linq;

namespace Main.Model
{
    /// <summary>
    /// チュートリアル用の敵スポーン制御
    /// モデル
    /// </summary>
    public class EnemiesSpawnTutorialModel : SpawnModel, IEnemiesSpawnTutorialModel
    {
        [SerializeField] private EnemiesSpawnTutorialStruct[] enemiesSpawnTutorialStructs;

        protected override void Start() { }

        protected override bool InstanceCloneObjects(float instanceRateTimeSec, ObjectsPoolModel objectsPoolModel)
        {
            throw new System.NotImplementedException();
        }

        // TODO: 引数へcntを追加。何番目のたまちゃんを生成するかを制御する。
        public bool InstanceTamachans()
        {
            try
            {
                var spawnUtility = new SpawnUtility();
                var enemiesSpawnTutorialStruct = enemiesSpawnTutorialStructs.FirstOrDefault(q => q.enemiesSpawnTutorialID == 0);
                if (!spawnUtility.ManageEnemiesSpawnTutorial(enemiesSpawnTutorialStruct,
                    _poolModel))
                    Debug.LogError("ManageEnemiesSpawnTutorial");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool InstanceAuraTamachans()
        {
            try
            {
                var spawnUtility = new SpawnUtility();
                var enemiesSpawnTutorialStruct = enemiesSpawnTutorialStructs.FirstOrDefault(q => q.enemiesSpawnTutorialID == 1);
                if (!spawnUtility.ManageEnemiesSpawnTutorial(enemiesSpawnTutorialStruct,
                    _poolModel))
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
        /// <returns>成功／失敗</returns>
        public bool InstanceTamachans();
        /// <summary>
        /// オーラ持ちたまちゃん3体を生成
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool InstanceAuraTamachans();
    }

    /// <summary>
    /// チュートリアル用の敵スポーン制御
    /// 構造体
    /// </summary>
    [System.Serializable]
    public struct EnemiesSpawnTutorialStruct
    {
        /// <summary>ID</summary>
        public int enemiesSpawnTutorialID;
        /// <summary>生成する位置</summary>
        public Vector2 instancePosition;
        /// <summary>移動速度を指定</summary>
        public float moveSpeed;
        /// <summary>生成する敵ID</summary>
        public EnemiesID enemiesID;
    }
}
