using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using UnityEngine;

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
                    message = "タイプ：ラップ\n攻撃　：Ａ\n連射　：Ｂ\n持続　：Ｃ\nサブＡ：追尾Ａ\nサブＢ：ー\nサブＣ：ー",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhanceShikigami,
                    message = "タイプ：ラップ\n攻撃　：Ａ\n連射　：Ｂ\n持続　：Ｃ\nサブＡ：追尾Ａ\nサブＢ：ー\nサブＣ：ー",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhancePlayer,
                    message = "最大体力　　　：100\n陰陽玉Ａ攻撃　：Ａ\n陰陽玉Ａ連射　：Ｂ\n陰陽玉Ａ持続　：Ｃ\n陰陽玉Ｂ攻撃　：Ａ\n陰陽玉Ｂ連射　：Ｂ\n陰陽玉Ｂ持続　：Ｃ\n",
                },
            };
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

        public bool SetPropetiesBeforeOfDescription(ClearRewardType clearRewardType)
        {
            try
            {
                var message = clearRewardPropsOfBefore.Where(q => q.clearRewardType.Equals(clearRewardType))
                    .Select(q => q.message)
                    .ToArray();
                if (message.Length < 1)
                    throw new System.ArgumentNullException($"説明に関するクリア報酬プロパティに該当する条件無し:[{clearRewardType}]");

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
        /// <param name="clearRewardType">タイトル用報酬タイプ種別</param>
        /// <returns>成功／失敗</returns>
        public bool SetPropetiesBeforeOfDescription(ClearRewardType clearRewardType);
    }

    /// <summary>
    /// クリア報酬のコンテンツ
    /// プロパティ
    /// </summary>
    public struct ClearRewardContentsState
    {
        /// <summary>魂のお金</summary>
        public int soulMoney;
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
