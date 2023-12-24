using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 敵
    /// モデル
    /// </summary>
    public class EnemyModel : MobCharacter, IEnemyModel
    {
        /// <summary>ダメージ判定</summary>
        [Tooltip("ダメージ判定")]
        [SerializeField] private DamageSufferedZoneOfEnemyModel damageSufferedZoneModel;
        /// <summary>当たったか</summary>
        public IReactiveProperty<bool> IsHit => damageSufferedZoneModel.IsHit;
        /// <summary>移動速度</summary>
        private float _moveSpeed;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>敵の設定</summary>
        [SerializeField] private EnemyConfig enemyConfig;
        /// <summary>ターゲット</summary>
        private Transform _target;

        public bool Initialize(Vector2 position, Transform target)
        {
            try
            {
                _moveSpeed = enemyConfig.moveSpeed;
                Transform.position = position;
                if (_target == null)
                    _target = target;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        protected override void Reset()
        {
            damageSufferedZoneModel = GetComponentInChildren<DamageSufferedZoneOfEnemyModel>();
            enemyConfig.moveSpeed = 3f;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start() { }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                var targetDirection = _target.position - Transform.position;
                var moveDirection = targetDirection.normalized;
                // 指定された方向と速度に弾を移動させる
                Transform.position += moveDirection * _moveSpeed * Time.fixedDeltaTime;
            }
        }
    }

    /// <summary>
    /// 敵
    /// 設定
    /// </summary>
    [System.Serializable]
    public struct EnemyConfig
    {
        /// <summary>移動速度</summary>
        [Tooltip("移動速度")]
        public float moveSpeed;
    }

    /// <summary>
    /// 敵
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IEnemyModel
    {
        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="position">生成位置</param>
        /// <param name="target">ターゲット</param>
        /// <returns>成功／失敗</returns>
        public bool Initialize(Vector2 position, Transform target);
    }
}
