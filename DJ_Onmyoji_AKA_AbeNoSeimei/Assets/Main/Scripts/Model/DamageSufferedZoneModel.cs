using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Main.Common;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// プレイヤーが攻撃を受ける判定のトリガー
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class DamageSufferedZoneModel : MonoBehaviour
    {
        /// <summary>当たったか</summary>
        public IReactiveProperty<bool> IsHit { get; private set; } = new BoolReactiveProperty();
        /// <summary>接触対象のオブジェクトタグ</summary>
        [SerializeField] private string[] tags = { ConstTagNames.TAG_NAME_ENEMY };
        /// <summary>無敵時間（秒）</summary>
        [SerializeField] private float invincibleTimeSec = 1f;

        private void Start()
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (0 < tags.Where(q => other.CompareTag(q))
            .Select(q => q)
            .ToArray()
            .Length &&
            !IsHit.Value)
            {
                IsHit.Value = true;
            }
        }
    }
}
