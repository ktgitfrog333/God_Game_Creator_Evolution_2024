using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Utility;
using TMPro;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// クリア報酬のコンテンツ
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ClearRewardTMPContents : MonoBehaviour, IClearRewardTMPContents
    {
        /// <summary>テキスト</summary>
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        /// <summary>変更前の説明内容に関するクリア報酬プロパティ</summary>
        [SerializeField] private ClearRewardProp[] clearRewardPropsOfAfter;
        /// <summary>名前に関するテキストのデフォルトフォーマット</summary>
        private const string DEFAULT_FORMAT_NAME = "ー";
        /// <summary>Mainのユーティリティ</summary>
        private MainViewUtility _mainViewUtility = new MainViewUtility();
        /// <summary>獲得経験値に関するテキストのデフォルトフォーマット</summary>
        protected const string DEFAULT_FORMAT_SOUL_MONEY = "0";
        /// <summary>値の更新をロック</summary>
        [SerializeField] private bool isLockValuePpdates;

        public bool SetPropetiesAfterOfDescription(ClearRewardType clearRewardType)
        {
            try
            {
                switch (clearRewardType)
                {
                    case ClearRewardType.AddShikigami:
                        // 式神追加の場合は差分表示が不要なた非表示にする
                        if (textMeshProUGUI.enabled)
                            textMeshProUGUI.enabled = false;

                        break;
                    default:
                        // 比較表示の場合は表示する
                        if (!textMeshProUGUI.enabled)
                            textMeshProUGUI.enabled = true;
                        var message = clearRewardPropsOfAfter.Where(q => q.clearRewardType.Equals(clearRewardType))
                            .Select(q => q.message)
                            .ToArray();
                        if (message.Length < 1)
                            throw new System.ArgumentNullException($"説明に関するクリア報酬プロパティに該当する条件無し:[{clearRewardType}]");

                        if (!isLockValuePpdates)
                            if (!_mainViewUtility.SetNameOfText(textMeshProUGUI, message[0], DEFAULT_FORMAT_NAME))
                                throw new System.Exception("SetNameOfText");

                        break;
                }

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
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            clearRewardPropsOfAfter = new ClearRewardProp[]
            {
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.AddShikigami,
                    message = "",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhanceShikigami,
                    message = "タイプ：ラップ\n攻撃　：Ａ\n連射　：Ｂ\n持続　：<color=#ff0000>Ａ</color>\nサブＡ：追尾Ａ\nサブＢ：ー\nサブＣ：ー",
                },
                new ClearRewardProp()
                {
                    clearRewardType = ClearRewardType.EnhancePlayer,
                    message = "最大体力　　　：<color=#ff0000>150</color>\n陰陽玉Ａ攻撃　：Ａ\n陰陽玉Ａ連射　：Ｂ\n陰陽玉Ａ持続　：Ｃ\n陰陽玉Ｂ攻撃　：Ａ\n陰陽玉Ｂ連射　：Ｂ\n陰陽玉Ｂ持続　：Ｃ\n",
                },
            };
        }

        public bool SetSoulMoney(int soulMoney)
        {
            return _mainViewUtility.SetSoulMoneyOfText(textMeshProUGUI, soulMoney, DEFAULT_FORMAT_SOUL_MONEY);
        }
    }

    /// <summary>
    /// クリア報酬のコンテンツ
    /// インターフェース
    /// </summary>
    public interface IClearRewardTMPContents : IClearContents
    {
        /// <summary>
        /// 説明内の強化前の説明をセット
        /// </summary>
        /// <param name="clearRewardType">タイトル用報酬タイプ種別</param>
        /// <returns>成功／失敗</returns>
        public bool SetPropetiesAfterOfDescription(ClearRewardType clearRewardType);
    }
}
