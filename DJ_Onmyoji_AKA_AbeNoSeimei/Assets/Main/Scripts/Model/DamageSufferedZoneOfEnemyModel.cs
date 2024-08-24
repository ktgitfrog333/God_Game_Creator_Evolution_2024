using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using Universal.Common;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// 敵が攻撃を受ける判定のトリガー
    /// </summary>
    public class DamageSufferedZoneOfEnemyModel : DamageSufferedZoneModel
    {
        /// <summary>即死タグ用</summary>
        [SerializeField] private string[] deadTags = { ConstTagNames.TAG_NAME_PLAYER };
        /// <summary>プレイヤーに触れた</summary>
        public IReactiveProperty<bool> IsHitPlayer { get; private set; } = new BoolReactiveProperty();

        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_BULLET;
            deadTags = new string[1];
            deadTags[0] = ConstTagNames.TAG_NAME_PLAYER;
        }

        protected override void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            invincibleTimeSec = adminDataSingleton.AdminBean.enemyModel.damageSufferedZoneOfEnemyModel.invincibleTimeSec;
            base.Start();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            if (_utility.IsCompareTagAndUpdateReactiveFlag(other, deadTags, IsHit))
            {
                IsHitPlayer.Value = true;
            }
        }

        public void OnTriggerEnter2DGraff(Collider2D other)
        {
            base.OnTriggerEnter2DPublic(other);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            IsHitPlayer.Value = false;
        }
    }
}
