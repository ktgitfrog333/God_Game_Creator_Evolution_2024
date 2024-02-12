using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// フェーダー
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class FaderUniversalView : MonoBehaviour, IFaderUniversalView
    {
        /// <summary>スライダー</summary>
        [SerializeField] private Slider slider;
        /// <summary>式神タイプ</summary>
        [SerializeField] private ShikigamiType shikigamiType;

        public bool SetSliderValue(float tempoLevel, ShikigamiType shikigamiType)
        {
            try
            {
                if(!shikigamiType.Equals(this.shikigamiType))
                    return true;

                slider.value = tempoLevel;

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
            slider = GetComponent<Slider>();
            slider.interactable = false;
            slider.direction = Slider.Direction.BottomToTop;
            slider.minValue = -1f;
            slider.maxValue = 1f;
            slider.value = 0f;
            if (name.Equals($"Fader{ShikigamiType.Wrap}") &&
            !name.Equals($"Fader{ShikigamiType.Dance}") &&
            !name.Equals($"Fader{ShikigamiType.Graffiti}"))
                shikigamiType = ShikigamiType.Wrap;
            else if (name.Equals($"Fader{ShikigamiType.Dance}"))
                shikigamiType = ShikigamiType.Dance;
            else if (name.Equals($"Fader{ShikigamiType.Graffiti}"))
                shikigamiType = ShikigamiType.Graffiti;
            else
            {
                // 陰陽玉は処理無し
            }
        }
    }

    /// <summary>
    /// フェーダー
    /// ビュー
    /// インタフェース
    /// </summary>
    public interface IFaderUniversalView
    {
        /// <summary>
        /// スライダーへ値をセット
        /// </summary>
        /// <param name="tempoLevel">テンポレベル</param>
        /// <param name="shikigamiType">式神タイプ</param>
        /// <returns>成功／失敗</returns>
        public bool SetSliderValue(float tempoLevel, ShikigamiType shikigamiType);
    }
}
