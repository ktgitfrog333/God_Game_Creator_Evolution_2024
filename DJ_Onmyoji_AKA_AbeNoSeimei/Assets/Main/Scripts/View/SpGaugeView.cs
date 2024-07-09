using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// 蝋燭リソースの情報に合わせてUIを変化させる
    /// ビュー
    /// </summary>
    public class SpGaugeView : MonoBehaviour, ISpGaugeView
    {
        /// <summary>ゲージ画像</summary>
        [SerializeField] protected GaugeImage gaugeImage;
        /// <summary>ゲージ画像（火）</summary>
        [SerializeField] private GaugeImage gaugeImageFire;

        protected virtual void Reset()
        {
            gaugeImage = transform.GetChild(0).GetComponent<GaugeImage>();
            gaugeImageFire = transform.GetChild(1).GetComponent<GaugeImage>();
        }

        public bool SetAnchor(float timeSec, float limitTimeSecMax)
        {
            return gaugeImageFire.SetAnchor(timeSec, limitTimeSecMax);
        }

        public bool SetVertical(float timeSec, float limitTimeSecMax)
        {
            return gaugeImage.SetVertical(timeSec, limitTimeSecMax) &&
                gaugeImageFire.SetAnchor(timeSec, limitTimeSecMax);
        }

        public void SetFire(bool isRest)
        {
            if (isRest)
            {
                gaugeImage.ChangeColor(Color.gray);
                gaugeImageFire.ChangeColor(Color.black);
            }
            else
            {
                gaugeImage.ChangeColor(Color.white);
                gaugeImageFire.ChangeColor(Color.white);
            }
        }
    }

    /// <summary>
    /// 蝋燭リソースの情報に合わせてUIを変化させる
    /// ビュー
    /// インタフェース
    /// </summary>
    public interface ISpGaugeView
    {
        /// <summary>
        /// 高いをセットする
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetVertical(float timeSec, float limitTimeSecMax);
        /// <summary>
        /// 高いをセットする
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetAnchor(float timeSec, float limitTimeSecMax);
    }
}
