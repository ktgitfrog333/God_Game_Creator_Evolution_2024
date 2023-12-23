using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Main.Common;
using Main.Utility;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 攻撃を受ける判定のトリガー
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class DamageSufferedZoneModel : MonoBehaviour
    {
        /// <summary>当たったか</summary>
        public IReactiveProperty<bool> IsHit { get; private set; } = new BoolReactiveProperty();
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] protected string[] tags = { ConstTagNames.TAG_NAME_ENEMY };
        /// <summary>無敵時間（秒）</summary>
        [SerializeField] protected float invincibleTimeSec = 1f;
        /// <summary>ユーティリティ</summary>
        private EnemyPlayerModelUtility _utility = new EnemyPlayerModelUtility();

        protected virtual void Start()
        {
            var collider2D = GetComponent<CircleCollider2D>();
            IsHit.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        collider2D.enabled = false;
                        DOVirtual.DelayedCall(invincibleTimeSec, () =>
                        {
                            IsHit.Value = false;
                            collider2D.enabled = true;
                        });
                    }
                });
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (_utility.IsCompareTagAndUpdateReactiveFlag(other, tags, IsHit))
            {
                IsHit.Value = true;
            }
        }
    }
}
