using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Model;
using UniRx;
using UnityEngine;

namespace Main.Utility
{
    /// <summary>
    /// 敵とプレイヤーのユーティリティ
    /// </summary>
    public class EnemyPlayerModelUtility : IEnemyPlayerModelUtility
    {
        public IEnumerator InstanceEnemiesOfBossEnemyModel(System.IObserver<bool> observer, ObjectsPoolModel objectsPoolModel, KingAoandonProp kingAoandonProp, Transform fromTransform)
        {
            observer.OnNext(true);
            while (true)
            {
                int instanceCount = Random.Range(kingAoandonProp.instanceCountRange[0], kingAoandonProp.instanceCountRange[1] + 1);
                float angleStep = 360f / instanceCount;
                for (int i = 0; i < instanceCount; i++)
                {
                    var targetPosition = GetTargetPosition(angleStep * i, kingAoandonProp.instanceDistanceMax, kingAoandonProp.lastTarget);
                    var instance = objectsPoolModel.GetEnemyModel(EnemiesID.EN0005);
                    if (!instance.isActiveAndEnabled)
                    {
                        if (!instance.SetEnabledOfColliders(false))
                            Debug.LogError("SetEnabledOfColliders");
                        instance.gameObject.SetActive(true);
                    }
                    // 購読を管理する新しいメソッドを呼び出し
                    if (!instance.SubscribeToIsDead(instance.State.IsDead.ObserveEveryValueChanged(x => x.Value), () =>
                    {
                        kingAoandonProp.clearCount.Value++;
                    }))
                        Debug.LogError("SubscribeToIsDead");
                    instance.Transform.position = fromTransform.position;
                    if (!instance.MoveTowards(targetPosition, kingAoandonProp))
                        Debug.LogError("MoveTowards");
                }

                yield return new WaitForSeconds(kingAoandonProp.instanceRateTimeSec);
            }
        }

        public bool IsCompareTagAndUpdateReactiveFlag(Collider2D other, string[] tags, IReactiveProperty<bool> isHit)
        {
            foreach (var tag in tags)
            {
                if (other.CompareTag(tag))
                {
                    if (!isHit.Value)
                    {
                        isHit.Value = true;
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public bool IsCompareTagAndUpdateReactiveFlagPublic(Collider2D other, string[] tags, IReactiveProperty<bool> isHit)
        {
            if (!isHit.Value)
            {
                isHit.Value = true;
                return true;
            }
            return false;
        }


        public bool MoveTowardsOfEnemyModel(Transform enemyTransform, ref Vector3 targetPosition, float moveSpeedBlendDeltaTime, Vector3 firstActivePosition, float spinSpeedBlendDeltaTime, ref float leapLevel, KingAoandonProp kingAoandonProp)
        {
            try
            {
                Vector3[] points = new Vector3[]
                {
                    Vector3.MoveTowards(enemyTransform.position, targetPosition, moveSpeedBlendDeltaTime),
                    Vector3.MoveTowards(enemyTransform.position, kingAoandonProp.lastTarget.position, moveSpeedBlendDeltaTime),
                };
                enemyTransform.position = Vector3.Lerp(points[0], points[1], leapLevel);
                float radius = Vector3.Distance(firstActivePosition, targetPosition);
                float currentAngle = Mathf.Atan2(targetPosition.y - firstActivePosition.y, targetPosition.x - firstActivePosition.x) * Mathf.Rad2Deg;
                currentAngle += spinSpeedBlendDeltaTime; // 時計回りに角度を増やす
                if (currentAngle >= 360f) currentAngle -= 360f; // 角度が360を超えたらリセット

                // 新しいtarget位置を計算
                float newX = firstActivePosition.x + radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
                float newY = firstActivePosition.y + radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
                targetPosition = new Vector3(newX, newY, targetPosition.z);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ターゲットポジションを取得
        /// </summary>
        /// <param name="sourceAngle">元の角度</param>
        /// <param name="distance">距離</param>
        /// <returns>ターゲットポジション</returns>
        private Vector3 GetTargetPosition(float sourceAngle, float distance, Transform target)
        {
            float angle = sourceAngle * Mathf.Deg2Rad;
            return target.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
        }

        public bool UpdateStateHPAndIsDead(CharacterState state)
        {
            try
            {
                state.IsHit.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (x)
                            if (state.Damage.Value < state.HP.Value)
                                state.HP.Value -= state.Damage.Value;
                            else
                            {
                                state.HP.Value = 0;
                                state.IsDead.Value = x;
                            }
                    });

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
    /// 敵とプレイヤーのユーティリティ
    /// インターフェース
    /// </summary>
    public interface IEnemyPlayerModelUtility
    {
        /// <summary>
        /// 対象がタグ内に含まれている場合はフラグを更新
        /// </summary>
        /// <param name="other">衝突した対象</param>
        /// <param name="tags">対象タグ</param>
        /// <param name="isHit">ヒットフラグ</param>
        /// <returns></returns>
        public bool IsCompareTagAndUpdateReactiveFlag(Collider2D other, string[] tags, IReactiveProperty<bool> isHit);
        /// <summary>
        /// ステータスを更新する
        /// HPを加算してMAXを超えると死亡フラグを有効にする
        /// </summary>
        /// <param name="state">キャラクターのステータス</param>
        /// <returns>成功／失敗</returns>
        public bool UpdateStateHPAndIsDead(CharacterState state);
        /// <summary>
        /// 敵を生成
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="objectsPoolModel">オブジェクトプールのモデル</param>
        /// <param name="kingAoandonProp">キング青行灯のプロパティ</param>
        /// <param name="fromTransform">fromトランスフォーム</param>
        /// <returns>コルーチン</returns>
        public IEnumerator InstanceEnemiesOfBossEnemyModel(System.IObserver<bool> observer, ObjectsPoolModel objectsPoolModel, KingAoandonProp kingAoandonProp, Transform fromTransform);
        /// <summary>
        /// 追尾移動かつ回転
        /// </summary>
        /// <param name="enemyTransform">敵のトランスフォーム</param>
        /// <param name="targetPosition">目標位置</param>
        /// <param name="moveSpeedBlendDeltaTime">移動速度</param>
        /// <param name="firstActivePosition">生成位置</param>
        /// <param name="spinSpeedBlendDeltaTime">回転移動速度</param>
        /// <param name="leapLevel">リープレベル</param>
        /// <param name="kingAoandonProp">キング青行灯のプロパティ</param>
        /// <returns>成功／失敗</returns>
        public bool MoveTowardsOfEnemyModel(Transform enemyTransform, ref Vector3 targetPosition, float moveSpeedBlendDeltaTime, Vector3 firstActivePosition, float spinSpeedBlendDeltaTime, ref float leapLevel, KingAoandonProp kingAoandonProp);
    }
}
