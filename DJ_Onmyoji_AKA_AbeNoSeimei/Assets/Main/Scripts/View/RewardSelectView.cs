using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// クリア報酬選択
    /// ビュー
    /// </summary>
    public class RewardSelectView : MonoBehaviour, IRewardSelectView
    {
        /// <summary>クリア報酬のコンテンツ</summary>
        [SerializeField] private ClearRewardTextContents resourcesContents;
        /// <summary>クリア報酬のコンテンツ</summary>
        [SerializeField] private RewardContent[] rewardContents;
        /// <summary>説明</summary>
        [SerializeField] private Description description;

        public bool Check(int index)
        {
            return rewardContents[index].Check(true);
        }

        public bool Disabled(int index)
        {
            return rewardContents[index].Disabled();
        }

        public bool ScaleDown(int index)
        {
            return rewardContents[index].PlayScalingAnimation(false);
        }

        public bool ScaleUp(int index)
        {
            return rewardContents[index].PlayScalingAnimation(true);
        }

        public bool SetContents(ClearRewardContentsState clearRewardContentsState)
        {
            return resourcesContents.SetSoulMoney(clearRewardContentsState.soulMoney);
        }

        public bool SetContents(RewardContentProp[] rewardContentProps)
        {
            try
            {
                if (rewardContentProps == null || rewardContentProps.Length < 4)
                    throw new System.ArgumentNullException("クリア報酬は3つ以上セットする必要があります");
                if (rewardContents.Length < rewardContentProps.Length)
                    throw new System.ArgumentOutOfRangeException($"表示コンテンツ超過 rewardContents:[{rewardContents.Length}] rewardIDs:[{rewardContentProps.Length}]");

                foreach (var item in rewardContentProps.Select((p, i) => new { Content = p, Index = i }))
                    if (!rewardContents[item.Index].SetContents(item.Content))
                        throw new System.Exception("SetContents");
                // 渡されたリワードID数が表示枠より小さい場合
                if (rewardContentProps.Length < rewardContents.Length)
                    foreach (var item in rewardContents.Select((p, i) => new { Content = p, Index = i })
                        .Where(q => (rewardContentProps.Length - 1) < q.Index))
                        // リワード情報がセットされないuGUIオブジェクトは無効化して非表示
                        item.Content.gameObject.SetActive(false);
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetContents(RewardContentProp rewardContentProp)
        {
            return description.SetDescription(rewardContentProp);
        }

        public bool UnCheck(int index)
        {
            return rewardContents[index].Check(false);
        }

        public bool UpdateCheckState(ClearRewardContentsState clearRewardContentsState, RewardContentProp[] rewardContentProps)
        {
            throw new System.NotImplementedException();
        }

        private void Reset()
        {
            resourcesContents = GameObject.Find("Resources").GetComponentInChildren<ClearRewardTextContents>();
            rewardContents = GetComponentsInChildren<RewardContent>();
            description = GetComponentInChildren<Description>();
        }
    }

    /// <summary>
    /// クリア報酬選択
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IRewardSelectView
    {
        /// <summary>
        /// コンテンツをセット
        /// </summary>
        /// <param name="clearRewardContentsState">クリア報酬のコンテンツのステート</param>
        /// <returns>成功／失敗</returns>
        public bool SetContents(ClearRewardContentsState clearRewardContentsState);
        /// <summary>
        /// コンテンツをセット
        /// </summary>
        /// <param name="rewardContentProps">リワード情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetContents(RewardContentProp[] rewardContentProps);
        /// <summary>
        /// コンテンツをセット
        /// </summary>
        /// <param name="rewardContentProp">リワード情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetContents(RewardContentProp rewardContentProp);
        /// <summary>
        /// 大きくする
        /// </summary>
        /// <param name="index">対象のインデックス</>
        /// <returns>成功／失敗</returns>
        public bool ScaleUp(int index);
        /// <summary>
        /// 小さくする
        /// </summary>
        /// <param name="index">対象のインデックス</>
        /// <returns>成功／失敗</returns>
        public bool ScaleDown(int index);
        /// <summary>
        /// チェックする
        /// </summary>
        /// <param name="index">対象のインデックス</>
        /// <returns>成功／失敗</returns>
        public bool Check(int index);
        /// <summary>
        /// チェックを外す
        /// </summary>
        /// <param name="index">対象のインデックス</>
        /// <returns>成功／失敗</returns>
        public bool UnCheck(int index);
        /// <summary>
        /// チェック無効
        /// </summary>
        /// <param name="index">対象のインデックス</>
        /// <returns>成功／失敗</returns>
        public bool Disabled(int index);
        /// <summary>
        /// 魂のお金のリソースを元にチェック状態を更新
        /// </summary>
        /// <param name="clearRewardContentsState">クリア報酬のコンテンツのステート</param>
        /// <param name="rewardContentProps">リワード情報</param>
        /// <returns>成功／失敗</returns>
        public bool UpdateCheckState(ClearRewardContentsState clearRewardContentsState, RewardContentProp[] rewardContentProps);
    }
}
