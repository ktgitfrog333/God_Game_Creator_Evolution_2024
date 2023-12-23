using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 魔力弾
    /// モデル
    /// </summary>
    public class OnmyoBulletModel : MonoBehaviour, IOnmyoBulletModel
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

        public bool Initialize(Vector2 position)
        {
            try
            {
                _moveDirection = onmyoBulletConfig.moveDirection;
                _moveSpeed = onmyoBulletConfig.moveSpeed;
                _disableTimeSec = onmyoBulletConfig.disableTimeSec;
                Transform.position = position;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            onmyoBulletConfig.moveDirection = Vector2.down;
            onmyoBulletConfig.moveSpeed = 8f;
            onmyoBulletConfig.disableTimeSec = 10f;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            DOVirtual.DelayedCall(_disableTimeSec, () => gameObject.SetActive(false));
        }

        private void FixedUpdate()
        {
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
        /// <summary>停止するまでの時間</summary>
        [Tooltip("停止するまでの時間")]
        public float disableTimeSec;
    }

    public interface IOnmyoBulletModel
    {
        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="position">生成位置</param>
        /// <returns>成功／失敗</returns>
        public bool Initialize(Vector2 position);
    }
}
