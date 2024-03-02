using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// クリア報酬のコンテンツ
    /// プロパティ
    /// </summary>
    [System.Serializable]
    public class RewardContentProp
    {
        /// <summary>リワードID</summary>
        public RewardID rewardID;
        /// <summary>リワードタイプ</summary>
        public ClearRewardType rewardType;
        /// <summary>イメージ</summary>
        public Sprite image;
        /// <summary>名前</summary>
        public string name;
        /// <summary>魂のお金</summary>
        public int soulMoney;
    }

    /// <summary>
    /// リワードID
    /// </summary>
    public enum RewardID
    {
        RE0000,
        RE0001,
        RE0002,
        RE0003,
        RE0004,
    }

    /// <summary>
    /// タイトル用クリア報酬タイプ種別
    /// </summary>
    public enum ClearRewardType
    {
        /// <summary>式神の追加</summary>
        AddShikigami,
        /// <summary>式神の強化</summary>
        EnhanceShikigami,
        /// <summary>プレイヤーの強化</summary>
        EnhancePlayer,
    }

    // /// <summary>
    // /// リワードタイプ
    // /// </summary>
    // public enum RewardType
    // {
    //     /// <summary>召喚</summary>
    //     Add,
    //     /// <summary>強化</summary>
    //     Enhance,
    // }
}
