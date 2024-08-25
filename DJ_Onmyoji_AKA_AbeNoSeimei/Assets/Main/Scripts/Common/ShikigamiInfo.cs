using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// 式神の情報
    /// </summary>
    [System.Serializable]
    public struct ShikigamiInfo
    {
        /// <summary>プロパティ</summary>
        public Prop prop;
        /// <summary>ステート</summary>
        public State state;

        /// <summary>
        /// プロパティ
        /// </summary>
        [System.Serializable]
        public struct Prop
        {
            /// <summary>式神キャラクターID</summary>
            public ShikigamiCharacterID characterID;
            /// <summary>
            /// 遺伝子タイプ
            /// タイプA、タイプB……の様に一つの式神を複数タイプ生成させたい場合に使用する
            /// </summary>
            public GenomeType genomeType;
            /// <summary>式神タイプ</summary>
            public ShikigamiType type;
            /// <summary>スロット番号</summary>
            public int slotId;
            /// <summary>レベル</summary>
            public int level;
            /// <summary>メインスキル</summary>
            public MainSkill[] mainSkills;
            /// <summary>サブスキル</summary>
            public SubSkill[] subSkills;
            /// <summary>テンポレベルを元へ戻すまでの時間（秒）</summary>
            public float tempoLevelRevertTimeSec;

            /// <summary>
            /// メインスキル
            /// </summary>
            [System.Serializable]
            public struct MainSkill
            {
                /// <summary>メインスキルタイプ</summary>
                public MainSkillType type;
                /// <summary>スキルランク</summary>
                public SkillRank rank;
                /// <summary>強調</summary>
                public EmphasisType emphasisType;
            }

            /// <summary>
            /// サブスキル
            /// </summary>
            [System.Serializable]
            public struct SubSkill
            {
                /// <summary>サブスキルタイプ</summary>
                public SubSkillType type;
                /// <summary>スキルランク</summary>
                public SkillRank rank;
                /// <summary>強調</summary>
                public EmphasisType emphasisType;
            }
        }

        /// <summary>
        /// ステート
        /// </summary>
        public struct State
        {
            /// <summary>テンポレベル</summary>
            public IReactiveProperty<float> tempoLevel;
            /// <summary>テンポレベルを元へ戻す状態</summary>
            public IReactiveProperty<int> tempoLevelRevertState;
            /// <summary>SPゲージ回復中</summary>
            public IReactiveProperty<bool> isRest;
        }
    }

    /// <summary>
    /// ステータス表示で使用
    /// </summary>
    [System.Serializable]
    public struct ShikigamiInfoVisualMaps
    {
        /// <summary>式神タイプ</summary>
        public ShikigamiInfoVisualMapOfShikigamiType[] shikigamiTypes;
        /// <summary>メインスキルタイプ</summary>
        public ShikigamiInfoVisualMapOfMainSkillType[] mainSkilltypes;
        /// <summary>サブスキルタイプ</summary>
        public ShikigamiInfoVisualMapOfSubSkillType[] subSkillTypes;
    }

    /// <summary>
    /// ステータス表示で使用
    /// 式神タイプ
    /// </summary>
    [System.Serializable]
    public struct ShikigamiInfoVisualMapOfShikigamiType
    {
        /// <summary>式神タイプ</summary>
        public ShikigamiType type;
        /// <summary>表記</summary>
        public string value;
    }

    /// <summary>
    /// ステータス表示で使用
    /// メインスキルタイプ
    /// </summary>
    [System.Serializable]
    public struct ShikigamiInfoVisualMapOfMainSkillType
    {
        /// <summary>メインスキルタイプ</summary>
        public MainSkillType type;
        /// <summary>表記</summary>
        public string value;
    }

    /// <summary>
    /// ステータス表示で使用
    /// サブスキルタイプ
    /// </summary>
    [System.Serializable]
    public struct ShikigamiInfoVisualMapOfSubSkillType
    {
        /// <summary>サブスキルタイプ</summary>
        public SubSkillType type;
        /// <summary>表記</summary>
        public string value;
    }
}
