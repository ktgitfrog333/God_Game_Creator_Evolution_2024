using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// モブキャラクターをプール管理する
    /// モデル
    /// </summary>
    public class MobCharacter : MonoBehaviour
    {
        /// <summary>相手への攻撃ヒット判定のトリガー</summary>
        [Tooltip("相手への攻撃ヒット判定のトリガー")]
        [SerializeField] protected AttackColliderOfOnmyoBullet attackCollider;

        protected virtual void Reset()
        {
            attackCollider = GetComponentInChildren<AttackColliderOfOnmyoBullet>();
        }

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        protected virtual void Start()
        {
            attackCollider.IsHit.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                        gameObject.SetActive(false);
                });
        }
    }
}
