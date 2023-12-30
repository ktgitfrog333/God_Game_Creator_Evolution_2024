using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using Universal.Common;

namespace Main.Model
{
    /// <summary>
    /// プレイヤーが攻撃を受ける判定のトリガー
    /// </summary>
    public class DamageSufferedZoneOfPlayerModel : DamageSufferedZoneModel
    {
        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_ENEMY;
        }

        protected override void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            invincibleTimeSec = adminDataSingleton.AdminBean.PlayerModel.DamageSufferedZoneOfPlayerModel.invincibleTimeSec;
            base.Start();
        }

        protected override void OnTriggerEnter2D(Collider2D other) { }

        private void OnTriggerStay2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
    }
}
