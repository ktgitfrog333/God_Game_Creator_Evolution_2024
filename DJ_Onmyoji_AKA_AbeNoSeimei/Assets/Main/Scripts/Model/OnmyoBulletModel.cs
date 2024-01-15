using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Universal.Common;
using Universal.Utility;

namespace Main.Model
{
    /// <summary>
    /// 魔力弾
    /// モデル
    /// </summary>
    public class OnmyoBulletModel : MobCharacter, IOnmyoBulletModel
    {
        /// <summary>移動方向</summary>
        private Vector2 _moveDirection;
        /// <summary>移動速度</summary>
        private float _moveSpeed;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>停止するまでの時間</summary>
        private float _disableTimeSec;
        /// <summary>設定</summary>
        [SerializeField] private OnmyoBulletConfig onmyoBulletConfig;
        /// <summary>敵が攻撃範囲へ侵入した判定のトリガー</summary>
        [Tooltip("敵が攻撃範囲へ侵入した判定のトリガー")]
        [SerializeField] private SearchRangeOfEnemyCollider searchRangeOfEnemyCollider;
        /// <summary>攻撃を与える判定のトリガー</summary>
        [SerializeField] private AttackColliderOfOnmyoBullet attackColliderOfOnmyoBullet;

        public bool Initialize(Vector2 position, Vector3 eulerAngles, float disableTimeSec, int attackPoint, Vector2 moveDirection=new Vector2())
        {
            try
            {
                _moveDirection = Quaternion.Euler(eulerAngles) * (!moveDirection.Equals(Vector2.zero) ?
                    moveDirection : onmyoBulletConfig.moveDirection);
                _moveSpeed = onmyoBulletConfig.moveSpeed;
                _disableTimeSec = disableTimeSec;
                Transform.position = position;
                if (!attackColliderOfOnmyoBullet.SetAttackPoint(attackPoint))
                    Debug.LogError("SetAttackPoint");

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
            searchRangeOfEnemyCollider = GetComponentInChildren<SearchRangeOfEnemyCollider>();
            attackColliderOfOnmyoBullet = GetComponentInChildren<AttackColliderOfOnmyoBullet>();
            base.Reset();
        }

        protected override void Awake()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            onmyoBulletConfig.moveDirection = adminDataSingleton.AdminBean.OnmyoBulletModel.moveDirection;
            onmyoBulletConfig.moveSpeed = adminDataSingleton.AdminBean.OnmyoBulletModel.moveSpeed;
            base.Awake();
        }

        private void OnEnable()
        {
            StartCoroutine(GeneralUtility.ActionsAfterDelay(_disableTimeSec, () => gameObject.SetActive(false)));
        }

        private void FixedUpdate()
        {
            if (searchRangeOfEnemyCollider.Target != null)
            {
                var targetDirection = searchRangeOfEnemyCollider.Target.position - Transform.position;
                _moveDirection = targetDirection.normalized;
            }
            // 指定された方向と速度に弾を移動させる
            Transform.position += (Vector3)_moveDirection * _moveSpeed * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// 魔力弾の設定
    /// </summary>
    [System.Serializable]
    public struct OnmyoBulletConfig
    {
        /// <summary>移動方向</summary>
        [Tooltip("移動方向")]
        public Vector2 moveDirection;
        /// <summary>移動速度</summary>
        [Tooltip("移動速度")]
        public float moveSpeed;
    }

    public interface IOnmyoBulletModel
    {
        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="position">生成位置</param>
        /// <param name="eulerAngles">初期角度</param>
        /// <param name="disableTimeSec">停止するまでの時間</param>
        /// <param name="attackPoint">攻撃力</param>
        /// <param name="moveDirection">移動方向</param>
        /// <returns>成功／失敗</returns>
        public bool Initialize(Vector2 position, Vector3 eulerAngles, float disableTimeSec, int attackPoint, Vector2 moveDirection=new Vector2());
    }
}
