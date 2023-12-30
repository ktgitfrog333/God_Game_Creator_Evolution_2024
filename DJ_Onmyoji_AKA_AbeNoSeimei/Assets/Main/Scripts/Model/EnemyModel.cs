using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UniRx;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using Universal.Common;

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
        /// <summary>移動速度</summary>
        private float _moveSpeed;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>ターゲット</summary>
        private Transform _target;
        /// <summary>ユーティリティ</summary>
        private EnemyPlayerModelUtility _utility = new EnemyPlayerModelUtility();
        /// <summary>プロパティ</summary>
        [SerializeField] private CharacterProp prop;
        /// <summary>ステータス</summary>
        public CharacterState State { get; private set; }

        public bool Initialize(Vector2 position, Transform target)
        {
            try
            {
                _moveSpeed = prop.moveSpeed;
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
            prop.moveSpeed = 1f;
            prop.hpMax = 3;
        }

        protected override void Awake()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            prop.moveSpeed = adminDataSingleton.AdminBean.EnemyModel.prop.moveSpeed;
            prop.hpMax = adminDataSingleton.AdminBean.EnemyModel.prop.hpMax;
            State = new CharacterState(damageSufferedZoneModel.IsHit, prop.hpMax);
            base.Awake();
        }

        protected override void Start()
        {
            // プレイヤーから攻撃を受ける
            State.HP.Value = prop.hpMax;
            if (!_utility.UpdateStateHPAndIsDead(State.IsHit, State.HP, prop.hpMax, State.IsDead))
                Debug.LogError("UpdateStateHPAndIsDead");
            // 死亡判定
            State.IsDead.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                        gameObject.SetActive(false);
                });
        }

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
    /// キャラクターのプロパティ
    /// </summary>
    [System.Serializable]
    public struct CharacterProp
    {
        /// <summary>移動速度</summary>
        [Tooltip("移動速度")]
        public float moveSpeed;
        /// <summary>最大HP</summary>
        [Tooltip("最大HP")]
        public int hpMax;
    }

    /// <summary>
    /// キャラクターのステータス
    /// </summary>
    public class CharacterState
    {
        /// <summary>当たったか</summary>
        public IReactiveProperty<bool> IsHit { get; private set; }
        /// <summary>死亡フラグ</summary>
        public IReactiveProperty<bool> IsDead { get; private set; } = new BoolReactiveProperty();
        /// <summary>最大HP</summary>
        public int HPMax { get; private set; }
        /// <summary>HP</summary>
        public IReactiveProperty<int> HP { get; private set; } = new IntReactiveProperty();
        /// <summary>
        /// キャラクターのステータス
        /// コンストラクタ
        /// </summary>
        public CharacterState(IReactiveProperty<bool> isHit, int hpMax)
        {
            IsHit = isHit;
            HPMax = hpMax;
        }
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
