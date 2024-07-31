using Main.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Audio
{
    /// <summary>
    /// BGM設定の詳細
    /// </summary>
    public class BgmConfDetails
    {
        /// <summary>入力値</summary>
        private float inputValue = 0f;

        /// <summary>入力値</summary>
        public float InputValue
        {
            get { return inputValue; }
            set { inputValue = value; }
        }

        /// <summary>偽：時計回り、真：反時計回り</summary>
        private bool invert = false;

        /// <summary>ペンダグラムの回転状態</summary>
        public PentagramSpinState PentagramSpinState { get; set; }

        /// <summary>再生速度</summary>
        public float PbSpeed
        {
            get
            {
                if (inputValue == 0)
                    return 0f;
                else
                    // 0.1～0.4で丸めた値 * ピッチ差異埋め * 時計回り／反時計回り
                    return Mathf.Clamp(Mathf.Abs(inputValue), .1f, .4f) *
                        3f *
                        (0f < inputValue ? 1f : -1f);
            }
        }

        /// <summary>偽：時計回り、真：反時計回り</summary>
        public bool Invert
        {
            get { return invert; }
            set { invert = value; }
        }
    }
}
