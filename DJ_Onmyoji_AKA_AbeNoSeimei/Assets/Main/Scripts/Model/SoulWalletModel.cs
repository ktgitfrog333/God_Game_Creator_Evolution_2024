using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Main.Utility;

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

        private void Start()
        {
            var utility = new MainCommonUtility();
            var userDataSingleton = utility.UserDataSingleton;
            SoulMoney.Value = userDataSingleton.UserBean.soulMoney;
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
