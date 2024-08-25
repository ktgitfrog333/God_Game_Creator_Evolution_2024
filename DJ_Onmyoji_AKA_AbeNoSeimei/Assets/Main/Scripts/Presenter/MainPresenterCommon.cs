using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Model;
using Main.View;
using UniRx;
using Universal.Template;
using System.Linq;
using Universal.Bean;
using Universal.Common;

namespace Main.Common
{
    /// <summary>
    /// プレゼンタの共通処理
    /// </summary>
    public class MainPresenterCommon : IMainPresenterCommon
    {
        public bool IsFinalLevel(UserBean userBean)
        {
            try
            {
                var adminDataSingleton = AdminDataSingleton.Instance != null ?
                    AdminDataSingleton.Instance :
                    new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                        .GetComponent<AdminDataSingleton>();

                return adminDataSingleton.AdminBean.finalStages[userBean.sceneId - 1] == 1;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }

    /// <summary>
    /// プレゼンタの共通処理
    /// インターフェース
    /// </summary>
    public interface IMainPresenterCommon
    {
        /// <summary>
        /// 最終ステージである
        /// または、各エリアの最終ステージかつシナリオ未読である
        /// </summary>
        /// <param name="userBean">ユーザー情報を保持するクラス</param>
        /// <returns>成功／失敗</returns>
        public bool IsFinalLevel(UserBean userBean);
    }
}
