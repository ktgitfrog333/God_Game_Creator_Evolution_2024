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
        /// <summary>式神タイプ</summary>
        public ShikigamiType shikigamiType;
        /// <summary>リワードタイプ</summary>
        public ClearRewardType rewardType;
        /// <summary>イメージ</summary>
        public Sprite image;
        /// <summary>名前</summary>
        public string name;
        /// <summary>魂のお金</summary>
        public int soulMoney;
        /// <summary>クリア報酬のコンテンツ詳細のプロパティ</summary>
        public RewardContentDetailProp detailProp;
    }

    /// <summary>
    /// クリア報酬のコンテンツ詳細
    /// プロパティ
    /// </summary>
    [System.Serializable]
    public class RewardContentDetailProp
    {
        /// <summary>パラメータ情報（変更前）</summary>
        public ShikigamiInfo.Prop beforeShikigamiInfoProp;
        /// <summary>パラメータ情報（変更後）</summary>
        public ShikigamiInfo.Prop afterShikigamiInfoProp;
        /// <summary>プレイヤー情報</summary>
        public PlayerInfo playerInfoProp;

        /// <summary>
        /// プレイヤー情報
        /// </summary>
        [System.Serializable]
        public class PlayerInfo
        {
            /// <summary>パラメータ情報（変更前）</summary>
            public ShikigamiInfo.Prop[] beforePlayerInfoProps;
            /// <summary>パラメータ情報（変更後）</summary>
            public ShikigamiInfo.Prop[] afterPlayerInfoProps;
        }
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

    /// <summary>
    /// チェック状態
    /// </summary>
    public enum CheckState
    {
        /// <summary>チェックなし</summary>
        UnCheck,
        /// <summary>チェックあり</summary>
        Check,
        /// <summary>不可</summary>
        Disabled,
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
}
