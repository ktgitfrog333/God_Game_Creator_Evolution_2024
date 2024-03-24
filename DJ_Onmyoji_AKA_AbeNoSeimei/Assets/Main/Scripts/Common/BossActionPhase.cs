using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// ボス演出フェーズ
    /// </summary>
    public enum BossDirectionPhase
    {
        /// <summary>待機</summary>
        Wait,
        /// <summary>登場</summary>
        Entrance,
        /// <summary>退場</summary>
        Exit,
    }

    /// <summary>
    /// ボス行動フェーズ
    /// </summary>
    public enum BossActionPhase
    {
        /// <summary>アイドル</summary>
        Idle,
        /// <summary>攻撃</summary>
        Attack,
        /// <summary>右手やられ</summary>
        DamageRight,
        /// <summary>左手やられ</summary>
        DamageLeft,
        /// <summary>両手やられ（ダウン状態）</summary>
        DamageAll,
    }
}
