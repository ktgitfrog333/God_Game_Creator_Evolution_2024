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
            /// <summary>式神タイプ</summary>
            public ShikigamiType type;
            /// <summary>レベル</summary>
            public int level;
            /// <summary>メインスキル</summary>
            public MainSkill[] mainSkills;
            /// <summary>サブスキル</summary>
            public SubSkill[] subSkills;

            [System.Serializable]
            public struct MainSkill
            {
                public MainSkillType type;
                public SkillRank rank;
            }

            [System.Serializable]
            public struct SubSkill
            {
                public SubSkillType type;
                public SkillRank rank;
            }
        }

        /// <summary>
        /// ステート
        /// </summary>
        public struct State
        {
            /// <summary>テンポレベル</summary>
            public IReactiveProperty<float> tempoLevel;
        }
    }
}
