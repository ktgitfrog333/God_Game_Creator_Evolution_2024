using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// スロット番号
    /// </summary>
    public enum SlotId
    {
        SL00,
        SL01,
        SL02,
        SL03,
        SL04,
    }

    /// <summary>
    /// 式神キャラクターID
    /// </summary>
    public enum ShikigamiCharacterID
    {
        SH0000,
        SH0001,
        SH0002,
        SH0003,
        SH0004,
        SH0005,
        SH0006,
        SH0007,
        SH0008,
        SH0009,
        SH0010,
        SH0011,
        SH0012,
    }

    /// <summary>
    /// 遺伝子タイプ
    /// タイプA、タイプB……の様に一つの式神を複数タイプ生成させたい場合に使用する
    /// </summary>
    public enum GenomeType
    {
        GE0000,
        GE0001,
        GE0002,
    }

    /// <summary>
    /// 式神タイプ
    /// </summary>
    public enum ShikigamiType
    {
        /// <summary>ラップ</summary>
        Wrap,
        /// <summary>ダンス</summary>
        Dance,
        /// <summary>グラフィティ</summary>
        Graffiti,
        /// <summary>陰陽玉</summary>
        OnmyoTurret,
    }

    /// <summary>
    /// スキルランク
    /// D < C < B < A < S
    /// </summary>
    public enum SkillRank
    {
        D,
        C,
        B,
        A,
        S,
    }

    /// <summary>
    /// 強調（強化遷移など）
    /// </summary>
    public enum EmphasisType
    {
        /// <summary>変化なし</summary>
        Neutral,
        /// <summary>増加</summary>
        Positive,
        /// <summary>減少</summary>
        Negative,
    }

    /// <summary>
    /// メインスキルタイプ
    /// </summary>
    public enum MainSkillType
    {
        /// <summary>攻撃間隔</summary>
        ActionRate = 1,
        /// <summary>攻撃力</summary>
        AttackPoint = 3,
        /// <summary>飛距離</summary>
        BulletLifeTime = 2,
        /// <summary>範囲</summary>
        Range = 6,
        /// <summary>デバフ効果時間</summary>
        DebuffEffectLifeTime = 7,
    }

    /// <summary>
    /// サブスキルタイプ
    /// </summary>
    public enum SubSkillType
    {
        /// <summary>追尾性能</summary>
        Homing = 2,
        /// <summary>爆発</summary>
        Explosion = 1,
        /// <summary>貫通</summary>
        Penetrating = 3,
        /// <summary>拡散</summary>
        Spreading = 4,
        /// <summary>連射</summary>
        ContinuousFire = 5,
    }
}
