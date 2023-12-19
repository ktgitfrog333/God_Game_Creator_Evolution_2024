using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Utility
{
    /// <summary>
    /// Mainのユーティリティ
    /// ビュー
    /// </summary>
    public class MainViewUtility : IMainViewUtility
    {
        /// <summary>デフォルト表示</summary>
        private readonly float DEFAULT = 1f;

        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax)
        {
            try
            {
                image.fillAmount = limitTimeSecMax == 0f ? DEFAULT : (timeSec / limitTimeSecMax);
                
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
    /// Mainのユーティリティ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IMainViewUtility
    {
        /// <summary>
        /// ImageのFillAmountをセットする
        /// </summary>
        /// <param name="image">イメージ</param>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax);
    }
}
