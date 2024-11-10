using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// チュートリアル用の敵スポーン制御
    /// モデル
    /// </summary>
    public class EnemiesSpawnTutorialModel : SpawnModel, IEnemiesSpawnTutorialModel
    {
        protected override bool InstanceCloneObjects(float instanceRateTimeSec, ObjectsPoolModel objectsPoolModel)
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool InstanceAuraTamachans()
        {
            throw new System.NotImplementedException();
        }

        public bool InstanceTamachans()
        {
            throw new System.NotImplementedException();
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
}
