using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 攻撃を与える判定のトリガー
    /// </summary>
    public class AttackColliderOfOnmyoBullet : DamageSufferedZoneModel
    {
        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_ENEMY;
        }

        protected override void Start() { }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }

        protected override void OnDisable()
        {
            IsHit.Value = false;
        }
    }
}
