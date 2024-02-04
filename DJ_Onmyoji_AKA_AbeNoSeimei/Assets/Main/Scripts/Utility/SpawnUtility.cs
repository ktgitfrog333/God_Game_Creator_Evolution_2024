using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Model;
using UniRx;
using UnityEngine;

namespace Main.Utility
{
    /// <summary>
    /// スポーンのユーティリティ
    /// </summary>
    public class SpawnUtility : ISpawnUtility
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

        public bool ManageEnemiesSpawn(EnemiesSpawnTable[] enemiesSpawnTables, ref float elapsedTime, Transform target, ObjectsPoolModel objectsPoolModel, float radiusMin, float radiusMax, float onmyoState)
        {
            try
            {
                float timeSec = enemiesSpawnTables[0].instanceRateTimeSec;

                // 待機時間に到達していない場合はスキップ、到達していれば実行
                if (timeSec <= elapsedTime)
                {
                    foreach (var enemiesSpawnTable in enemiesSpawnTables)
                    {
                        var resultMaxCount = GetCalcMaxCountAndAddRemaining(enemiesSpawnTable, onmyoState);
                        for (int i = 0; i < resultMaxCount; i++)
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
        /// 最大カウント数の計算を行い結果を取得
        /// 計算後の端数（小数点以下の値）は格納しておく
        /// </summary>
        /// <param name="enemiesSpawnTable">敵のスポーンテーブル</param>
        /// <param name="onmyoState">陰陽（昼夜）の状態</param>
        /// <returns>端数切捨て後の最大カウント数</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">onmyoStateが想定範囲外である場合に例外をスロー</exception>
        private int GetCalcMaxCountAndAddRemaining(EnemiesSpawnTable enemiesSpawnTable, float onmyoState)
        {
            if (onmyoState <= 1f &&
                -1f <= onmyoState)
            {
                if (enemiesSpawnTable.instanceCountRemaining == null)
                    enemiesSpawnTable.instanceCountRemaining = new FloatReactiveProperty();
                var calcOnmyoState = enemiesSpawnTable.sunMoonState switch
                {
                    SunMoonState.Daytime => (onmyoState + 1) / 2,
                    SunMoonState.Night => (onmyoState - 1) / 2,
                    _ => throw new System.Exception("例外をスロー"),
                };
                var result = (enemiesSpawnTable.instanceCount + enemiesSpawnTable.instanceCountRemaining.Value) * Mathf.Abs(calcOnmyoState) / 1f;
                enemiesSpawnTable.instanceCountRemaining.Value = result - Mathf.Floor(result);

                return (int)result;
            }
            else
                throw new System.ArgumentOutOfRangeException($"onmyoStateが範囲外:[{onmyoState}]");
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
        /// <param name="enemiesSpawnTables">敵のスポーンテーブル配列</param>
        /// <param name="elapsedTime">経過時間</param>
        /// <param name="target">トランスフォーム</param>
        /// <param name="objectsPoolModel">オブジェクトプール</param>
        /// <param name="radiusMin">最小半径</param>
        /// <param name="radiusMax">最大半径</param>
        /// <param name="onmyoState">陰陽（昼夜）の状態</param>
        /// <returns>成功／失敗</returns>
        public bool ManageEnemiesSpawn(EnemiesSpawnTable[] enemiesSpawnTables, ref float elapsedTime, Transform target, ObjectsPoolModel objectsPoolModel, float radiusMin, float radiusMax, float onmyoState);
    }
}
