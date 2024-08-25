using Main.Common;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// クリア報酬のコンテンツ
    /// </summary>
    public class ClearRewardTextContents : ClearContents, IClearRewardContents
    {
        /// <summary>タイトルに関するクリア報酬プロパティ</summary>
        [SerializeField] private ClearRewardProp[] clearRewardPropsOfTitle;
        /// <summary>変更前の説明内容に関するクリア報酬プロパティ</summary>
        [SerializeField] private ClearRewardProp[] clearRewardPropsOfBefore;
        /// <summary>名前に関するテキストのデフォルトフォーマット</summary>
        private const string DEFAULT_FORMAT_NAME = "ー";
        /// <summary>クリア報酬のコンテンツで表示する情報のビュー</summary>
        [SerializeField] private ClearRewardVisualMapsView clearRewardVisualMapsView;

        protected override void Reset()
        {
            base.Reset();
            clearRewardPropsOfTitle = new ClearRewardProp[]
            {
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.AddShikigami,
                    message = "式神の特徴",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhanceShikigami,
                    message = "強化の内容",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhancePlayer,
                    message = "強化の内容",
                },
            };
            clearRewardPropsOfBefore = new ClearRewardProp[]
            {
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.AddShikigami,
                    message = "タイプ：__AMGTshikigamiType__\n" +
                    "__AMGTmainSkillType__：__AMGTmainSkillTypeSkillRank__\n" +
                    "__AMGTmainSkillType2__：__AMGTmainSkillTypeSkillRank2__\n" +
                    "__AMGTmainSkillType3__：__AMGTmainSkillTypeSkillRank3__\n" +
                    "サブＡ：__AMGTsubSkillType____AMGTsubSkillTypeSkillRank__\n" +
                    "サブＢ：__AMGTsubSkillType2____AMGTsubSkillTypeSkillRank2__\n" +
                    "サブＣ：__AMGTsubSkillType3____AMGTsubSkillTypeSkillRank3__",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhanceShikigami,
                    message = "タイプ：__AMGTshikigamiType__\n" +
                    "__AMGTmainSkillType__：__AMGTmainSkillTypeSkillRank__\n" +
                    "__AMGTmainSkillType2__：__AMGTmainSkillTypeSkillRank2__\n" +
                    "__AMGTmainSkillType3__：__AMGTmainSkillTypeSkillRank3__\n" +
                    "サブＡ：__AMGTsubSkillType____AMGTsubSkillTypeSkillRank__\n" +
                    "サブＢ：__AMGTsubSkillType2____AMGTsubSkillTypeSkillRank2__\n" +
                    "サブＣ：__AMGTsubSkillType3____AMGTsubSkillTypeSkillRank3__",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhancePlayer,
                    message = "__AMGTshikigamiType__Ａ__AMGTmainSkillType__：__AMGTmainSkillTypeSkillRank__\n" +
                    "__AMGTshikigamiType__Ａ__AMGTmainSkillType2__：__AMGTmainSkillTypeSkillRank2__\n" +
                    "__AMGTshikigamiType__Ａ__AMGTmainSkillType3__：__AMGTmainSkillTypeSkillRank3__\n" +
                    "__AMGTshikigamiType__Ｂ__AMGTmainSkillType4__：__AMGTmainSkillTypeSkillRank4__\n" +
                    "__AMGTshikigamiType__Ｂ__AMGTmainSkillType5__：__AMGTmainSkillTypeSkillRank5__\n" +
                    "__AMGTshikigamiType__Ｂ__AMGTmainSkillType6__：__AMGTmainSkillTypeSkillRank6__"
                },
            };
            clearRewardVisualMapsView = GetComponent<ClearRewardVisualMapsView>();
        }

        public bool SetSoulMoney(int soulMoney)
        {
            return _mainViewUtility.SetSoulMoneyOfText(text, soulMoney, DEFAULT_FORMAT_SOUL_MONEY);
        }

        public bool SetTitleOfDescription(ClearRewardType clearRewardType)
        {
            try
            {
                var message = clearRewardPropsOfTitle.Where(q => q.clearRewardType.Equals(clearRewardType))
                    .Select(q => q.message)
                    .ToArray();
                if (message.Length < 1)
                    throw new System.ArgumentNullException($"タイトルに関するクリア報酬プロパティに該当する条件無し:[{clearRewardType}]");

                if (!_mainViewUtility.SetNameOfText(text, message[0], DEFAULT_FORMAT_NAME))
                    throw new System.Exception("SetNameOfText");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetPropetiesBeforeOfDescription(RewardContentProp rewardContentProp)
        {
            try
            {
                switch (rewardContentProp.rewardType)
                {
                    case ClearRewardType.AddShikigami:
                        var template = clearRewardPropsOfBefore.Where(q => q.clearRewardType.Equals(rewardContentProp.rewardType))
                            .Select(q => q.message)
                            .ToArray();
                        if (template.Length < 1)
                            throw new System.ArgumentNullException($"説明に関するクリア報酬プロパティに該当する条件無し:[{rewardContentProp.rewardType}]");

                        if (!_mainViewUtility.SetShikigamiInfoPropOfText(text, template[0], rewardContentProp.detailProp.beforeShikigamiInfoProp, new ShikigamiInfoVisualMaps()
                        {
                            shikigamiTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.shikigamiTypes,
                            mainSkilltypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.mainSkilltypes,
                            subSkillTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.subSkillTypes,
                        }, DEFAULT_FORMAT_NAME))
                            throw new System.Exception("SetShikigamiInfoPropOfText");

                        break;
                    case ClearRewardType.EnhanceShikigami:
                        var template1 = clearRewardPropsOfBefore.Where(q => q.clearRewardType.Equals(rewardContentProp.rewardType))
                            .Select(q => q.message)
                            .ToArray();
                        if (template1.Length < 1)
                            throw new System.ArgumentNullException($"説明に関するクリア報酬プロパティに該当する条件無し:[{rewardContentProp.rewardType}]");

                        if (!_mainViewUtility.SetShikigamiInfoPropOfText(text, template1[0], rewardContentProp.detailProp.beforeShikigamiInfoProp, new ShikigamiInfoVisualMaps()
                        {
                            shikigamiTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.shikigamiTypes,
                            mainSkilltypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.mainSkilltypes,
                            subSkillTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.subSkillTypes,
                        }, DEFAULT_FORMAT_NAME))
                            throw new System.Exception("SetShikigamiInfoPropOfText");

                        break;
                    case ClearRewardType.EnhancePlayer:
                        var template2 = clearRewardPropsOfBefore.Where(q => q.clearRewardType.Equals(rewardContentProp.rewardType))
                            .Select(q => q.message)
                            .ToArray();
                        if (template2.Length < 1)
                            throw new System.ArgumentNullException($"説明に関するクリア報酬プロパティに該当する条件無し:[{rewardContentProp.rewardType}]");

                        if (!_mainViewUtility.SetPlayerInfoPropOfText(text, template2[0], rewardContentProp.detailProp.playerInfoProp.beforePlayerInfoProps, new ShikigamiInfoVisualMaps()
                        {
                            shikigamiTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.shikigamiTypes,
                            mainSkilltypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.mainSkilltypes,
                            subSkillTypes = clearRewardVisualMapsView.ShikigamiInfoVisualMaps.subSkillTypes,
                        }, DEFAULT_FORMAT_NAME))
                            throw new System.Exception("SetPlayerInfoPropOfText");

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

        public bool SetName(string name)
        {
            return _mainViewUtility.SetNameOfText(text, name, DEFAULT_FORMAT_NAME);
        }
    }

    /// <summary>
    /// クリア報酬のコンテンツ
    /// インターフェース
    /// </summary>
    public interface IClearRewardContents : IClearContents
    {
        /// <summary>
        /// 名前をセット
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>成功／失敗</returns>
        public bool SetName(string name);
        /// <summary>
        /// 説明内のタイトルをセット
        /// </summary>
        /// <param name="clearRewardType">タイトル用報酬タイプ種別</param>
        /// <returns>成功／失敗</returns>
        public bool SetTitleOfDescription(ClearRewardType clearRewardType);
        /// <summary>
        /// 説明内の強化前の説明をセット
        /// </summary>
        /// <param name="rewardContentProp">リワード情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetPropetiesBeforeOfDescription(RewardContentProp rewardContentProp);
    }

    /// <summary>
    /// クリア報酬プロパティ
    /// </summary>
    [System.Serializable]
    public struct ClearRewardProp
    {
        /// <summary>タイトル用クリア報酬タイプ種別</summary>
        public ClearRewardType clearRewardType;
        /// <summary>メッセージ</summary>
        [TextArea]
        public string message;
    }
}
