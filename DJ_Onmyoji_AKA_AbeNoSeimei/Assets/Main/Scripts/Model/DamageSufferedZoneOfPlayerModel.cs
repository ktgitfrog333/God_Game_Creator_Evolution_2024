using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;

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
            base.Start();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
    }
}
