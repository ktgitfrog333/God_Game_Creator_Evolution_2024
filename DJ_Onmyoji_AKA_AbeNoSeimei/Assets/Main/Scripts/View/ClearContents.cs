using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// コンテンツ
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ClearContents : MonoBehaviour
    {
        /// <summary>テキスト</summary>
        [SerializeField] protected Text text;
        /// <summary>Mainのユーティリティ</summary>
        protected MainViewUtility _mainViewUtility = new MainViewUtility();
        /// <summary>獲得経験値に関するテキストのデフォルトフォーマット</summary>
        protected const string DEFAULT_FORMAT_SOUL_MONEY = "0";

        protected virtual void Reset()
        {
            text = GetComponent<Text>();
        }
    }

    /// <summary>
    /// コンテンツ
    /// インターフェース
    /// </summary>
    public interface IClearContents
    {
        /// <summary>
        /// 魂のお金をセット
        /// </summary>
        /// <param name="soulMoney">魂のお金</param>
        /// <returns>成功／失敗</returns>
        public bool SetSoulMoney(int soulMoney);
    }
}
