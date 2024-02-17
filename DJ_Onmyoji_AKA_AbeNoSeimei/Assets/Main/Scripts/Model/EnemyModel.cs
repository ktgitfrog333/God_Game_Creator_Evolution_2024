using System.Collections;
using System.Collections.Generic;
using Main.Utility;
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
        /// <summary>攻撃を与える判定のトリガー</summary>
        [SerializeField] private AttackColliderOfEnemy attackColliderOfEnemy;
        /// <summary>敵ID</summary>
        [SerializeField] private EnemiesID enemiesID;
        /// <summary>敵ID</summary>
        public EnemiesID EnemiesID => enemiesID;
        /// <summary>敵のプロパティ</summary>
        [SerializeField] private EnemiesProp enemiesProp;
        /// <summary>敵のプロパティ</summary>
        public EnemiesProp EnemiesProp => enemiesProp;
        /// <summary>共通のユーティリティ</summary>
        private MainCommonUtility _commonUtility = new MainCommonUtility();

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
            attackColliderOfEnemy = GetComponentInChildren<AttackColliderOfEnemy>();
            prop.moveSpeed = .25f;
            prop.hpMax = 2;
            enemiesProp.soulMoneyPoint = 1;
            enemiesProp.attackPoint = 1;
        }

        protected override void Awake()
        {
            // TODO:余力あれば管理画面でパラメータ管理
            // prop.moveSpeed = _commonUtility.AdminDataSingleton.AdminBean.enemyModel.prop.moveSpeed;
            // prop.hpMax = _commonUtility.AdminDataSingleton.AdminBean.enemyModel.prop.hpMax;
            // enemiesProp.soulMoneyPoint = _commonUtility.AdminDataSingleton.AdminBean.enemyModel.enemiesProp.soulMoneyPoint;
            // enemiesProp.attackPoint = _commonUtility.AdminDataSingleton.AdminBean.enemyModel.enemiesProp.attackPoint;
            if (!attackColliderOfEnemy.SetAttackPoint(enemiesProp.attackPoint))
                Debug.LogError("SetAttackPoint");
            State = new CharacterState(damageSufferedZoneModel.IsHit, prop.hpMax, damageSufferedZoneModel.Damage);
            base.Awake();
        }

        private void OnEnable()
        {
            // プレイヤーから攻撃を受ける
            State.HP.Value = prop.hpMax;
            State.IsDead.Value = false;
        }

        protected override void Start()
        {
            // プレイヤーから攻撃を受ける
            if (!_utility.UpdateStateHPAndIsDead(State))
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
    /// 敵のプロパティ
    /// </summary>
    [System.Serializable]
    public struct EnemiesProp
    {
        /// <summary>魂の経験値ポイント</summary>
        public int soulMoneyPoint;
        /// <summary>攻撃力</summary>
        public int attackPoint;
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
        /// <summary>ダメージ値</summary>
        public IReactiveProperty<int> Damage { get; private set; } = new IntReactiveProperty();
        /// <summary>
        /// キャラクターのステータス
        /// コンストラクタ
        /// </summary>
        public CharacterState(IReactiveProperty<bool> isHit, int hpMax, IReactiveProperty<int> damage)
        {
            IsHit = isHit;
            HPMax = hpMax;
            Damage = damage;
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
