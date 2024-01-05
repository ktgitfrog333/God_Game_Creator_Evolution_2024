using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.InputSystem;
using Main.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Universal.Common;

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
        /// <summary>最小値</summary>
        public const float MIN = -1f;
        /// <summary>最大値</summary>
        public const float MAX = 1f;
        /// <summary>陰陽（昼夜）の状態デフォルト</summary>
        [SerializeField, Range(MIN, MAX)] private float defaultOnmyoStateValue = 1f;
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
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            durations[0] = adminDataSingleton.AdminBean.sunMoonSystemModel.durations[0];
            var utility = new InputSystemUtility();
            OnmyoState.Value = defaultOnmyoStateValue;
            if (!utility.SetOnmyoStateInModel(OnmyoState, durations, this))
                Debug.LogError("SetOnmyoState");
        }
    }
}
