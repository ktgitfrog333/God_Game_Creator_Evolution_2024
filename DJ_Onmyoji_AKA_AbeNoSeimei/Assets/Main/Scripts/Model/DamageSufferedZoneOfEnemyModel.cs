using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using Universal.Common;

namespace Main.Model
{
    /// <summary>
    /// 敵が攻撃を受ける判定のトリガー
    /// </summary>
    public class DamageSufferedZoneOfEnemyModel : DamageSufferedZoneModel
    {
        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_BULLET;
        }

        protected override void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            invincibleTimeSec = adminDataSingleton.AdminBean.EnemyModel.DamageSufferedZoneOfEnemyModel.invincibleTimeSec;
            base.Start();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
    }
}
