using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Utility;
using TMPro;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// クリア報酬のコンテンツ
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ClearRewardTMPContents : MonoBehaviour, IClearRewardTMPContents
    {
        /// <summary>テキスト</summary>
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        /// <summary>変更前の説明内容に関するクリア報酬プロパティ</summary>
        [SerializeField] private ClearRewardProp[] clearRewardPropsOfAfter;
        /// <summary>名前に関するテキストのデフォルトフォーマット</summary>
        private const string DEFAULT_FORMAT_NAME = "ー";
        /// <summary>Mainのユーティリティ</summary>
        private MainViewUtility _mainViewUtility = new MainViewUtility();
        /// <summary>獲得経験値に関するテキストのデフォルトフォーマット</summary>
        protected const string DEFAULT_FORMAT_SOUL_MONEY = "0";
        /// <summary>値の更新をロック</summary>
        [SerializeField] private bool isLockValuePpdates;
        /// <summary>クリア報酬のコンテンツで表示する情報のビュー</summary>
        [SerializeField] private ClearRewardVisualMapsView clearRewardVisualMapsView;

        private void Reset()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            clearRewardPropsOfAfter = new ClearRewardProp[]
            {
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.AddShikigami,
                    message = "",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhanceShikigami,
                    message = "タイプ：__AMGTshikigamiType__\n" +
                    "__AMGTmainSkillType__：##AMGTmainSkillTypeEmphasisTypeBegin##__AMGTmainSkillTypeSkillRank__##AMGTmainSkillTypeEmphasisTypeEnd##\n" +
                    "__AMGTmainSkillType2__：##AMGTmainSkillTypeEmphasisType2Begin##__AMGTmainSkillTypeSkillRank2__##AMGTmainSkillTypeEmphasisType2End##\n" +
                    "__AMGTmainSkillType3__：##AMGTmainSkillTypeEmphasisType3Begin##__AMGTmainSkillTypeSkillRank3__##AMGTmainSkillTypeEmphasisType3End##\n" +
                    "サブＡ：##AMGTsubSkillTypeEmphasisTypeBegin##__AMGTsubSkillType____AMGTsubSkillTypeSkillRank__##AMGTsubSkillTypeEmphasisTypeEnd##\n" +
                    "サブＢ：##AMGTsubSkillTypeEmphasisType2Begin##__AMGTsubSkillType2____AMGTsubSkillTypeSkillRank2__##AMGTsubSkillTypeEmphasisType2End##\n" +
                    "サブＣ：##AMGTsubSkillTypeEmphasisType3Begin##__AMGTsubSkillType3____AMGTsubSkillTypeSkillRank3__##AMGTsubSkillTypeEmphasisType3End##",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhancePlayer,
                    message = "__AMGTshikigamiType__Ａ__AMGTmainSkillType__：##AMGTmainSkillTypeEmphasisTypeBegin##__AMGTmainSkillTypeSkillRank__##AMGTmainSkillTypeEmphasisTypeEnd##\n" +
                    "__AMGTshikigamiType__Ａ__AMGTmainSkillType2__：##AMGTmainSkillTypeEmphasisType2Begin##__AMGTmainSkillTypeSkillRank2__##AMGTmainSkillTypeEmphasisType2End##\n" +
                    "__AMGTshikigamiType__Ａ__AMGTmainSkillType3__：##AMGTmainSkillTypeEmphasisType3Begin##__AMGTmainSkillTypeSkillRank3__##AMGTmainSkillTypeEmphasisType3End##\n" +
                    "__AMGTshikigamiType__Ｂ__AMGTmainSkillType4__：##AMGTmainSkillTypeEmphasisType4Begin##__AMGTmainSkillTypeSkillRank4__##AMGTmainSkillTypeEmphasisType4End##\n" +
                    "__AMGTshikigamiType__Ｂ__AMGTmainSkillType5__：##AMGTmainSkillTypeEmphasisType5Begin##__AMGTmainSkillTypeSkillRank5__##AMGTmainSkillTypeEmphasisType5End##\n" +
                    "__AMGTshikigamiType__Ｂ__AMGTmainSkillType6__：##AMGTmainSkillTypeEmphasisType6Begin##__AMGTmainSkillTypeSkillRank6__##AMGTmainSkillTypeEmphasisType6End##"
                },
            };
            clearRewardVisualMapsView = GetComponent<ClearRewardVisualMapsView>();
        }

        public bool SetPropetiesAfterOfDescription(RewardContentProp rewardContentProp)
        {
            try
            {
                switch (rewardContentProp.rewardType)
                {
                    case ClearRewardType.AddShikigami:
                        // 式神追加の場合は差分表示が不要なため非表示にする
                        if (textMeshProUGUI.enabled)
                            textMeshProUGUI.enabled = false;

                        break;
                    case ClearRewardType.EnhanceShikigami:
                        // 比較表示の場合は表示する
                        if (!textMeshProUGUI.enabled)
                            textMeshProUGUI.enabled = true;
                        var template = clearRewardPropsOfAfter.Where(q => q.clearRewardType.Equals(rewardContentProp.rewardType))
                            .Select(q => q.message)
                            .ToArray();
                        if (template.Length < 1)
                            throw new System.ArgumentNullException($"説明に関するクリア報酬プロパティに該当する条件無し:[{rewardContentProp.rewardType}]");

                        if (!isLockValuePpdates)
                        {
                            if (!_mainViewUtility.SetShikigamiInfoPropOfText(textMeshProUGUI, template[0], rewardContentProp.detailProp.afterShikigamiInfoProp, new ShikigamiInfoVisualMaps()
                            {
                                shikigamiTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.shikigamiTypes,
                                mainSkilltypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.mainSkilltypes,
                                subSkillTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.subSkillTypes,
                            }, DEFAULT_FORMAT_NAME))
                                throw new System.Exception("SetShikigamiInfoPropOfText");
                        }

                        break;
                    case ClearRewardType.EnhancePlayer:
                        // 比較表示の場合は表示する
                        if (!textMeshProUGUI.enabled)
                            textMeshProUGUI.enabled = true;
                        var template1 = clearRewardPropsOfAfter.Where(q => q.clearRewardType.Equals(rewardContentProp.rewardType))
                            .Select(q => q.message)
                            .ToArray();
                        if (template1.Length < 1)
                            throw new System.ArgumentNullException($"説明に関するクリア報酬プロパティに該当する条件無し:[{rewardContentProp.rewardType}]");

                        if (!isLockValuePpdates)
                        {
                            if (!_mainViewUtility.SetPlayerInfoPropOfText(textMeshProUGUI, template1[0], rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps, new ShikigamiInfoVisualMaps()
                            {
                                shikigamiTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.shikigamiTypes,
                                mainSkilltypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.mainSkilltypes,
                                subSkillTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.subSkillTypes,
                            }, DEFAULT_FORMAT_NAME))
                                throw new System.Exception("SetPlayerInfoPropOfText");
                        }

                        break;
                    default:

                        break;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSoulMoney(int soulMoney)
        {
            return _mainViewUtility.SetSoulMoneyOfText(textMeshProUGUI, soulMoney, DEFAULT_FORMAT_SOUL_MONEY);
        }
    }

    /// <summary>
    /// クリア報酬のコンテンツ
    /// インターフェース
    /// </summary>
    public interface IClearRewardTMPContents : IClearContents
    {
        /// <summary>
        /// 説明内の強化前の説明をセット
        /// </summary>
        /// <param name="rewardContentProp">リワード情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetPropetiesAfterOfDescription(RewardContentProp rewardContentProp);
    }
}
