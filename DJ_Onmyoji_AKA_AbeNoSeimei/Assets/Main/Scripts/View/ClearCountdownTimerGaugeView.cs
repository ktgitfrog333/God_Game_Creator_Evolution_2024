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
    public class ClearCountdownTimerGaugeView : MonoBehaviour, IClearCountdownTimerGaugeView
    {
        /// <summary>対象の画像</summary>
        [SerializeField] private Image image;
        /// <summary>ユーティリティ</summary>
        private MainViewUtility _utility = new MainViewUtility();

        public bool SetHorizontal(float timeSec, float limitTimeSecMax)
        {
            return _utility.SetFillAmountOfImage(image, timeSec, limitTimeSecMax);
        }

        private void Reset()
        {
            image = GetComponent<Image>();
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = 0;
        }
    }

    /// <summary>
    /// カウントダウンタイマーの情報に合わせてUIを変化させる
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IClearCountdownTimerGaugeView
    {
        /// <summary>
        /// 横幅をセットする
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetHorizontal(float timeSec, float limitTimeSecMax);
    }
}
