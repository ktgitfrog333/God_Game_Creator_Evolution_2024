using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// リワード情報を管理
    /// モデル
    /// </summary>
    public class RewardsModel : MonoBehaviour, IRewardsModel
    {
        /// <summary>クリア報酬のコンテンツのプロパティ</summary>
        [SerializeField] private RewardContentProp[] rewardContentProps;

        public RewardContentProp GetRewardContentProp(RewardID rewardID)
        {
            throw new System.NotImplementedException();
        }

        public RewardContentProp GetRewardContentProp(int index)
        {
            try
            {
                if (rewardContentProps.Length - 1 < index)
                    throw new System.ArgumentOutOfRangeException($"存在しないインデックスを指定:index[{index}]_length:[{rewardContentProps.Length}]");

                return rewardContentProps[index];
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return new RewardContentProp();
            }
        }
    }

    /// <summary>
    /// リワード情報を管理
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IRewardsModel
    {
        /// <summary>
        /// リワード情報を取得
        /// </summary>
        /// <param name="rewardID">リワードID</param>
        /// <returns>リワード情報</returns>
        public RewardContentProp GetRewardContentProp(RewardID rewardID);
        /// <summary>
        /// リワード情報を取得
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>リワード情報</returns>
        public RewardContentProp GetRewardContentProp(int index);
    }
}
