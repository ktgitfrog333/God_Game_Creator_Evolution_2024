using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Universal.Utility;

namespace Main.Model
{
    /// <summary>
    /// ボス敵
    /// モデル
    /// </summary>
    public class BossEnemyModel : EnemyModel, IBossEnemyModel
    {
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
            instanceDistanceMax = 8f,
            instanceCountRange = new int[]
            {
                4,
                32,
            },
            instanceDurations = new float[]
            {
                1f,
                .1f,
                1f,
                5f,
                10f,
            },
            moveSpeed = 4f,
            spinSpeed = 10f,
        };
        /// <summary>キング青行灯のプロパティ</summary>
        public KingAoandonProp KingAoandonProp => kingAoandonProp;
        /// <summary>オブジェクトプールのプレハブ</summary>
        [SerializeField] private Transform objectsPoolPrefab;

        private void OnEnable()
        {
            kingAoandonProp.bossDirectionPhase.Value = (int)BossDirectionPhase.Entrance;
        }

        protected override void Start()
        {
            base.Start();
            State.IsDead.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ => kingAoandonProp.bossDirectionPhase.Value = (int)BossDirectionPhase.Exit);
            this.UpdateAsObservable()
                .Select(_ => GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_PLAYER))
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x => kingAoandonProp.lastTarget = x.transform);
            SpawnUtility spawnUtility = new SpawnUtility();
            ObjectsPoolModel objectsPoolModel = spawnUtility.FindOrInstantiateForGetObjectsPoolModel(objectsPoolPrefab);
            System.IDisposable instanceEnemies = this.UpdateAsObservable().Subscribe(_ => {});
            kingAoandonProp.clearCount.ObserveEveryValueChanged(x => x.Value)
                .Where(_ => isActiveAndEnabled)
                .Subscribe(x =>
                {
                    // 撃破目標数を超えた場合
                    if (kingAoandonProp.targetClearedCountMax <= x)
                    {
                        if (kingAoandonProp.bossActionPhase.Value != (int)BossActionPhase.DamageAll)
                        {
                            instanceEnemies.Dispose();
                            kingAoandonProp.bossActionPhase.Value = (int)BossActionPhase.DamageAll;
                            // ダウン状態⇒攻撃までのインターバル
                            Observable.FromCoroutine<bool>(observer => GeneralUtility.ActionsAfterDelay(kingAoandonProp.intervalTimeSec, () => kingAoandonProp.clearCount.Value = 0))
                                .Subscribe(_ => {})
                                .AddTo(gameObject);
                            if (!SetEnabledOfColliders(true))
                                Debug.LogError("SetEnabledOfColliders");
                            if (!objectsPoolModel.KillEnemyModels(EnemiesID.EN0005))
                                Debug.LogError("KillEnemyModels");
                        }
                    }
                    // 目標数の半分を超えた場合（右手解除）
                    else if ((kingAoandonProp.targetClearedCountMax / 2) <= x)
                    {
                        kingAoandonProp.bossActionPhase.Value = (int)BossActionPhase.DamageRight;
                    }
                    // ボス攻撃中
                    else if (-1 < x)
                    {
                        if (kingAoandonProp.bossActionPhase.Value != (int)BossActionPhase.Attack)
                        {
                            kingAoandonProp.bossActionPhase.Value = (int)BossActionPhase.Attack;
                            instanceEnemies = Observable.FromCoroutine<bool>(observer => _utility.InstanceEnemiesOfBossEnemyModel(observer, objectsPoolModel, kingAoandonProp, Transform))
                                .Subscribe(_ => {})
                                .AddTo(gameObject);
                            if (!SetEnabledOfColliders(false))
                                Debug.LogError("SetEnabledOfColliders");
                        }
                    }
                });
            if (!SetEnabledOfColliders(false))
                Debug.LogError("SetEnabledOfColliders");
        }

        public bool SetClearCount(int clearCount)
        {
            try
            {
                kingAoandonProp.clearCount.Value = clearCount;

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
    /// ボス敵
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IBossEnemyModel
    {
        /// <summary>
        /// 撃破数をセット
        /// </summary>
        /// <param name="clearCount">撃破数</param>
        /// <returns>成功／失敗</returns>
        public bool SetClearCount(int clearCount);
    }

    /// <summary>
    /// キング青行灯のプロパティ
    /// </summary>
    [System.Serializable]
    public struct KingAoandonProp
    {
        /// <summary>ボス演出フェーズ</summary>
        public IReactiveProperty<int> bossDirectionPhase;
        /// <summary>ボス行動フェーズ</summary>
        public IReactiveProperty<int> bossActionPhase;
        /// <summary>撃破数</summary>
        public IReactiveProperty<int> clearCount;
        /// <summary>撃破目標数</summary>
        public int targetClearedCountMax;
        /// <summary>インターバル時間（秒）</summary>
        public float intervalTimeSec;
        /// <summary>生成するレート</summary>
        public float instanceRateTimeSec;
        /// <summary>生成する最大距離</summary>
        public float instanceDistanceMax;
        /// <summary>生成数の最小～最大範囲</summary>
        public int[] instanceCountRange;
        /// <summary>アニメーション終了時間</summary>
        public float[] instanceDurations;
        /// <summary>最終的なターゲット</summary>
        public Transform lastTarget;
        /// <summary>移動速度</summary>
        public float moveSpeed;
        /// <summary>回転移動速度</summary>
        public float spinSpeed;
    }
}
