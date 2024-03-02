using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 説明に関するビュー
    /// </summary>
    public class Description : MonoBehaviour, IDescription
    {
        /// <summary>クリア報酬のコンテンツ</summary>
        [SerializeField] private ClearRewardTextContents[] clearRewardTextContents;
        /// <summary>クリア報酬のコンテンツ</summary>
        [SerializeField] private ClearRewardTMPContents[] clearRewardTMPContents;

        public bool SetDescription(ClearRewardType clearRewardType)
        {
            try
            {
                if (!clearRewardTextContents[0].SetTitleOfDescription(clearRewardType))
                    throw new System.Exception("SetTitleOfDescription");
                if (!clearRewardTextContents[1].SetPropetiesBeforeOfDescription(clearRewardType))
                    throw new System.Exception("SetPropetiesBeforeOfDescription");
                if (!clearRewardTMPContents[0].SetPropetiesAfterOfDescription(clearRewardType))
                    throw new System.Exception("SetPropetiesAfterOfDescription");
                if (!clearRewardTMPContents[1].SetPropetiesAfterOfDescription(clearRewardType))
                    throw new System.Exception("SetPropetiesAfterOfDescription");
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            clearRewardTextContents = GetComponentsInChildren<ClearRewardTextContents>();
            clearRewardTMPContents = GetComponentsInChildren<ClearRewardTMPContents>();
        }
    }

    public interface IDescription
    {
        /// <summary>
        /// 説明をセット
        /// </summary>
        /// <param name="clearRewardType">タイトル用クリア報酬タイプ種別</param>
        /// <returns>成功／失敗</returns>
        public bool SetDescription(ClearRewardType clearRewardType);
    }
}
