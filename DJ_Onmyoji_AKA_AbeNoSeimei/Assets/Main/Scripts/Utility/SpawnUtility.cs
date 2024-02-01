using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Model;
using Main.Test.Driver;
using UnityEngine;

namespace Main.Utility
{
    /// <summary>
    /// スポーンのユーティリティ
    /// </summary>
    public class SpawnUtility : ISpawnUtility, ISpawnUtilityTest
    {
        /// <summary>クローンオブジェクトを生成する時間間隔（秒）のホールド補正値</summary>
        private const float INSTANCE_RATE_TIME_SEC_STOP = 60f * 60f;

        public bool ManageBulletSpawn(JockeyCommandType jockeyCommandType, float instanceRateTimeSecCorrection, ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig config, ref float elapsedTime, System.Func<ObjectsPoolModel, OnmyoBulletConfig, bool> actionOfBullet)
        {
            try
            {
                float timeSec;
                switch (jockeyCommandType)
                {
                    case JockeyCommandType.Hold:
                        timeSec = INSTANCE_RATE_TIME_SEC_STOP;
                        break;
                    case JockeyCommandType.Scratch:
                        timeSec = config.actionRate / instanceRateTimeSecCorrection;
                        break;
                    default:
                        // デフォルト値
                        timeSec = config.actionRate;
                        break;
                }

                // 待機時間に到達していない場合はスキップ、到達していれば実行
                if (timeSec <= elapsedTime)
                {
                    if (!actionOfBullet(objectsPoolModel, config))
                        throw new System.Exception("ActionOfBullet");
                    elapsedTime = 0f;
                }
                else
                    elapsedTime += Time.deltaTime;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool ManageEnemiesSpawn(EnemiesSpawnTable enemiesSpawnTable, ref float elapsedTime, Transform target, ObjectsPoolModel objectsPoolModel, float radiusMin, float radiusMax, float onmyoState)
        {
            try
            {
                float timeSec = enemiesSpawnTable.instanceRateTimeSec;

                // 待機時間に到達していない場合はスキップ、到達していれば実行
                if (timeSec <= elapsedTime)
                {
                    for (int i = 0; i < enemiesSpawnTable.instanceCount; i++)
                    {
                        if (target != null)
                        {
                            var enemy = objectsPoolModel.GetEnemyModel(GetRandomEnemiesID(enemiesSpawnTable.enemiesIDs));
                            if (enemy == null)
                                throw new System.Exception("GetEnemyModel");
                            if (!enemy.Initialize(GetPositionOfAroundThePlayer(target, radiusMin, radiusMax), target))
                                throw new System.Exception("Initialize");
                            if (!enemy.isActiveAndEnabled)
                                enemy.gameObject.SetActive(true);
                            elapsedTime = 0f;
                        }
                        else
                            throw new System.Exception("ターゲット用のオブジェクトを配置して下さい");
                    }
                }
                else
                    elapsedTime += Time.deltaTime;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 引数で渡された敵ID配列からランダムで一つ選んで返す
        /// 引数の敵ID配列が一つのみだった場合はその値のみ返却
        /// 引数の敵ID配列の要素数が1より小さい場合は例外をスロー
        /// </summary>
        /// <param name="enemiesIDs">敵ID配列</param>
        /// <returns>敵ID</returns>
        /// <exception cref="System.Exception">敵ID配列の要素数が1より小さい場合</exception>
        private EnemiesID GetRandomEnemiesID(EnemiesID[] enemiesIDs)
        {
            if (enemiesIDs.Length < 1)
                throw new System.Exception("敵ID配列の要素数が1より小さい");

            if (enemiesIDs.Length == 1)
                return enemiesIDs[0];

            int randomIndex = Random.Range(0, enemiesIDs.Length);
            return enemiesIDs[randomIndex];
        }

        /// <summary>
        /// ターゲットの指定半径範囲内にランダムで位置情報を返す
        /// </summary>
        /// <param name="target">ターゲット</param>
        /// <param name="radiusMin">最小半径</param>
        /// <param name="radiusMax">最大半径</param>
        /// <returns>位置情報</returns>
        private Vector2 GetPositionOfAroundThePlayer(Transform target, float radiusMin, float radiusMax)
        {
            float distance = Random.Range(radiusMin, radiusMax);
            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector2 position = new Vector2(target.position.x + distance * Mathf.Cos(angle), target.position.y + distance * Mathf.Sin(angle));
            return position;
        }

        EnemiesID ISpawnUtilityTest.GetRandomEnemiesID(EnemiesID[] enemiesIDs)
        {
            return GetRandomEnemiesID(enemiesIDs);
        }
    }

    /// <summary>
    /// スポーンのユーティリティ
    /// インターフェース
    /// </summary>
    public interface ISpawnUtility
    {
        /// <summary>
        /// 魔力弾系のスポーン制御
        /// </summary>
        /// <param name="jockeyCommandType">ジョッキーコマンドタイプ</param>
        /// <param name="instanceRateTimeSecCorrection">クローンオブジェクトを生成する時間間隔（秒）のバフ補正値</param>
        /// <param name="objectsPoolModel">オブジェクトプール</param>
        /// <param name="config">魔力弾の設定</param>
        /// <param name="elapsedTime">経過時間</param>
        /// <param name="actionOfBullet">魔力弾／円舞範囲／デバフ魔力弾の制御</param>
        /// <returns>成功／失敗</returns>
        public bool ManageBulletSpawn(JockeyCommandType jockeyCommandType, float instanceRateTimeSecCorrection, ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig config, ref float elapsedTime, System.Func<ObjectsPoolModel, OnmyoBulletConfig, bool> actionOfBullet);
        /// <summary>
        /// 敵系のスポーン制御
        /// </summary>
        /// <param name="enemiesSpawnTable">敵のスポーンテーブル</param>
        /// <param name="elapsedTime"></param>
        /// <param name="target">経過時間</param>
        /// <param name="objectsPoolModel">オブジェクトプール</param>
        /// <param name="radiusMin">最小半径</param>
        /// <param name="radiusMax">最大半径</param>
        /// <returns>成功／失敗</returns>
        public bool ManageEnemiesSpawn(EnemiesSpawnTable enemiesSpawnTable, ref float elapsedTime, Transform target, ObjectsPoolModel objectsPoolModel, float radiusMin, float radiusMax, float onmyoState);
    }
}
