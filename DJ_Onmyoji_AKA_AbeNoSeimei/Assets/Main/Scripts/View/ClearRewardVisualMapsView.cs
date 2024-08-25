using Main.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// クリア報酬のコンテンツで表示する情報
    /// ビュー
    /// </summary>
    public class ClearRewardVisualMapsView : MonoBehaviour
    {
        /// <summary>ステータス表示で使用</summary>
        [SerializeField]
        private ShikigamiInfoVisualMaps shikigamiInfoVisualMaps = new ShikigamiInfoVisualMaps()
        {
            shikigamiTypes = new ShikigamiInfoVisualMapOfShikigamiType[]
            {
                new ShikigamiInfoVisualMapOfShikigamiType()
                {
                    type = ShikigamiType.Wrap,
                    value = "ラップ",
                },
                new ShikigamiInfoVisualMapOfShikigamiType()
                {
                    type = ShikigamiType.Dance,
                    value = "ダンス",
                },
                new ShikigamiInfoVisualMapOfShikigamiType()
                {
                    type = ShikigamiType.Graffiti,
                    value = "グラフィティ",
                },
                new ShikigamiInfoVisualMapOfShikigamiType()
                {
                    type = ShikigamiType.OnmyoTurret,
                    value = "陰陽玉",
                },
            },
            mainSkilltypes = new ShikigamiInfoVisualMapOfMainSkillType[]
            {
                new ShikigamiInfoVisualMapOfMainSkillType()
                {
                    type = MainSkillType.ActionRate,
                    value = "攻撃間隔",
                },
                new ShikigamiInfoVisualMapOfMainSkillType()
                {
                    type = MainSkillType.AttackPoint,
                    value = "ダメージ",
                },
                new ShikigamiInfoVisualMapOfMainSkillType()
                {
                    type = MainSkillType.BulletLifeTime,
                    value = "飛距離",
                },
                new ShikigamiInfoVisualMapOfMainSkillType()
                {
                    type = MainSkillType.Range,
                    value = "攻撃範囲",
                },
                new ShikigamiInfoVisualMapOfMainSkillType()
                {
                    type = MainSkillType.DebuffEffectLifeTime,
                    value = "展開時間",
                },
            },
            subSkillTypes = new ShikigamiInfoVisualMapOfSubSkillType[]
            {
                new ShikigamiInfoVisualMapOfSubSkillType()
                {
                    type = SubSkillType.Explosion,
                    value = "爆発",
                },
                new ShikigamiInfoVisualMapOfSubSkillType()
                {
                    type = SubSkillType.Homing,
                    value = "追尾性能",
                },
                new ShikigamiInfoVisualMapOfSubSkillType()
                {
                    type = SubSkillType.Penetrating,
                    value = "貫通",
                },
                new ShikigamiInfoVisualMapOfSubSkillType()
                {
                    type = SubSkillType.Spreading,
                    value = "拡散",
                },
                new ShikigamiInfoVisualMapOfSubSkillType()
                {
                    type = SubSkillType.ContinuousFire,
                    value = "連射",
                },
            },
        };

        /// <summary>ステータス表示で使用</summary>
        public ShikigamiInfoVisualMaps ShikigamiInfoVisualMaps => shikigamiInfoVisualMaps;
    }
}
