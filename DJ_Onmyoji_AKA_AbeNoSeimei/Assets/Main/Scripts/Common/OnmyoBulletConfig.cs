using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
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
        public float? moveSpeed;
        /// <summary>行動間隔</summary>
        public float actionRate;
        /// <summary>停止するまでの時間</summary>
        public float bulletLifeTime;
        /// <summary>攻撃力</summary>
        public int attackPoint;
        /// <summary>攻撃範囲</summary>
        public float range;
        /// <summary>トラッキング対象</summary>
        public RectTransform trackingOfAny;
        /// <summary>デバフ効果時間</summary>
        public float debuffEffectLifeTime;
    }
}
