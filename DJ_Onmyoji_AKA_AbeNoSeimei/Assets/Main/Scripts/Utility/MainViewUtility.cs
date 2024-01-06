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

        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null)
        {
            try
            {
                if (maskAngle < 0f || 1f < maskAngle)
                    throw new System.Exception("不正な値セット:0fから1fの値を設定して下さい");
                else if (0f < maskAngle && transform == null)
                    throw new System.Exception("不正な値セット:maskAngle設定時はtransformも設定が必要です");

                float baseAmount = limitTimeSecMax == 0f ? DEFAULT : (timeSec / limitTimeSecMax);
                float calc = System.Math.Max(0f, maskAngle);
                image.fillAmount = baseAmount * (1f - calc);
                if (transform != null)
                    transform.eulerAngles = new Vector3(0f, 0f, 360f) * (calc * .5f);
                
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
        /// <param name="maskAngle">マスクする角度の割合（0f~1f）</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null);
    }
}
