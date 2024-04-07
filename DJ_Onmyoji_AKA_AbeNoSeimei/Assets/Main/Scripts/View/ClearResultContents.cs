using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// クリア結果のコンテンツ
    /// </summary>
    public class ClearResultContents : ClearContents, IClearResultContents
    {
        /// <summary>タイマーに関するテキストのデフォルトフォーマット</summary>
        private const string DEFAULT_FORMAT_TIME_SEC = "00：00";

        public bool SetTimeSec(float timeSec)
        {
            try
            {
                var minutes = Mathf.FloorToInt(timeSec / 60);
                var seconds = Mathf.FloorToInt(timeSec % 60);
                var timeFormat = string.Format("{0:00}：{1:00}", minutes, seconds);
                text.text = timeFormat;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                text.text = DEFAULT_FORMAT_TIME_SEC;

                return false;
            }
        }

        public bool SetSoulMoney(int soulMoney)
        {
            return _mainViewUtility.SetSoulMoneyOfText(text, soulMoney, DEFAULT_FORMAT_SOUL_MONEY);
        }
    }

    /// <summary>
    /// クリア結果のコンテンツ
    /// インターフェース
    /// </summary>
    public interface IClearResultContents : IClearContents
    {
        /// <summary>
        /// タイマーをセット
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <returns>成功／失敗</returns>
        public bool SetTimeSec(float timeSec);
    }

    /// <summary>
    /// クリア結果のコンテンツ
    /// プロパティ
    /// </summary>
    public struct ClearResultContentsState
    {
        /// <summary>タイマー</summary>
        public float timeSec;
        /// <summary>魂のお金</summary>
        public int soulMoney;
    }
}
