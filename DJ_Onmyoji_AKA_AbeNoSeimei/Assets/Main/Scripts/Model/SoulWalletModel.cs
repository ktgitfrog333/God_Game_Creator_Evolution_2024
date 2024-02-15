using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Main.Utility;
using Universal.Template;
using Universal.Common;

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

        private void Start()
        {
            var utility = new MainCommonUtility();
            var userDataSingleton = utility.UserDataSingleton;
            SoulMoney.Value = userDataSingleton.UserBean.soulMoney;
            this.UpdateAsObservable()
                .Select(_ => GameObject.Find("Player").GetComponent<PlayerModel>())
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
                return SoulMoney.Value += soulMoney;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return -1;
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
    }
}
