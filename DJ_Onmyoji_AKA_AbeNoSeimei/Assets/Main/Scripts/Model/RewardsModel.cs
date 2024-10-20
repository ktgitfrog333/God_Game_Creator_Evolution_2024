using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UniRx;
using UniRx.Triggers;
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
        /// <summary>データロード完了</summary>
        public IReactiveProperty<bool> IsLoadedData { private set; get; } = new BoolReactiveProperty();

        private void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => MainGameManager.Instance)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    rewardContentProps = x.LevelOwner.GetRewardContentProps();
                    if (rewardContentProps == null)
                        Debug.LogError("GetRewardContentProps");
                    IsLoadedData.Value = true;
                });
        }

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
            catch (System.ArgumentOutOfRangeException aoe)
            {
                Debug.LogWarning(aoe);
                return null;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public RewardContentProp[] GetRewardContentProps()
        {
            return rewardContentProps;
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
        /// <summary>
        /// リワード情報を全て取得
        /// </summary>
        /// <returns>リワード情報（全て）</returns>
        public RewardContentProp[] GetRewardContentProps();
    }
}
