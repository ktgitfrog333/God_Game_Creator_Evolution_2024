using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Universal.Common;
using Main.Utility;

namespace Main.Model
{
    /// <summary>
    /// ペンダグラムシステム
    /// デバイスの入力情報を内部管理する
    /// モデル
    /// </summary>
    public class PentagramSystemModel : MonoBehaviour
    {
        /// <summary>入力角度</summary>
        public IReactiveProperty<float> InputValue { get; private set; } = new FloatReactiveProperty();
        /// <summary>距離の補正乗算値</summary>
        private float _multiDistanceCorrected = 7.5f;
        /// <summary>自動回転の速度</summary>
        [Tooltip("自動回転の速度")]
        [SerializeField] private float autoSpinSpeed = .01f;

        private void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            autoSpinSpeed = adminDataSingleton.AdminBean.PentagramSystemModel.autoSpinSpeed;
            Vector2ReactiveProperty previousInput = new Vector2ReactiveProperty(Vector2.zero); // 前回の入力を保存する変数
            var utility = new InputSystemUtility();
            if (!utility.SetInputValueInModel(InputValue, _multiDistanceCorrected, previousInput, autoSpinSpeed, this))
                Debug.LogError("SetInputValue");
        }
    }
}
