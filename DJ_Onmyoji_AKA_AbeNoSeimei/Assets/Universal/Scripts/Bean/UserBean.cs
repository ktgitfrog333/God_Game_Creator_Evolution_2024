using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Universal.Common;

namespace Universal.Bean
{
    [System.Serializable]
    /// <summary>
    /// ユーザー情報を保持するクラス
    /// </summary>
    public class UserBean
    {
        /// <summary>
        /// デフォルトのシーンID
        /// </summary>
        private static readonly int SCENEID_DEFAULT = 1;
        
        /// <summary>
        /// 全てのシーンID
        /// </summary>
        private readonly int SCENEID_ALL = 1;
        
        
        /// <summary>
        /// デフォルトのクリアステータス
        /// </summary>
        private readonly int[] STATE_DEFAULT = {1,0,0,0,0,0,0};
        
        /// <summary>
        /// 全解放時のクリアステータス
        /// </summary>
        private readonly int[] STATE_ALL = {2,2,2,2,2,2,2};

        /// <summary>
        /// シーンID
        /// </summary>
        public int sceneId = SCENEID_DEFAULT;
        
        /// <summary>
        /// クリアステータス
        /// </summary>
        public int[] state = new int[ConstBeanRules.STATELENGTH];
        
        /// <summary>
        /// オーディオボリュームインデックス
        /// </summary>
        public int audioVolumeIndex = 5;
        
        /// <summary>
        /// BGMボリュームインデックス
        /// </summary>
        public int bgmVolumeIndex = 5;
        
        /// <summary>
        /// SEボリュームインデックス
        /// </summary>
        public int seVolumeIndex = 5;
        
        /// <summary>
        /// 振動有効インデックス
        /// </summary>
        public int vibrationEnableIndex = 0;

        private readonly PentagramTurnTableInfo PENTAGRAM_TURN_TABLE_INFO_DEFAULT = new PentagramTurnTableInfo()
        {
            slots = new PentagramTurnTableInfo.Slot[]
            {
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 0,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        characterID = 3,
                        genomeType = 0,
                        name = "朱雀",
                        type = 1,
                        slotId = 0,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 6,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                        subSkills = new ShikigamiInfo.SubSkill[0],
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 1,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        characterID = 12,
                        genomeType = 0,
                        name = "晴明",
                        type = 3,
                        slotId = 1,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                        subSkills = new ShikigamiInfo.SubSkill[0],
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 2,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        characterID = 0,
                        genomeType = 0,
                        name = "勾陳",
                        type = 2,
                        slotId = 2,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 6,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 7,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                        subSkills = new ShikigamiInfo.SubSkill[0],
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 3,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        characterID = 1,
                        genomeType = 0,
                        name = "六合",
                        type = 0,
                        slotId = 3,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                        subSkills = new ShikigamiInfo.SubSkill[0],
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 4,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        characterID = 12,
                        genomeType = 0,
                        name = "晴明",
                        type = 3,
                        slotId = 4,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                        subSkills = new ShikigamiInfo.SubSkill[0],
                    }
                },
            }
        };

        private readonly PentagramTurnTableInfo PENTAGRAM_TURN_TABLE_INFO_DEFAULT_2 = new PentagramTurnTableInfo()
        {
            slots = new PentagramTurnTableInfo.Slot[]
            {
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 1,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        characterID = 12,
                        genomeType = 0,
                        name = "晴明",
                        type = 3,
                        slotId = 1,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                        subSkills = new ShikigamiInfo.SubSkill[0],
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 4,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        characterID = 12,
                        genomeType = 0,
                        name = "晴明",
                        type = 3,
                        slotId = 4,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill()
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                        subSkills = new ShikigamiInfo.SubSkill[0],
                    }
                },
            }
        };

        public PentagramTurnTableInfo pentagramTurnTableInfo = new PentagramTurnTableInfo()
        {
            slots = new PentagramTurnTableInfo.Slot[]
            {
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 0,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        type = 3,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                    },
                },
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 1,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        type = 3,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                    },
                },
            }
        };

        /// <summary>魂のお金</summary>
        public int soulMoney = 0;
        private readonly int SOUL_MONEY = 0;
        /// <summary>入力モード</summary>
        public int inputMode = 0;

        /// <summary>
        /// ユーザー情報を保持するクラス
        /// </summary>
        public UserBean(EnumLoadMode enumLoadMode=EnumLoadMode.Continue)
        {
            switch (enumLoadMode)
            {
                case EnumLoadMode.Continue:
                    break;
                case EnumLoadMode.Default:
                    sceneId = SCENEID_DEFAULT;
                    state = STATE_DEFAULT;
                    // TODO: デフォルトは結局どちらなのか
                    // PENTAGRAM_TURN_TABLE_INFO_DEFAULT / PENTAGRAM_TURN_TABLE_INFO_DEFAULT_2
                    pentagramTurnTableInfo = PENTAGRAM_TURN_TABLE_INFO_DEFAULT;
                    soulMoney = SOUL_MONEY;

                    break;
                case EnumLoadMode.All:
                    sceneId = SCENEID_ALL;
                    state = STATE_ALL;

                    break;
            }
        }

        /// <summary>
        /// 式神の情報
        /// </summary>
        /// <see cref="Main.Common.ShikigamiInfo"/>
        [System.Serializable]
        public class ShikigamiInfo
        {
            /// <summary>式神キャラクターID</summary>
            public int characterID;
            /// <summary>
            /// 遺伝子タイプ
            /// タイプA、タイプB……の様に一つの式神を複数タイプ生成させたい場合に使用する
            /// </summary>
            public int genomeType;
            /// <summary>名称</summary>
            public string name;
            /// <summary>式神タイプ</summary>
            public int type;
            /// <summary>スロット番号</summary>
            public int slotId;
            /// <summary>レベル</summary>
            public int level;
            /// <summary>メインスキル</summary>
            public MainSkill[] mainSkills;
            /// <summary>サブスキル</summary>
            public SubSkill[] subSkills;

            /// <summary>
            /// メインスキル
            /// </summary>
            [System.Serializable]
            public class MainSkill
            {
                /// <summary>メインスキルタイプ</summary>
                public int type;
                /// <summary>スキルランク</summary>
                public int rank;

                public MainSkill() { }
                public MainSkill(MainSkill source)
                {
                    type = source.type;
                    rank = source.rank;
                }
            }

            /// <summary>
            /// サブスキル
            /// </summary>
            [System.Serializable]
            public class SubSkill
            {
                /// <summary>サブスキルタイプ</summary>
                public int type;
                /// <summary>スキルランク</summary>
                public int rank;

                public SubSkill() { }
                public SubSkill(SubSkill source)
                {
                    type = source.type;
                    rank = source.rank;
                }

                // 参考：ChatGpt-4o
                public override bool Equals(object obj)
                {
                    if (obj is SubSkill other)
                    {
                        return type == other.type && rank == other.rank;
                    }
                    else
                    {
                        return false;
                    }
                }

                public override int GetHashCode()
                {
                    return HashCode.Combine(type, rank);
                }
            }

            public ShikigamiInfo() { }
            public ShikigamiInfo(ShikigamiInfo source)
            {
                characterID = source.characterID;
                genomeType = source.genomeType;
                name = source.name;
                type = source.type;
                slotId = source.slotId;
                level = source.level;
                mainSkills = source.mainSkills.Select(ms => new MainSkill(ms)).ToArray();
                subSkills = source.subSkills.Select(ss => new SubSkill(ss)).ToArray();
            }
        }

        /// <summary>
        /// ペンダグラムターンテーブル情報
        /// </summary>
        /// <see cref="Main.Common.PentagramTurnTableInfo"/>
        [System.Serializable]
        public class PentagramTurnTableInfo
        {
            /// <summary>スロット</summary>
            public Slot[] slots;

            /// <summary>
            /// スロット
            /// </summary>
            [System.Serializable]
            public class Slot
            {
                /// <summary>スロット番号</summary>
                public int slotId;
                /// <summary>式神の情報</summary>
                public ShikigamiInfo shikigamiInfo;
            }
        }

        /// <summary>
        /// ユーザー情報を保持するクラス
        /// </summary>
        public UserBean(UserBean userBean)
        {
            sceneId = userBean.sceneId;
            state = userBean.state;
            audioVolumeIndex = userBean.audioVolumeIndex;
            bgmVolumeIndex = userBean.bgmVolumeIndex;
            seVolumeIndex = userBean.seVolumeIndex;
            vibrationEnableIndex = userBean.vibrationEnableIndex;
            pentagramTurnTableInfo = userBean.pentagramTurnTableInfo;
            soulMoney = userBean.soulMoney;
            inputMode = userBean.inputMode;
        }
    }
}
