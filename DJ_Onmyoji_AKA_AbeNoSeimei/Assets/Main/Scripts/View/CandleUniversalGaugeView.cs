using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 式神レベルの情報に合わせてUIを変化させる
    /// ビュー
    /// </summary>
    public class CandleUniversalGaugeView : SpGaugeView, IFaderUniversalView
    {
        /// <summary>式神タイプ</summary>
        [SerializeField] private ShikigamiType shikigamiType;
        /// <summary>補正値</summary>
        private const float OFFSET = 1f;

        private void Start()
        {
            var transform = this.transform;
            this.UpdateAsObservable()
                .Subscribe(_ => transform.eulerAngles = Vector3.zero);
        }

        public bool SetSliderValue(float tempoLevel, ShikigamiType shikigamiType)
        {
            try
            {
                if(!shikigamiType.Equals(this.shikigamiType))
                    return true;

                // amountオプションは0から1だが渡される値は-1fから1fの間になる
                // x + 1して0fから2fの間と変換する
                return _utility.SetFillAmountOfImage(image, tempoLevel + OFFSET, 1f + OFFSET);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        protected override void Reset()
        {
            base.Reset();
            if (name.Equals($"Candle{ShikigamiType.Wrap}Gauge") &&
            !name.Equals($"Candle{ShikigamiType.Dance}Gauge") &&
            !name.Equals($"Candle{ShikigamiType.Graffiti}Gauge"))
                shikigamiType = ShikigamiType.Wrap;
            else if (name.Equals($"Candle{ShikigamiType.Dance}Gauge"))
                shikigamiType = ShikigamiType.Dance;
            else if (name.Equals($"Candle{ShikigamiType.Graffiti}Gauge"))
                shikigamiType = ShikigamiType.Graffiti;
            else
            {
                // 陰陽玉は処理無し
            }
        }
    }
}
