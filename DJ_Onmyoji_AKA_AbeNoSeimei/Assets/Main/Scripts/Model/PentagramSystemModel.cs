using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Utility;

namespace Main.Model
{
    /// <summary>
    /// ペンダグラムシステム
    /// デバイスの入力情報を内部管理する
    /// モデル
    /// </summary>
    public class PentagramSystemModel : MonoBehaviour, IPentagramSystemModel
    {
        /// <summary>自動回転の速度</summary>
        [Tooltip("自動回転の速度")]
        [SerializeField] private static float autoSpinSpeed = .01f;
        /// <summary>入力角度</summary>
        public IReactiveProperty<float> InputValue { get; private set; } = new FloatReactiveProperty(autoSpinSpeed);
        /// <summary>距離の補正乗算値</summary>
        private float _multiDistanceCorrected = 7.5f;
        /// <summary>ジョッキーコマンドタイプ</summary>
        /// TODO:コマンドを可変させるロジックを実装
        public IReactiveProperty<int> JockeyCommandType { get; private set; } = new IntReactiveProperty((int)Common.JockeyCommandType.None);
        /// <summary>コマンドの最大入力数</summary>
        [SerializeField] private int inputHistoriesLimit = 100;
        /// <summary>InputSystemのユーティリティ</summary>
        private InputSystemUtility _inputSystemUtility = new InputSystemUtility();
        /// <summary>ジョッキーコマンドのユーティリティクラス</summary>
        private JockeyCommandUtility _jockeyCommandUtility = new JockeyCommandUtility();
        /// <summary>バックスピンの入力情報</summary>
        [SerializeField] private InputBackSpinState inputBackSpinState = new InputBackSpinState()
        {
            recordInputTimeSecLimit = .5f,
            targetAngle = 720f,
        };

        private void Start()
        {
            var utilityCommon = new MainCommonUtility();
            var adminDataSingleton = utilityCommon.AdminDataSingleton;
            autoSpinSpeed = adminDataSingleton.AdminBean.pentagramSystemModel.autoSpinSpeed;
            inputHistoriesLimit = adminDataSingleton.AdminBean.pentagramSystemModel.inputHistoriesLimit;
            Vector2ReactiveProperty previousInput = new Vector2ReactiveProperty(Vector2.zero); // 前回の入力を保存する変数
            if (!_inputSystemUtility.SetInputValueInModel(InputValue, _multiDistanceCorrected, previousInput, autoSpinSpeed, this))
                Debug.LogError("SetInputValueInModel");
            inputBackSpinState.inputVelocityValue = new Vector2ReactiveProperty();
            inputBackSpinState.recordInputTimeSec = new FloatReactiveProperty();
            if (!_inputSystemUtility.SetInputValueInModel(inputBackSpinState, this))
                Debug.LogError("SetInputValueInModel");
            if (!_jockeyCommandUtility.UpdateJockeyCommandType(InputValue, inputBackSpinState, JockeyCommandType, autoSpinSpeed, inputHistoriesLimit))
                Debug.LogError("UpdateJockeyCommandType");
        }

        public bool ResetJockeyCommandType()
        {
            return _jockeyCommandUtility.SetNone(JockeyCommandType);
        }
    }

    /// <summary>
    /// ペンダグラムシステム
    /// デバイスの入力情報を内部管理する
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IPentagramSystemModel
    {
        /// <summary>
        /// ジョッキーコマンドタイプをリセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ResetJockeyCommandType();
    }

    /// <summary>
    /// バックスピンの入力情報
    /// </summary>
    [System.Serializable]
    public struct InputBackSpinState
    {
        /// <summary>入力座標</summary>
        public IReactiveProperty<Vector2> inputVelocityValue;
        /// <summary>入力保持時間（秒）</summary>
        public IReactiveProperty<float> recordInputTimeSec;
        /// <summary>入力保持最大時間（秒）</summary>
        public float recordInputTimeSecLimit;
        /// <summary>入力検知の角度</summary>
        public float targetAngle;
    }
}
