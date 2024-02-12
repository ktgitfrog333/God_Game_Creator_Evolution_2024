using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 陰陽（昼夜）の切り替え
    /// モデル
    /// </summary>
    public class SunMoonSystemModel : MonoBehaviour
    {
        /// <summary>陰陽（昼夜）の状態</summary>
        public IReactiveProperty<float> OnmyoState { get; private set; } = new FloatReactiveProperty();
        /// <summary>陰陽（昼夜）の状態デフォルト</summary>
        [SerializeField, Range(InputSystemUtility.MIN, InputSystemUtility.MAX)] private float defaultOnmyoStateValue = 1f;
        /// <summary>
        /// ボタン押下の時間管理
        /// [0]長押し
        /// </summary>
        [SerializeField] private float[] durations =
        {
            1.75f,
        };

        private void Start()
        {
            var commonUtility = new MainCommonUtility();
            durations[0] = commonUtility.AdminDataSingleton.AdminBean.sunMoonSystemModel.durations[0];
            var utility = new InputSystemUtility();
            OnmyoState.Value = defaultOnmyoStateValue;
            if (!utility.SetOnmyoStateInModel(OnmyoState, durations, this))
                Debug.LogError("SetOnmyoState");
        }
    }
}
