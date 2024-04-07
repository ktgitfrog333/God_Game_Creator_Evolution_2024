using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Utility;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// ゲージ画像
    /// </summary>
    public class GaugeImage : MonoBehaviour, ISpGaugeView, IGaugeImage
    {
        /// <summary>対象の画像</summary>
        [SerializeField] private Image image;
        /// <summary>ユーティリティ</summary>
        private MainViewUtility _utility = new MainViewUtility();
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>Rectトランスフォーム</summary>
        public RectTransform RectTransform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = transform;
                }
                return _transform as RectTransform;
            }
            private set { }
        }
        /// <summary>アンカー位置（最大）</summary>
        [SerializeField] private Vector2 anchorPosMax = new Vector2(48f, 0f);
        /// <summary>アンカー位置（最小）</summary>
        [SerializeField] private Vector2 anchorPosMin = new Vector2(48f, -860f);

        public bool SetVertical(float timeSec, float limitTimeSecMax)
        {
            return _utility.SetFillAmountOfImage(image, timeSec, limitTimeSecMax);
        }

        public bool SetAnchor(float timeSec, float limitTimeSecMax)
        {
            return _utility.SetAnchorOfImage(RectTransform, timeSec, limitTimeSecMax, anchorPosMin, anchorPosMax);
        }

        public bool SetSliderValue(float tempoLevel, float limitTimeSecMax)
        {
            return _utility.SetAnchorOfImage(RectTransform, tempoLevel, limitTimeSecMax, anchorPosMin, anchorPosMax);
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
    /// ゲージ画像
    /// インターフェース
    /// </summary>
    public interface IGaugeImage
    {
        /// <summary>
        /// スライダーへ値をセット
        /// </summary>
        /// <param name="tempoLevel">テンポレベル</param>
        /// <param name="limitTimeSecMax">神タイプ</param>
        /// <returns>成功／失敗</returns>
        public bool SetSliderValue(float tempoLevel, float limitTimeSecMax);
    }
}
