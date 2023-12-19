using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// カウントダウンタイマーの情報に合わせてUIを変化させる
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ClearCountdownTimerCircleView : MonoBehaviour, IClearCountdownTimerCircleView
    {
        /// <summary>対象の画像</summary>
        [SerializeField] private Image image;
        /// <summary>ユーティリティ</summary>
        private MainViewUtility _utility = new MainViewUtility();

        private void Reset()
        {
            image = GetComponent<Image>();
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillOrigin = 2;
            image.fillClockwise = false;
        }

        public bool SetAngle(float timeSec, float limitTimeSecMax)
        {
            return _utility.SetFillAmountOfImage(image, timeSec, limitTimeSecMax);
        }
    }

    /// <summary>
    /// カウントダウンタイマーの情報に合わせてUIを変化させる
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IClearCountdownTimerCircleView
    {
        /// <summary>
        /// 角度をセットする
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetAngle(float timeSec, float limitTimeSecMax);
    }
}
