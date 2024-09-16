using Main.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Universal.Bean;

namespace Main.Utility
{
    /// <summary>
    /// 報酬情報管理
    /// ユーティリティ
    /// </summary>
    public class MainRewardsUtility : IMainRewardsUtility
    {
        /// <summary>共通のユーティリティ</summary>
        private MainCommonUtility _commonUtility = new MainCommonUtility();

        public UserBean.PentagramTurnTableInfo.Slot GetSlotUserBean(Common.RewardContentProp rewardContentProp)
        {
            switch (rewardContentProp.rewardType)
            {
                case ClearRewardType.AddShikigami:
                    return new UserBean.PentagramTurnTableInfo.Slot()
                    {
                        slotId = rewardContentProp.detailProp.beforeShikigamiInfoProp.slotId,
                        shikigamiInfo = new UserBean.ShikigamiInfo()
                        {
                            characterID = (int)rewardContentProp.detailProp.beforeShikigamiInfoProp.characterID,
                            genomeType = (int)rewardContentProp.detailProp.beforeShikigamiInfoProp.genomeType,
                            name = rewardContentProp.name,
                            type = (int)rewardContentProp.shikigamiType,
                            slotId = rewardContentProp.detailProp.beforeShikigamiInfoProp.slotId,
                            level = rewardContentProp.detailProp.beforeShikigamiInfoProp.level,
                            mainSkills = rewardContentProp.detailProp.beforeShikigamiInfoProp.mainSkills.Select(p =>
                                new UserBean.ShikigamiInfo.MainSkill()
                                {
                                    type = (int)p.type,
                                    rank = (int)p.rank,
                                }).ToArray(),
                            subSkills = new UserBean.ShikigamiInfo.SubSkill[] { },
                        },
                    };
                case ClearRewardType.EnhanceShikigami:
                    return new UserBean.PentagramTurnTableInfo.Slot()
                    {
                        slotId = rewardContentProp.detailProp.afterShikigamiInfoProp.slotId,
                        shikigamiInfo = new UserBean.ShikigamiInfo()
                        {
                            characterID = (int)rewardContentProp.detailProp.afterShikigamiInfoProp.characterID,
                            genomeType = (int)rewardContentProp.detailProp.afterShikigamiInfoProp.genomeType,
                            name = rewardContentProp.name,
                            type = (int)rewardContentProp.shikigamiType,
                            slotId = rewardContentProp.detailProp.afterShikigamiInfoProp.slotId,
                            level = rewardContentProp.detailProp.afterShikigamiInfoProp.level,
                            mainSkills = rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills.Select(p =>
                                new UserBean.ShikigamiInfo.MainSkill()
                                {
                                    type = (int)p.type,
                                    rank = (int)p.rank,
                                }).ToArray(),
                            subSkills = rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills != null &&
                                    0 < rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills.Length ?
                                rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills.Select(p =>
                                new UserBean.ShikigamiInfo.SubSkill()
                                {
                                    type = (int)p.type,
                                    rank = (int)p.rank,
                                }).ToArray() : new UserBean.ShikigamiInfo.SubSkill[] { },
                        },
                    };
                default:
                    throw new System.ArgumentOutOfRangeException($"スロットセットはこの処理では未使用: [{rewardContentProp.rewardType}]");
            }
        }

        public UserBean.PentagramTurnTableInfo.Slot GetSlotUserBeanPlayerInfo(Common.RewardContentProp rewardContentProp)
        {
            var prop = rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps.Where(p => p.mainSkills.Any(skill => !skill.emphasisType.Equals(EmphasisType.Neutral))).ToArray()[0];

            return new UserBean.PentagramTurnTableInfo.Slot()
            {
                slotId = prop.slotId,
                shikigamiInfo = new UserBean.ShikigamiInfo()
                {
                    characterID = (int)prop.characterID,
                    genomeType = (int)prop.genomeType,
                    name = rewardContentProp.name,
                    type = (int)rewardContentProp.shikigamiType,
                    slotId = prop.slotId,
                    level = prop.level,
                    mainSkills = prop.mainSkills.Select(p =>
                        new UserBean.ShikigamiInfo.MainSkill()
                        {
                            type = (int)p.type,
                            rank = (int)p.rank,
                        }).ToArray(),
                    subSkills = new UserBean.ShikigamiInfo.SubSkill[] { },
                },
            };
        }

        public Common.RewardContentProp[] InstanceRewardTablesAndGetRewards(ShikigamiInfoSplitesProp[] shikigamiInfoSplitesProps, EnhanceProp[] enhanceProps)
        {
            try
            {
                var slots = _commonUtility.UserDataSingleton.UserBean.pentagramTurnTableInfo.slots;
                // 取得済みの式神タイプは強化、未取得なら召喚で抽出
                var rewardContentProps = _commonUtility.AdminDataSingleton.AdminBean.levelDesign.rewardContentProps.Where(q => !slots.Any(slot => slot.shikigamiInfo.type == q.shikigamiInfo.type) &&
                    q.rewardType == (int)ClearRewardType.AddShikigami)
                    .ToList();
                // 強化の場合はランク+1でレコード作成
                rewardContentProps.AddRange(GetEnhanceRecord(slots, enhanceProps));
                List<Universal.Bean.RewardContentProp> rewardContentPropsDiced = new List<Universal.Bean.RewardContentProp>();
                // ラップダイスして、1件抽出
                // ダンスダイスして、1件抽出
                // グラフダイスして、1件抽出
                // 陰陽玉ダイスして、2件抽出

                // 作成者：ChatGPT-4o
                // 各typeごとにリストをグループ化
                var groupedByType = rewardContentProps.GroupBy(q => q.shikigamiInfo.type);
                foreach (var group in groupedByType.Where(q => q.Key == (int)ShikigamiType.Wrap ||
                    q.Key == (int)ShikigamiType.Dance ||
                    q.Key == (int)ShikigamiType.Graffiti))
                {
                    var randomOne = group.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
                    if (randomOne != null)
                    {
                        rewardContentPropsDiced.Add(randomOne);
                    }
                }
                // ランダム枠。異なるものをランダムに抽出して5枚になるまで追加
                var diceCountMax = 5 - rewardContentPropsDiced.Count;
                for (var i = 0; i < diceCountMax; i++)
                {
                    Universal.Bean.RewardContentProp randomOne = null;
                    var loopCnt = 0;
                    var loopCntMax = 10000;
                    // 同じプロパティが追加されないようにする
                    do
                    {
                        randomOne = rewardContentProps.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault();
                        loopCnt++;
                    }
                    while (loopCnt < loopCntMax &&
                       randomOne != null &&
                       rewardContentPropsDiced.Any(d =>
                           d.shikigamiInfo.type == randomOne.shikigamiInfo.type &&  // 式神タイプが同じ
                           d.shikigamiInfo.slotId == randomOne.shikigamiInfo.slotId &&  // スロットIDが同じ

                           // メインスキルのタイプとランクが重複
                            d.shikigamiInfo.mainSkills.Where(q => 0 < q.addRankCnt)
                                .Any(oms => randomOne.shikigamiInfo.mainSkills.Where(q => 0 < q.addRankCnt).Any(mms => mms.type == oms.type))
                           )); // メインスキルのタイプとランクがすべて一致

                    // 新しい内容が選ばれた場合は追加 ChatGPT 4o
                    if (randomOne != null)
                    {
                        rewardContentPropsDiced.Add(randomOne);
                    }
                }

                List<Common.RewardContentProp> rewardContentPropList = new List<Common.RewardContentProp>();
                foreach (var rewardContentProp in rewardContentPropsDiced.OrderBy(q => q.shikigamiInfo.type)
                    .Select((p, i) => new { Content = p, Index = i }))
                {
                    var prop = GetRewardContentDetailProp(new Common.RewardContentProp()
                    {
                        rewardID = (RewardID)rewardContentProp.Index,
                        rewardType = (ClearRewardType)rewardContentProp.Content.rewardType,
                        shikigamiType = (ShikigamiType)rewardContentProp.Content.shikigamiInfo.type,
                        rareType = (RareType)rewardContentProp.Content.rareType,
                        image = shikigamiInfoSplitesProps.Where(q => (int)q.shikigamiCharacterID == rewardContentProp.Content.shikigamiInfo.characterID)
                            .Select(q => q.image)
                            .ToArray()[0],
                        name = rewardContentProp.Content.shikigamiInfo.name,
                        soulMoney = rewardContentProp.Content.soulMoney,
                    }, rewardContentProp.Content, slots);
                    if (prop == null)
                        throw new System.Exception("GetRewardContentDetailProp");

                    rewardContentPropList.Add(prop);
                }

                return rewardContentPropList.ToArray();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// 強化用の報酬テーブルレコードを取得
        /// </summary>
        /// <param name="slots">スロット情報</param>
        /// <param name="enhanceProps">強化プロパティ</param>
        /// <returns>強化用の報酬テーブルレコード</returns>
        private Universal.Bean.RewardContentProp[] GetEnhanceRecord(Universal.Bean.UserBean.PentagramTurnTableInfo.Slot[] slots, EnhanceProp[] enhanceProps)
        {
            // スロットから式神情報を1件取り出す
            List<Universal.Bean.RewardContentProp> createdRewardContentProps = new List<Universal.Bean.RewardContentProp>();
            foreach (var slot in slots)
            {
                // 強化レベルごとの報酬を用意する
                foreach (var enhanceProp in enhanceProps)
                {
                    var createdRewardContentProp = new Universal.Bean.RewardContentProp()
                    {
                        rewardType = (ShikigamiType)slot.shikigamiInfo.type switch
                        {
                            ShikigamiType.OnmyoTurret => (int)ClearRewardType.EnhancePlayer,
                            _ => (int)ClearRewardType.EnhanceShikigami,
                        },
                        shikigamiInfo = slot.shikigamiInfo,
                        soulMoney = enhanceProp.soulMoney,
                    };
                    // メインスキルの数ごとに強化モードごとにランク+xの式神情報を作成
                    foreach (var mainSkill in createdRewardContentProp.shikigamiInfo.mainSkills.Select((p, i) => new { Content = p, Index = i })
                        .Where(q => q.Content.type != (int)MainSkillType.BulletLifeTime))
                    {
                        // （slotId、mailSkill.type、mailSkill.rank+x）でcreatedRewardContentPropsにランク+x登録済みかどうかを判定する
                        var length = createdRewardContentProps.Where(q => q.shikigamiInfo.slotId == createdRewardContentProp.shikigamiInfo.slotId &&
                            q.shikigamiInfo.mainSkills.Any(ms => ms.type == mainSkill.Content.type &&
                                mainSkill.Content.rank + (int)enhanceProp.level == ms.rank))
                            .ToArray()
                            .Length;
                        if (length < 1 &&
                             mainSkill.Content.rank + (int)enhanceProp.level <= (int)SkillRank.S)
                        {
                            // 存在しない場合はスキルランク+xを格納する
                            var clone = new Universal.Bean.RewardContentProp(createdRewardContentProp);
                            clone.shikigamiInfo.mainSkills[mainSkill.Index].rank += (int)enhanceProp.level;
                            clone.shikigamiInfo.mainSkills[mainSkill.Index].addRankCnt = (int)enhanceProp.level;
                            // 強化モードに応じてレア度を設定
                            // ●モード1ならノーマル
                            // ●モード2ならレア
                            // ●モード3ならSレア
                            clone.rareType = (int)enhanceProp.level - 1;
                            createdRewardContentProps.Add(clone);
                        }
                    }
                    // createdRewardContentPropsにランク+1をxつ登録済みかどうかを判定する
                    switch (enhanceProp.level)
                    {
                        case EnhanceLevel.Mode2:
                            createdRewardContentProps = InstanceMltRank(createdRewardContentProp, createdRewardContentProps, enhanceProp.level);

                            break;
                        case EnhanceLevel.Mode3:
                            createdRewardContentProps = InstanceMltRank(createdRewardContentProp, createdRewardContentProps, enhanceProp.level);

                            break;
                        default:
                            break;
                    }
                    switch ((ClearRewardType)createdRewardContentProp.rewardType)
                    {
                        case ClearRewardType.EnhanceShikigami:
                            // サブスキルがあれば、その数ごとにランク+1の式神情報を作成
                            if (createdRewardContentProp.shikigamiInfo.subSkills != null &&
                                0 < createdRewardContentProp.shikigamiInfo.subSkills.Length)
                            {
                                foreach (var subSkill in createdRewardContentProp.shikigamiInfo.subSkills.Select((p, i) => new { Content = p, Index = i }))
                                {
                                    // （slotId、subSkill.type、subSkill.rank+1）でcreatedRewardContentPropsにランク+1登録済みかどうかを判定する
                                    var length = createdRewardContentProps.Where(q => q.shikigamiInfo.slotId == createdRewardContentProp.shikigamiInfo.slotId &&
                                            q.shikigamiInfo.subSkills != null &&
                                            0 < q.shikigamiInfo.subSkills.Length &&
                                            q.shikigamiInfo.subSkills.Any(ms => ms.type == subSkill.Content.type &&
                                            subSkill.Content.rank < ms.rank))
                                        .ToArray()
                                        .Length;
                                    if (length < 1)
                                    {
                                        // 存在しない場合はスキルランク+1を格納する
                                        var clone = new Universal.Bean.RewardContentProp(createdRewardContentProp);
                                        clone.shikigamiInfo.subSkills[subSkill.Index].rank++;
                                        createdRewardContentProps.Add(clone);
                                    }
                                }
                            }
                            // ラップ、ダンス、グラフィティの場合はさらに、報酬テーブルから未取得のサブスキルを取得して、サブスキル追加の式神情報を作成
                            var addSubSkills = _commonUtility.AdminDataSingleton.AdminBean.levelDesign.rewardContentProps.Where(q => q.rewardType == (int)ClearRewardType.EnhanceShikigami &&
                                q.shikigamiInfo.characterID == createdRewardContentProp.shikigamiInfo.characterID &&
                                q.shikigamiInfo.genomeType == createdRewardContentProp.shikigamiInfo.genomeType &&
                                q.shikigamiInfo.subSkills != null &&
                                0 < q.shikigamiInfo.subSkills.Length)
                                .ToArray();

                            if (0 < addSubSkills.Length)
                            {
                                if (createdRewardContentProp.shikigamiInfo.subSkills != null &&
                                    0 < createdRewardContentProp.shikigamiInfo.subSkills.Length)
                                {
                                    var skillsToRemove = new List<Universal.Bean.UserBean.ShikigamiInfo.SubSkill>();
                                    foreach (var addSubSkill in addSubSkills[0].shikigamiInfo.subSkills)
                                    {
                                        foreach (var gotSubSkill in createdRewardContentProp.shikigamiInfo.subSkills)
                                        {
                                            if (gotSubSkill.type == addSubSkill.type)
                                            {
                                                skillsToRemove.Add(gotSubSkill);
                                            }
                                        }
                                    }
                                    foreach (var item in skillsToRemove)
                                    {
                                        var list = addSubSkills[0].shikigamiInfo.subSkills.ToList();
                                        list.Remove(item);
                                        addSubSkills[0].shikigamiInfo.subSkills = list.ToArray();
                                    }
                                }
                                if (addSubSkills[0].shikigamiInfo.subSkills != null &&
                                    0 < addSubSkills[0].shikigamiInfo.subSkills.Length)
                                {
                                    foreach (var addSubSkill in addSubSkills[0].shikigamiInfo.subSkills)
                                    {
                                        // 存在しない場合はスキルランク+1を格納する
                                        var clone = new Universal.Bean.RewardContentProp();
                                        clone = createdRewardContentProp;
                                        var list = clone.shikigamiInfo.subSkills.ToList();
                                        list.Add(addSubSkill);
                                        clone.shikigamiInfo.subSkills = list.ToArray();
                                        createdRewardContentProps.Add(clone);
                                    }
                                }
                            }

                            break;
                        case ClearRewardType.EnhancePlayer:
                            // プレイヤー強化にサブスキルは無関係

                            break;
                        default:
                            throw new System.ArgumentOutOfRangeException($"対象外のタイトル用クリア報酬タイプ種別: [{(ClearRewardType)createdRewardContentProp.rewardType}]");
                    }
                }
            }

            return createdRewardContentProps.ToArray();
        }

        /// <summary>
        /// 複数ランク上げを生成
        /// </summary>
        /// <param name="createdRewardContentProp">強化対象</param>
        /// <param name="createdRewardContentProps">強化用の報酬テーブル</param>
        /// <param name="enhanceLevel">強化モード</param>
        /// <returns>複数ランク上げ報酬</returns>
        private List<Universal.Bean.RewardContentProp> InstanceMltRank(Universal.Bean.RewardContentProp createdRewardContentProp, List<Universal.Bean.RewardContentProp> createdRewardContentProps, EnhanceLevel enhanceLevel)
        {
            // createdRewardContentPropsにランク+1を2つ登録済みかどうかを判定する ChatGPT 4o
            if (!createdRewardContentProp.shikigamiInfo.mainSkills
                .Where(mainSkill => mainSkill.type != (int)MainSkillType.BulletLifeTime) // BulletLifeTime 以外のスキルを対象
                .Any(mainSkill =>
                    createdRewardContentProps.Any(q =>
                        q.shikigamiInfo.slotId == createdRewardContentProp.shikigamiInfo.slotId &&
                        q.shikigamiInfo.mainSkills.Count(ms => ms.type == mainSkill.type && ms.rank == mainSkill.rank + 1) == (int)enhanceLevel)
                ))
            {

                List<Dictionary<string, MainSkillType>> mainSkillsMatchs = new List<Dictionary<string, MainSkillType>>();
                var mainSkills = createdRewardContentProp.shikigamiInfo.mainSkills.Select((p, i) => new { Content = p, Index = i })
                    .Where(q => q.Content.type != (int)MainSkillType.BulletLifeTime &&
                        q.Index < createdRewardContentProp.shikigamiInfo.mainSkills.Length - 1);
                switch (enhanceLevel)
                {
                    case EnhanceLevel.Mode2:
                        foreach (var current in mainSkills)
                        {
                            foreach (var next in mainSkills)
                            {
                                if (current.Content.type != next.Content.type)
                                {
                                    Dictionary<string, MainSkillType> mainSkillsMatch = new Dictionary<string, MainSkillType>();
                                    mainSkillsMatch["current"] = (MainSkillType)current.Content.type;
                                    mainSkillsMatch["next"] = (MainSkillType)next.Content.type;
                                    var length = mainSkillsMatchs.Where(q => (q["current"] == mainSkillsMatch["next"] &&
                                                q["next"] == mainSkillsMatch["current"]) ||
                                            (q["current"] == mainSkillsMatch["current"] &&
                                                q["next"] == mainSkillsMatch["next"]))
                                        .ToArray()
                                        .Length;
                                    if (length < 1)
                                        mainSkillsMatchs.Add(mainSkillsMatch);
                                }
                            }
                        }

                        break;
                    case EnhanceLevel.Mode3:
                        // ChatGPT 4o
                        foreach (var current in mainSkills)
                        {
                            foreach (var next in mainSkills)
                            {
                                foreach (var more in mainSkills)
                                {
                                    if (current.Content.type != next.Content.type &&
                                        current.Content.type != more.Content.type &&
                                        next.Content.type != more.Content.type)
                                    {
                                        // current, next, more の値をセットに格納
                                        HashSet<MainSkillType> currentSet = new HashSet<MainSkillType>
                                        {
                                            (MainSkillType)current.Content.type,
                                            (MainSkillType)next.Content.type,
                                            (MainSkillType)more.Content.type
                                        };
                                        // 既存のmainSkillsMatchsの組み合わせと比較する
                                        bool matchFound = mainSkillsMatchs.Any(match =>
                                            new HashSet<MainSkillType>
                                            {
                                                match["current"],
                                                match["next"],
                                                match["more"]
                                            }.SetEquals(currentSet) // SetEquals でセット同士を比較
                                        );
                                        // 一致する組み合わせがない場合、mainSkillsMatchsに追加
                                        if (!matchFound)
                                        {
                                            Dictionary<string, MainSkillType> mainSkillsMatch = new Dictionary<string, MainSkillType>
                                            {
                                                { "current", (MainSkillType)current.Content.type },
                                                { "next", (MainSkillType)next.Content.type },
                                                { "more", (MainSkillType)more.Content.type }
                                            };
                                            mainSkillsMatchs.Add(mainSkillsMatch);
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException($"未到達想定の強化モード: [{enhanceLevel}]");
                }
                foreach (var mainSkillsMatch in mainSkillsMatchs)
                {
                    var clone = new Universal.Bean.RewardContentProp(createdRewardContentProp);
                    var count = 0;
                    // 存在しない場合はスキルランク+1を2つ格納する
                    for (var i = 0; i < clone.shikigamiInfo.mainSkills.Length; i++)
                    {
                        switch (enhanceLevel)
                        {
                            case EnhanceLevel.Mode2:
                                if (clone.shikigamiInfo.mainSkills[i].rank < (int)SkillRank.S &&
                                        ((int)mainSkillsMatch["current"] == clone.shikigamiInfo.mainSkills[i].type ||
                                        (int)mainSkillsMatch["next"] == clone.shikigamiInfo.mainSkills[i].type))
                                {
                                    clone.shikigamiInfo.mainSkills[i].rank++;
                                    clone.shikigamiInfo.mainSkills[i].addRankCnt = 1;
                                    clone.rareType = (int)RareType.Rare;
                                    count++;
                                }

                                break;
                            case EnhanceLevel.Mode3:
                                if (clone.shikigamiInfo.mainSkills[i].rank < (int)SkillRank.S && 
                                        ((int)mainSkillsMatch["current"] == clone.shikigamiInfo.mainSkills[i].type ||
                                        (int)mainSkillsMatch["next"] == clone.shikigamiInfo.mainSkills[i].type ||
                                        (int)mainSkillsMatch["more"] == clone.shikigamiInfo.mainSkills[i].type))
                                {
                                    clone.shikigamiInfo.mainSkills[i].rank++;
                                    clone.shikigamiInfo.mainSkills[i].addRankCnt = 1;
                                    clone.rareType = (int)RareType.SRare;
                                    count++;
                                }

                                break;
                            default:
                                throw new System.ArgumentOutOfRangeException($"未到達想定の強化モード: [{enhanceLevel}]");
                        }
                        if ((int)enhanceLevel <= count)
                        {
                            createdRewardContentProps.Add(clone);
                            break;
                        }
                    }
                }
            }

            return createdRewardContentProps;
        }

        /// <summary>
        /// クリア報酬のコンテンツ詳細のプロパティをセットして取得
        /// </summary>
        /// <param name="rewardContentProp">クリア報酬のコンテンツプロパティ</param>
        /// <param name="rewardContentPropBean">クリア報酬のコンテンツプロパティ（Bean側）</param>
        /// <param name="slots">スロット情報</param>
        /// <returns>クリア報酬のコンテンツプロパティ</returns>
        private Common.RewardContentProp GetRewardContentDetailProp(Common.RewardContentProp rewardContentProp, Universal.Bean.RewardContentProp rewardContentPropBean, Universal.Bean.UserBean.PentagramTurnTableInfo.Slot[] slots)
        {
            try
            {
                switch (rewardContentProp.rewardType)
                {
                    case ClearRewardType.AddShikigami:
                        // 召喚の場合は、パラメータ情報（変更前）へスキル情報をセット
                        rewardContentProp.detailProp = new RewardContentDetailProp()
                        {
                            beforeShikigamiInfoProp = new ShikigamiInfo.Prop()
                            {
                                mainSkills = rewardContentPropBean.shikigamiInfo.mainSkills.Select(q => new ShikigamiInfo.Prop.MainSkill()
                                {
                                    type = (MainSkillType)q.type,
                                    rank = (SkillRank)q.rank,
                                })
                                    .ToArray(),
                                type = (ShikigamiType)rewardContentPropBean.shikigamiInfo.type,
                                slotId = rewardContentPropBean.shikigamiInfo.slotId,
                                characterID = (ShikigamiCharacterID)rewardContentPropBean.shikigamiInfo.characterID,
                                level = rewardContentPropBean.shikigamiInfo.level,
                            },
                        };

                        return rewardContentProp;
                    case ClearRewardType.EnhanceShikigami:
                        // 強化の場合は、
                        //  パラメータ情報（変更前）へ現在スロットへセットされている式神のスキル情報をセット
                        //  パラメータ情報（変更後）へスキル情報をセット
                        rewardContentProp.detailProp = new RewardContentDetailProp()
                        {
                            beforeShikigamiInfoProp = new ShikigamiInfo.Prop()
                            {
                                mainSkills = slots.Where(q => q.shikigamiInfo.type == rewardContentPropBean.shikigamiInfo.type)
                                    .Select(q => q.shikigamiInfo.mainSkills)
                                    .ToArray()[0]
                                    .Select(q => new ShikigamiInfo.Prop.MainSkill()
                                    {
                                        type = (MainSkillType)q.type,
                                        rank= (SkillRank)q.rank,
                                    })
                                        .ToArray(),
                                type = (ShikigamiType)rewardContentPropBean.shikigamiInfo.type,
                            },
                            afterShikigamiInfoProp = new ShikigamiInfo.Prop()
                            {
                                mainSkills = rewardContentPropBean.shikigamiInfo.mainSkills.Select(q => new ShikigamiInfo.Prop.MainSkill()
                                {
                                    type = (MainSkillType)q.type,
                                    rank = (SkillRank)q.rank,
                                })
                                    .ToArray(),
                                type = (ShikigamiType)rewardContentPropBean.shikigamiInfo.type,
                                slotId = rewardContentPropBean.shikigamiInfo.slotId,
                                characterID = (ShikigamiCharacterID)rewardContentPropBean.shikigamiInfo.characterID,
                                level = rewardContentPropBean.shikigamiInfo.level,
                            },
                        };
                        // サブスキルがあるなら格納
                        if (0 < rewardContentPropBean.shikigamiInfo.subSkills.Length)
                        {
                            rewardContentProp.detailProp.beforeShikigamiInfoProp.subSkills = slots.Where(q => q.shikigamiInfo.type == rewardContentPropBean.shikigamiInfo.type)
                                .Select(q => q.shikigamiInfo.subSkills)
                                .ToArray()[0]
                                .Select(q => new ShikigamiInfo.Prop.SubSkill()
                                {
                                    type = (SubSkillType)q.type,
                                    rank = (SkillRank)q.rank,
                                })
                                    .ToArray();
                            rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills = rewardContentPropBean.shikigamiInfo.subSkills.Select(q => new ShikigamiInfo.Prop.SubSkill()
                            {
                                type = (SubSkillType)q.type,
                                rank = (SkillRank)q.rank,
                            })
                                .ToArray();
                        }
                        // 差分比較して強調（メインスキル）
                        foreach (var before in rewardContentProp.detailProp.beforeShikigamiInfoProp.mainSkills)
                            for (var i = 0; i < rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills.Length; i++)
                                if (rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].type.Equals(before.type))
                                {
                                    if (before.rank < rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].rank)
                                        rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].emphasisType = EmphasisType.Positive;
                                    else if (rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].rank < before.rank)
                                        rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].emphasisType = EmphasisType.Negative;
                                    else
                                        rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].emphasisType = EmphasisType.Neutral;
                                }
                        // 差分比較して強調（サブスキル）
                        if (rewardContentProp.detailProp.beforeShikigamiInfoProp.subSkills != null &&
                            0 < rewardContentProp.detailProp.beforeShikigamiInfoProp.subSkills.Length &&
                            rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills != null &&
                            0 < rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills.Length)
                        {
                            foreach (var before in rewardContentProp.detailProp.beforeShikigamiInfoProp.subSkills.Select((p, i) => new { Content = p, Index = i }))
                                for (var i = 0; i < rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills.Length; i++)
                                {
                                    // 新規追加スキルの場合は比較は不要。強調表示とする。
                                    if (before.Index < i)
                                    {
                                        rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].emphasisType = EmphasisType.Positive;

                                        continue;
                                    }
                                    if (rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].type.Equals(before.Content.type))
                                    {
                                        if (before.Content.rank < rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].rank)
                                            rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].emphasisType = EmphasisType.Positive;
                                        else if (rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].rank < before.Content.rank)
                                            rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].emphasisType = EmphasisType.Negative;
                                        else
                                            rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].emphasisType = EmphasisType.Neutral;
                                    }
                                }
                        }
                        else if (rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills != null &&
                            0 < rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills.Length)
                        {
                            // スロットの式神が持つサブスキルが0だった場合
                            for (var i = 0; i < rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills.Length; i++)
                                // 新規追加スキルの場合は比較は不要。強調表示とする。
                                rewardContentProp.detailProp.afterShikigamiInfoProp.subSkills[i].emphasisType = EmphasisType.Positive;
                        }

                        return rewardContentProp;
                    case ClearRewardType.EnhancePlayer:
                        // プレイヤー強化はさらに特殊
                        // 陰陽玉AとBを並べなければならないため2つの情報を両方セットする
                        rewardContentProp.detailProp = new RewardContentDetailProp()
                        {
                            playerInfoProp = new RewardContentDetailProp.PlayerInfo()
                            {
                                beforePlayerInfoProps = new ShikigamiInfo.Prop[2],
                                afterPlayerInfoProps = new ShikigamiInfo.Prop[2],
                            }
                        };
                        foreach (var item in slots.Where(q => q.shikigamiInfo.type == rewardContentPropBean.shikigamiInfo.type)
                            .Select(q => q.shikigamiInfo)
                            .OrderBy(q => q.slotId)
                            .Select((p, i) => new { Content = p, Index = i }))
                        {
                            rewardContentProp.detailProp.playerInfoProp.beforePlayerInfoProps[item.Index].slotId = item.Content.slotId;
                            rewardContentProp.detailProp.playerInfoProp.beforePlayerInfoProps[item.Index].mainSkills = item.Content.mainSkills.Select(q => new ShikigamiInfo.Prop.MainSkill()
                            {
                                type = (MainSkillType)q.type,
                                rank = (SkillRank)q.rank,
                            })
                                .ToArray();
                            rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].type = (ShikigamiType)rewardContentPropBean.shikigamiInfo.type;
                            rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].characterID = (ShikigamiCharacterID)item.Content.characterID;
                            rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].level = item.Content.level;
                            rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].type = (ShikigamiType)item.Content.type;
                            if (item.Content.slotId == rewardContentPropBean.shikigamiInfo.slotId)
                            {
                                rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].slotId = rewardContentPropBean.shikigamiInfo.slotId;
                                rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].mainSkills = rewardContentPropBean.shikigamiInfo.mainSkills.Select(q => new ShikigamiInfo.Prop.MainSkill()
                                {
                                    type = (MainSkillType)q.type,
                                    rank = (SkillRank)q.rank,
                                })
                                    .ToArray();
                            }
                            else
                            {
                                rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].slotId = item.Content.slotId;
                                rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[item.Index].mainSkills = item.Content.mainSkills.Select(q => new ShikigamiInfo.Prop.MainSkill()
                                {
                                    type = (MainSkillType)q.type,
                                    rank = (SkillRank)q.rank,
                                })
                                    .ToArray();
                            }
                        }
                        // 差分比較して強調（メインスキル）
                        foreach (var beforeProp in rewardContentProp.detailProp.playerInfoProp.beforePlayerInfoProps.Select((p, i) => new { Content = p, Index = i }))
                        {
                            foreach (var before in beforeProp.Content.mainSkills.Select((p, i) => new { Content = p, Index = i }))
                            {
                                for (var i = 0; i < rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps.Length; i++)
                                {
                                    for (var j = 0; j < rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills.Length; j++)
                                    {
                                        if (beforeProp.Index == i &&
                                            rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].type.Equals(before.Content.type))
                                        {
                                            if (before.Content.rank < rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].rank)
                                                rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].emphasisType = EmphasisType.Positive;
                                            else if (rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].rank < before.Content.rank)
                                                rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].emphasisType = EmphasisType.Negative;
                                            else
                                                rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].emphasisType = EmphasisType.Neutral;
                                        }
                                    }
                                }
                            }
                        }

                        return rewardContentProp;
                    default:
                        throw new System.ArgumentOutOfRangeException($"存在しないリワードタイプ: [{rewardContentProp.rewardType}]");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public Common.RewardContentProp[] MergeRewards(Common.RewardContentProp[] rewardContentProps)
        {
            List<Common.RewardContentProp> newRewardContentProps = new List<Common.RewardContentProp>();
            newRewardContentProps = GetMergeEnhanceShikigami(rewardContentProps, newRewardContentProps);
            newRewardContentProps = GetMergeEnhancePlayer(rewardContentProps, newRewardContentProps);
            var oldRewardContentProps = rewardContentProps.Where(q =>
                // 式神強化    
                (q.rewardType.Equals(ClearRewardType.EnhanceShikigami) &&
                !newRewardContentProps.Any(ncp => ncp.rewardType.Equals(ClearRewardType.EnhanceShikigami) &&
                    ncp.detailProp.afterShikigamiInfoProp.slotId == q.detailProp.afterShikigamiInfoProp.slotId)) ||
                // 式神召喚
                q.rewardType.Equals(ClearRewardType.AddShikigami))
                .ToArray();
            // マージ対象とならなかったプロパティをまとめてリストへ追加
            newRewardContentProps.AddRange(oldRewardContentProps);

            return newRewardContentProps.ToArray();
        }

        /// <summary>
        /// 式神強化のマージを取得
        /// </summary>
        /// <param name="rewardContentProps">クリア報酬のコンテンツプロパティ</param>
        /// <param name="newRewardContentProps">マージ後のプロパティ</param>
        /// <returns>マージ後のプロパティ</returns>
        private List<Common.RewardContentProp> GetMergeEnhanceShikigami(Common.RewardContentProp[] rewardContentProps, List<Common.RewardContentProp> newRewardContentProps)
        {
            Dictionary<SlotId, int> slotCnt = new Dictionary<SlotId, int>();
            var enhanceShikigami = rewardContentProps.Where(q => q.rewardType.Equals(ClearRewardType.EnhanceShikigami));
            foreach (var slotId in enhanceShikigami.Select(q => q.detailProp.afterShikigamiInfoProp.slotId)
                .Distinct())
                slotCnt[(SlotId)slotId] = enhanceShikigami.Where(q => q.detailProp.afterShikigamiInfoProp.slotId == slotId).ToArray().Length;
            // スロットIDが複数存在するクリア報酬が該当する
            foreach (var slotId in slotCnt.Where(q => 1 < q.Value)
                .Select(q => q.Key))
            {
                Common.RewardContentProp newRewardContentProp = null;
                foreach (var rewardContentProp in enhanceShikigami.Where(q => q.detailProp.afterShikigamiInfoProp.slotId == (int)slotId))
                    if (newRewardContentProp == null)
                        // 1件目はそのまま入れる
                        newRewardContentProp = rewardContentProp;
                    else
                    {
                        // 2件目以降はスキルランクアップを反映する
                        for (var i = 0; i < newRewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills.Length; i++)
                            if (newRewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].rank < rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].rank)
                                newRewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].rank = rewardContentProp.detailProp.afterShikigamiInfoProp.mainSkills[i].rank;
                    }
                newRewardContentProps.Add(newRewardContentProp);
            }

            return newRewardContentProps;
        }

        /// <summary>
        /// プレイヤー強化のマージを取得
        /// </summary>
        /// <param name="rewardContentProps">クリア報酬のコンテンツプロパティ</param>
        /// <param name="newRewardContentProps">マージ後のプロパティ</param>
        /// <returns>マージ後のプロパティ</returns>
        private List<Common.RewardContentProp> GetMergeEnhancePlayer(Common.RewardContentProp[] rewardContentProps, List<Common.RewardContentProp> newRewardContentProps)
        {
            List<SlotId> slotIds = new List<SlotId>();
            var enhancePlayer = rewardContentProps.Where(q => q.rewardType.Equals(ClearRewardType.EnhancePlayer));
            foreach (var rewardContentProp in enhancePlayer)
            {
                slotIds.AddRange(rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps.Select(q => (SlotId)q.slotId)
                    .Distinct()
                    .ToArray());
            }
            // スロットIDが複数存在するクリア報酬が該当する
            foreach (var slotId in slotIds.Distinct())
            {
                Common.RewardContentProp newRewardContentProp = null;
                var target = enhancePlayer.Where(q =>
                        q.detailProp.playerInfoProp.afterPlayerInfoProps.Any(apip =>
                            !apip.mainSkills.All(ms => ms.emphasisType.Equals(EmphasisType.Neutral)) &&
                            apip.slotId == (int)slotId))
                    .ToArray();
                foreach (var rewardContentProp in target)
                    if (newRewardContentProp == null)
                        // 1件目はそのまま入れる
                        newRewardContentProp = rewardContentProp;
                    else
                    {
                        // 2件目以降はスキルランクアップを反映する
                        for (var i = 0; i < newRewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps.Length; i++)
                        {
                            if (newRewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills.All(ms => ms.emphasisType.Equals(EmphasisType.Neutral)))
                                continue;

                            for (var j = 0; j < newRewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills.Length; j++)
                                if (newRewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].rank < rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].rank)
                                {
                                    newRewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].rank = rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].rank;
                                    newRewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].emphasisType = rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps[i].mainSkills[j].emphasisType;
                                }
                        }
                    }
                if (newRewardContentProp != null)
                    newRewardContentProps.Add(newRewardContentProp);
            }

            return newRewardContentProps;
        }
    }

    /// <summary>
    /// 報酬情報管理
    /// ユーティリティ
    /// インターフェース
    /// </summary>
    public interface IMainRewardsUtility
    {
        /// <summary>
        /// 報酬テーブルを生成
        /// 現在のプレイヤー情報を元に報酬内容を選別して取得する
        /// </summary>
        /// <param name="shikigamiInfoSplitesProps">式神と画像を連携する情報</param>
        /// <param name="enhanceProps">強化プロパティ</param>
        /// <returns>クリア報酬のコンテンツプロパティ</returns>
        public Common.RewardContentProp[] InstanceRewardTablesAndGetRewards(ShikigamiInfoSplitesProp[] shikigamiInfoSplitesProps, EnhanceProp[] enhanceProps);
        /// <summary>
        /// クリア報酬のコンテンツプロパティのマージ
        /// 式神1体に対して複数の報酬の効果を適用する
        /// </summary>
        /// <param name="rewardContentProps">クリア報酬のコンテンツプロパティ</param>
        /// <returns>クリア報酬のコンテンツプロパティ</returns>
        public Common.RewardContentProp[] MergeRewards(Common.RewardContentProp[] rewardContentProps);
        /// <summary>
        /// スロットへセットして取得
        /// </summary>
        /// <param name="rewardContentProp">クリア報酬のコンテンツプロパティ</param>
        /// <returns>スロット</returns>
        public UserBean.PentagramTurnTableInfo.Slot GetSlotUserBean(Common.RewardContentProp rewardContentProp);
        /// <summary>
        /// スロットへセットして取得（陰陽玉）
        /// </summary>
        /// <param name="rewardContentProp">クリア報酬のコンテンツプロパティ</param>
        /// <returns>スロット</returns>
        public UserBean.PentagramTurnTableInfo.Slot GetSlotUserBeanPlayerInfo(Common.RewardContentProp rewardContentProp);
    }
}
