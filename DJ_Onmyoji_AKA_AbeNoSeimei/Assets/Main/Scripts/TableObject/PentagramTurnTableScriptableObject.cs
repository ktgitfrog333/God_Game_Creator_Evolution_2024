using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.TableObject
{
    /// <summary>
    /// ペンダグラムターンテーブル
    /// スクリプテーブル
    /// </summary>
    [CreateAssetMenu(fileName = "PentagramTurnTableScriptableObject", menuName = "LevelDesign/ScriptableObject")]
    public class PentagramTurnTableScriptableObject : ScriptableObject
    {
        /// <summary>チュートリアル用のペンダグラムターンテーブル情報</summary>
        [SerializeField] private PentagramTurnTableInfo pentagramTurnTableInfoTutorial = new PentagramTurnTableInfo()
        {
            slots = new PentagramTurnTableInfo.Slot[]
            {
                new PentagramTurnTableInfo.Slot()
                {
                    prop = new PentagramTurnTableInfo.Slot.Prop()
                    {
                        slotId = SlotId.SL00,
                        shikigamiInfo = new ShikigamiInfo()
                        {
                            prop = new ShikigamiInfo.Prop()
                            {
                                characterID = ShikigamiCharacterID.SH0003,
                                genomeType = GenomeType.GE0000,
                                type = ShikigamiType.Dance,
                                slotId = 0,
                                level = 1,
                                mainSkills = new ShikigamiInfo.Prop.MainSkill[]
                                {
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.ActionRate,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.AttackPoint,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.Range,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.BulletLifeTime,
                                        rank = SkillRank.D,
                                    },
                                },
                                subSkills = new ShikigamiInfo.Prop.SubSkill[0]
                            }
                        },
                        instanceId = ConstShikigamiParameters.UNSET_SLOT_NUMBER,
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    prop = new PentagramTurnTableInfo.Slot.Prop()
                    {
                        slotId = SlotId.SL01,
                        shikigamiInfo = new ShikigamiInfo()
                        {
                            prop = new ShikigamiInfo.Prop()
                            {
                                characterID = ShikigamiCharacterID.SH0012,
                                genomeType = GenomeType.GE0000,
                                type = ShikigamiType.OnmyoTurret,
                                slotId = 1,
                                level = 1,
                                mainSkills = new ShikigamiInfo.Prop.MainSkill[]
                                {
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.ActionRate,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.AttackPoint,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.BulletLifeTime,
                                        rank = SkillRank.D,
                                    },
                                },
                                subSkills = new ShikigamiInfo.Prop.SubSkill[0]
                            }
                        },
                        instanceId = ConstShikigamiParameters.UNSET_SLOT_NUMBER,
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    prop = new PentagramTurnTableInfo.Slot.Prop()
                    {
                        slotId = SlotId.SL02,
                        shikigamiInfo = new ShikigamiInfo()
                        {
                            prop = new ShikigamiInfo.Prop()
                            {
                                characterID = ShikigamiCharacterID.SH0000,
                                genomeType = GenomeType.GE0000,
                                type = ShikigamiType.Graffiti,
                                slotId = 2,
                                level = 1,
                                mainSkills = new ShikigamiInfo.Prop.MainSkill[]
                                {
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.ActionRate,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.Range,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.DebuffEffectLifeTime,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.BulletLifeTime,
                                        rank = SkillRank.D,
                                    },
                                },
                                subSkills = new ShikigamiInfo.Prop.SubSkill[0]
                            }
                        },
                        instanceId = ConstShikigamiParameters.UNSET_SLOT_NUMBER,
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    prop = new PentagramTurnTableInfo.Slot.Prop()
                    {
                        slotId = SlotId.SL03,
                        shikigamiInfo = new ShikigamiInfo()
                        {
                            prop = new ShikigamiInfo.Prop()
                            {
                                characterID = ShikigamiCharacterID.SH0001,
                                genomeType = GenomeType.GE0000,
                                type = ShikigamiType.Wrap,
                                slotId = 3,
                                level = 1,
                                mainSkills = new ShikigamiInfo.Prop.MainSkill[]
                                {
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.ActionRate,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.AttackPoint,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.Homing,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.BulletLifeTime,
                                        rank = SkillRank.D,
                                    },
                                },
                                subSkills = new ShikigamiInfo.Prop.SubSkill[0]
                            }
                        },
                        instanceId = ConstShikigamiParameters.UNSET_SLOT_NUMBER,
                    }
                },
                new PentagramTurnTableInfo.Slot()
                {
                    prop = new PentagramTurnTableInfo.Slot.Prop()
                    {
                        slotId = SlotId.SL04,
                        shikigamiInfo = new ShikigamiInfo()
                        {
                            prop = new ShikigamiInfo.Prop()
                            {
                                characterID = ShikigamiCharacterID.SH0012,
                                genomeType = GenomeType.GE0000,
                                type = ShikigamiType.OnmyoTurret,
                                slotId = 4,
                                level = 1,
                                mainSkills = new ShikigamiInfo.Prop.MainSkill[]
                                {
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.ActionRate,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.AttackPoint,
                                        rank = SkillRank.D,
                                    },
                                    new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = MainSkillType.BulletLifeTime,
                                        rank = SkillRank.D,
                                    },
                                },
                                subSkills = new ShikigamiInfo.Prop.SubSkill[0]
                            }
                        },
                        instanceId = ConstShikigamiParameters.UNSET_SLOT_NUMBER,
                    }
                },
            }
        };
        /// <summary>チュートリアル用のペンダグラムターンテーブル情報</summary>
        public PentagramTurnTableInfo PentagramTurnTableInfoTutorial => pentagramTurnTableInfoTutorial;
    }
}
