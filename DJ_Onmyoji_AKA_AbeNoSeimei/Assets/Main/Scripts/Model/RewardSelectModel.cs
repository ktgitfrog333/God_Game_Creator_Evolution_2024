using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// クリア報酬選択
    /// モデル
    /// </summary>
    public class RewardSelectModel : MonoBehaviour
    {
        /// <summary>クリア報酬のコンテンツのモデル</summary>
        [SerializeField] private RewardContentModel[] rewardContentModels;
        /// <summary>クリア報酬のコンテンツのモデル</summary>
        public RewardContentModel[] RewardContentModels => rewardContentModels;
        /// <summary>リワード情報を管理モデル</summary>
        [SerializeField] private RewardsModel rewardsModel;

        private void Reset()
        {
            rewardContentModels = GetComponentsInChildren<RewardContentModel>();
            rewardsModel = GetComponent<RewardsModel>();
        }

        private void Start()
        {
            var models = rewardContentModels.Select((p, i) => new { Content = p, Index = i })
                .Where(q => q.Content.isActiveAndEnabled);
            foreach (var item in models)
            {
                // インデックス0の場合は前のボタンはnull
                if (!item.Content.SetNavigationOfButton(0 < item.Index ? models.ToArray()[item.Index - 1].Content.Button : null,
                    // インデックスがmaxの場合は次のボタンはnull
                    item.Index < (models.ToArray().Length - 1) ? models.ToArray()[item.Index + 1].Content.Button : null))
                    Debug.LogError("SetNavigationOfButton");
                if (!item.Content.SetRewardContentProp(rewardsModel.GetRewardContentProp(item.Index)))
                    Debug.LogError("SetRewardContentProp");
            }
        }
    }
}
