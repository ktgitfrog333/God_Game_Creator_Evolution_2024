using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Main.Utility;
using Universal.Template;
using Universal.Common;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// 魂の財布、獲得したソウルの管理
    /// モデル
    /// </summary>
    public class SoulWalletModel : MonoBehaviour, ISoulWalletModel
    {
        /// <summary>魂のお金</summary>
        public IReactiveProperty<int> SoulMoney { get; private set; } = new IntReactiveProperty();
        /// <summary>プレイヤーの死亡フラグ</summary>
        private BoolReactiveProperty _isDeadOfPlayer;
        /// <summary>経験値を更新をロック</summary>
        public bool IsUnLockUpdateOfSoulMoney { get; private set; }

        private void Start()
        {
            var utility = new MainCommonUtility();
            var userDataSingleton = utility.UserDataSingleton;
            SoulMoney.Value = userDataSingleton.UserBean.soulMoney;
            this.UpdateAsObservable()
                .Where(_ => GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_PLAYER) != null)
                .Select(_ => GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_PLAYER).GetComponent<PlayerModel>())
                .Where(model => model != null)
                .Take(1)
                .Subscribe(model => _isDeadOfPlayer = (BoolReactiveProperty)model.State.IsDead);
        }

        private void OnDestroy()
        {
            var temp = new TemplateResourcesAccessory();
            var userBean = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            userBean.soulMoney = _isDeadOfPlayer != null && !_isDeadOfPlayer.Value ? SoulMoney.Value : 0;
            var userBeanUpd = userBean;
            if (!temp.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, userBeanUpd))
                throw new System.Exception("SaveDatasJsonOfUserBean");
        }

        public int AddSoulMoney(int soulMoney)
        {
            try
            {
                if (IsUnLockUpdateOfSoulMoney)
                    return SoulMoney.Value += soulMoney;
                else
                    // ロック中は経験値を更新しない
                    return SoulMoney.Value;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
            }
        }

        public bool SetIsUnLockUpdateOfSoulMoney(bool IsUnLock)
        {
            try
            {
                IsUnLockUpdateOfSoulMoney = IsUnLock;

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
    /// 魂の財布
    /// モデル
    /// インターフェース
    /// </summary>
    public interface ISoulWalletModel
    {
        /// <summary>
        /// ソウルを加算
        /// </summary>
        /// <param name="soulMoney">獲得ソウル</param>
        /// <returns>取得後の総数</returns>
        public int AddSoulMoney(int soulMoney);
        /// <summary>
        /// 更新アンロック状態をセット
        /// </summary>
        /// <param name="IsUnLock">アンロック状態／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsUnLockUpdateOfSoulMoney(bool IsUnLock);
    }
}
