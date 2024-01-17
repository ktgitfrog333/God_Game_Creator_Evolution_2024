using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Main.Common;
using Main.Audio;
using Main.Utility;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// プレイヤー
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerModel : LevelPhysicsSerializerCapsule, IPlayerModel
    {
        /// <summary>ジャンプ速度</summary>
        [SerializeField] private float jumpSpeed = 6f;
        /// <summary>操作禁止フラグ</summary>
        private bool _inputBan;
        /// <summary>操作禁止フラグ</summary>
        public bool InputBan => _inputBan;
        /// <summary>ダメージ判定</summary>
        [SerializeField] private DamageSufferedZoneOfPlayerModel damageSufferedZoneModel;
        /// <summary>生成されたか</summary>
        public IReactiveProperty<bool> IsInstanced { get; private set; } = new BoolReactiveProperty();
        /// <summary>ユーティリティ</summary>
        private EnemyPlayerModelUtility _utility = new EnemyPlayerModelUtility();
        /// <summary>プロパティ</summary>
        [SerializeField] private CharacterProp prop;
        /// <summary>ステータス</summary>
        public CharacterState State { get; private set; }

        public bool SetInputBan(bool unactive)
        {
            try
            {
                _inputBan = unactive;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetIsDead(bool enabled)
        {
            try
            {
                State.IsDead.Value = enabled;
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
            base.Reset();
            distance = 0f;
            damageSufferedZoneModel = GetComponentInChildren<DamageSufferedZoneOfPlayerModel>();
        }

        private void Awake()
        {
            var utility = new MainCommonUtility();
            var adminDataSingleton = utility.AdminDataSingleton;
            prop.moveSpeed = adminDataSingleton.AdminBean.PlayerModel.prop.moveSpeed;
            prop.hpMax = adminDataSingleton.AdminBean.PlayerModel.prop.hpMax;
            State = new CharacterState(damageSufferedZoneModel.IsHit, prop.hpMax, damageSufferedZoneModel.Damage);
        }

        private void Start()
        {
            // Rigidbody
            var rigidbody = GetComponent<Rigidbody2D>();
            var rigidbodyGravityScale = rigidbody.gravityScale;
            // 移動制御のベロシティ
            var moveVelocity = new Vector3();
            // 位置・スケールのキャッシュ
            var tForm = transform;
            // ジャンプフラグ
            var isJumped = false;
            // 移動入力に応じて移動座標をセット
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    origin = gameObject.transform.position;
                    if (!_inputBan)
                    {
                        moveVelocity = new Vector3(MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.x, moveVelocity.y, moveVelocity.z) * prop.moveSpeed * (1f + Time.deltaTime);
                        var rayCastHit = Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, LayerMask.GetMask(ConstLayerNames.LAYER_NAME_FLOOR));
                        if (!isJumped &&
                            MainGameManager.Instance.InputSystemsOwner.InputPlayer.Jumped &&
                            rayCastHit.transform != null)
                        {
                            isJumped = true;
                        }
                        // 空中のみ重力が有効（Y軸引力にAddForceのX軸制御が負けるため）
                        rigidbody.gravityScale = rayCastHit.transform == null ? rigidbodyGravityScale : 0f;
                    }
                });
            // 移動制御
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (isJumped)
                    {
                        moveVelocity = new Vector3(moveVelocity.x, jumpSpeed, moveVelocity.z);
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_player_jump);
                        isJumped = false;
                        // ジャンプ挙動
                        rigidbody.AddForce(moveVelocity, ForceMode2D.Impulse);
                    }
                    else
                    {
                        moveVelocity = new Vector3(moveVelocity.x, 0f, moveVelocity.z);
                    }
                    // 歩く走る挙動
                    rigidbody.AddForce(moveVelocity);
                });
            // 敵から攻撃を受ける
            State.HP.Value = prop.hpMax;
            if (!_utility.UpdateStateHPAndIsDead(State))
                Debug.LogError("UpdateStateHPAndIsDead");
            // 死亡判定
            State.IsDead.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        _inputBan = true;
                        moveVelocity = Vector3.zero;
                    }
                });
            if (!IsInstanced.Value)
                IsInstanced.Value = true;
        }
    }

    /// <summary>
    /// モデル
    /// プレイヤー
    /// インターフェース
    /// </summary>
    public interface IPlayerModel
    {
        /// <summary>
        /// 操作禁止フラグをセット
        /// </summary>
        /// <param name="unactive">許可／禁止</param>
        /// <returns>成功／失敗</returns>
        public bool SetInputBan(bool unactive);
        /// <summary>
        /// 死亡フラグをセット
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsDead(bool enabled);
    }
}
