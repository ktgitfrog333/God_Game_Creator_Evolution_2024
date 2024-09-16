using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Main.Common;
using UniRx;
using UnityEngine.UI;

namespace Main.Model
{
    /// <summary>
    /// クリア報酬選択
    /// モデル
    /// </summary>
    public class RewardSelectModel : MonoBehaviour, IRewardSelectModel
    {
        /// <summary>クリア報酬のコンテンツのモデル</summary>
        [SerializeField] private RewardContentModel[] rewardContentModels;
        /// <summary>クリア報酬のコンテンツのモデル</summary>
        public RewardContentModel[] RewardContentModels => rewardContentModels;
        /// <summary>リワード情報を管理モデル</summary>
        [SerializeField] private RewardsModel rewardsModel;
        /// <summary>実行済み</summary>
        private readonly BoolReactiveProperty _isCompleted = new BoolReactiveProperty();
        /// <summary>実行済み</summary>
        public IReactiveProperty<bool> IsCompleted => _isCompleted;
        /// <summary>（最後のみ使用）次の項目ボタン</summary>
        [SerializeField] private Button lastContent;
        /// <summary>クリア報酬のコンテンツのプロパティ</summary>
        public RewardContentProp[] RewardContentProps => rewardsModel.GetRewardContentProps();

        private void Reset()
        {
            rewardContentModels = GetComponentsInChildren<RewardContentModel>();
            rewardsModel = GetComponent<RewardsModel>();
            lastContent = GameObject.Find("GameSelectButton").GetComponent<Button>();
        }

        private void Start()
        {
            rewardsModel.IsLoadedData.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    // アクティブを条件にしているようだがあまり機能していない
                    var models = rewardContentModels.Where(q => q.isActiveAndEnabled)
                        .Select((p, i) => new { Content = p, Index = i });
                    foreach (var item in models)
                    {
                        // インデックス0の場合は前のボタンはnull
                        if (!item.Content.SetNavigation(0 < item.Index ? models.ToArray()[item.Index - 1].Content.Button : null,
                            // インデックスがmaxの場合は次のボタンは「確定」
                            item.Index < (models.ToArray().Length - 1) ? models.ToArray()[item.Index + 1].Content.Button : lastContent))
                            Debug.LogError("SetNavigationOfButton");
                        var rewardContentProp = rewardsModel.GetRewardContentProp(item.Index);
                        if (rewardContentProp != null)
                        {
                            if (!item.Content.SetRewardContentProp(rewardContentProp))
                                Debug.LogError("SetRewardContentProp");
                        }
                        else
                        {
                            // カードは少なくとも1枚は残る前提
                            // リワード情報が取得できなかったカードは非表示となるため、一つ前のカードに対してボタンナビゲーション設定を行う。
                            if (1 < item.Index)
                            {
                                if (!models.ToArray()[item.Index - 1].Content.SetNavigation(models.ToArray()[item.Index - 2].Content.Button,
                                    // インデックスがmaxの場合は次のボタンは「確定」
                                    lastContent))
                                    Debug.LogError("SetNavigation");
                                if (!lastContent.GetComponent<GameSelectButtonModel>().SetNavigation(models.ToArray()[item.Index - 1].Content.Button,
                                    null))
                                    Debug.LogError("SetNavigation");
                            }
                            else
                            {
                                if (!models.ToArray()[0].Content.SetNavigation(null,
                                    // インデックスがmaxの場合は次のボタンは「確定」
                                    lastContent))
                                    Debug.LogError("SetNavigationOf");
                                if (!lastContent.GetComponent<GameSelectButtonModel>().SetNavigation(models.ToArray()[0].Content.Button,
                                    null))
                                    Debug.LogError("SetNavigation");
                            }
                        }
                    }
                    _isCompleted.Value = true;
                });
        }

        public bool Check(int index)
        {
            return rewardContentModels[index].Check(CheckState.Check);
        }

        public bool UnCheck(int index)
        {
            return rewardContentModels[index].Check(CheckState.UnCheck);
        }

        public bool DiffCostVsResorceAndDisabled(ClearRewardContentsState clearRewardContentsState)
        {
            try
            {
                // 無効からチェックなしへ変更する
                foreach (var item in rewardContentModels.Select((p, i) => new { Content = p, Index = i })
                    .Where(q => q.Content.CheckState.Value == (int)CheckState.Disabled &&
                    q.Content.RewardContentProp != null &&
                    q.Content.RewardContentProp.soulMoney <= clearRewardContentsState.soulMoney))
                    if (!rewardContentModels[item.Index].Check(CheckState.UnCheck, true))
                        throw new System.Exception("Check");

                // チェックなしから無効へ変更する
                foreach (var item in rewardContentModels.Select((p, i) => new { Content = p, Index = i })
                    .Where(q => q.Content.CheckState.Value == (int)CheckState.UnCheck &&
                    q.Content.RewardContentProp != null &&
                    clearRewardContentsState.soulMoney < q.Content.RewardContentProp.soulMoney))
                    if (!rewardContentModels[item.Index].Disable())
                        throw new System.Exception("Disable");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// クリア報酬選択
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IRewardSelectModel
    {
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
        /// 現在のリソース情報とコストを比較
        /// 下回る場合は無効状態とする
        /// </summary>
        /// <param name="clearRewardContentsState">クリア報酬のコンテンツのプロパティ</>
        /// <returns>成功／失敗</returns>
        public bool DiffCostVsResorceAndDisabled(ClearRewardContentsState clearRewardContentsState);
    }
}
