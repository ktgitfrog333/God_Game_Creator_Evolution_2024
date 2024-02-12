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
    [RequireComponent(typeof(Image))]
    public class SpGaugeView : MonoBehaviour, ISpGaugeView
    {
        /// <summary>対象の画像</summary>
        [SerializeField] protected Image image;
        /// <summary>ユーティリティ</summary>
        protected MainViewUtility _utility = new MainViewUtility();

        public bool SetVertical(float timeSec, float limitTimeSecMax)
        {
            return _utility.SetFillAmountOfImage(image, timeSec, limitTimeSecMax);
        }

        protected virtual void Reset()
        {
            image = GetComponent<Image>();
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Vertical;
            image.fillOrigin = 0;
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
    }
}
