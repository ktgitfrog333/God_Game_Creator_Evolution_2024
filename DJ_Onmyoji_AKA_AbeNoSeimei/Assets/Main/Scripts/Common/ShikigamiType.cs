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
    /// レア度
    /// </summary>
    public enum RareType
    {
        /// <summary>ノーマル</summary>
        Normal,
        /// <summary>レア</summary>
        Rare,
        /// <summary>Sレア</summary>
        SRare,
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
        /// <summary>ホーミング性能</summary>
        Homing = 8,
    }

    /// <summary>
    /// サブスキルタイプ
    /// </summary>
    public enum SubSkillType
    {
        /// <summary>サブスキルなし</summary>
        None = 0,
        /// <summary>爆発（ラップ）</summary>
        Explosion = 1,
        /// <summary>麻痺（ラップ）</summary>
        Paralysis = 2,
        /// <summary>貫通（ラップ）</summary>
        Penetrating = 3,
        /// <summary>拡散（ラップ）</summary>
        Spreading = 4,
        /// <summary>活力（ダンス）</summary>
        Heal = 5,
        /// <summary>炎上（ダンス）</summary>
        Fire = 6,
        /// <summary>突風（ダンス）</summary>
        Knockback = 7,
        /// <summary>急所（ダンス）</summary>
        Critical = 8,
        /// <summary>毒（グラフ）</summary>
        Poison = 9,
        /// <summary>暗闇（グラフ）</summary>
        Darkness = 10,
        /// <summary>雷撃（グラフ）</summary>
        Thunder = 11,
        /// <summary>呪詛（グラフ）</summary>
        Curse = 12,
    }
}
