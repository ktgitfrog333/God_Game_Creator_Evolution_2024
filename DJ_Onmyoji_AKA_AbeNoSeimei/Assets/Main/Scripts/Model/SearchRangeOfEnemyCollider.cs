using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Utility;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 敵が攻撃範囲へ侵入した判定のトリガー
    /// </summary>
    public class SearchRangeOfEnemyCollider : DamageSufferedZoneModel
    {
        /// <summary>ユーティリティ</summary>
        private EnemyPlayerModelUtility _utility = new EnemyPlayerModelUtility();
        public Transform Target { get; private set; }
        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_ENEMY;
        }

        protected override void Start() { }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_utility.IsCompareTagAndUpdateReactiveFlag(other, tags, IsHit))
            {
                Target = other.transform;
                IsHit.Value = true;
            }
        }

        private void OnDisable()
        {
            Target = null;
        }
    }
}
