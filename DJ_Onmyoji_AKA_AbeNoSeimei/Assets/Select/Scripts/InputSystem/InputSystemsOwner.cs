using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Universal.Template;
using Universal.Common;
using Select.Utility;

namespace Select.InputSystem
{
    /// <summary>
    /// InputSystemのオーナー
    /// </summary>
    public class InputSystemsOwner : MonoBehaviour, ISelectGameManager
    {
        /// <summary>インプットアクション</summary>
        private FutureContents3D_Main _inputActions;
        /// <summary>インプットアクション</summary>
        public FutureContents3D_Main InputActions => _inputActions;
        /// <summary>監視管理</summary>
        private CompositeDisposable _compositeDisposable;
        /// <summary>現在の入力モード（コントローラー／キーボード）</summary>
        private IntReactiveProperty _currentInputMode;
        /// <summary>現在の入力モード（コントローラー／キーボード）</summary>
        public IntReactiveProperty CurrentInputMode => _currentInputMode;
        /// <summary>ゲームパッド</summary>
        private Gamepad _gamepad;
        /// <summary>左モーター（低周波）の回転数</summary>
        [SerializeField] private float leftMotor = .8f;
        /// <summary>右モーター（高周波）の回転数</summary>
        [SerializeField] private float rightMotor = 0f;
        /// <summary>振動を停止するまでの時間</summary>
        [SerializeField] private float delayTime = .3f;
        /// <summary>振動を有効フラグ</summary>
        [SerializeField] private bool isVibrationEnabled;
        /// <summary>MIDIJack（DDJ-200）の入力を取得</summary>
        [SerializeField] private InputMidiJackDDJ200 inputMidiJackDDJ200;
        /// <summary>MIDIJack（DDJ-200）の入力を取得</summary>
        public InputMidiJackDDJ200 InputMidiJackDDJ200 => inputMidiJackDDJ200;
        /// <summary>UI用のInputAction</summary>
        [SerializeField] private InputUI inputUI;
        /// <summary>UI用のInputAction</summary>
        public InputUI InputUI => inputUI;

        private void Reset()
        {
            inputMidiJackDDJ200 = GetComponent<InputMidiJackDDJ200>();
            inputUI = GetComponent<InputUI>();
        }

        public void OnStart()
        {
            _inputActions = new FutureContents3D_Main();
            _inputActions.UI.Pause.started += inputUI.OnPaused;
            _inputActions.UI.Pause.performed += inputUI.OnPaused;
            _inputActions.UI.Pause.canceled += inputUI.OnPaused;

            _inputActions.Enable();

            _compositeDisposable = new CompositeDisposable();
            var utility = new SelectCommonUtility();
            var userDataSingleton = utility.UserDataSingleton;
            _currentInputMode = new IntReactiveProperty(userDataSingleton.UserBean.inputMode);
            // ゲームパッドの情報をセット
            _gamepad = Gamepad.current;

            var temp = new TemplateResourcesAccessory();
            // ステージ共通設定の取得
            var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);

            isVibrationEnabled = datas.vibrationEnableIndex == 1;
        }

        public bool Exit()
        {
            try
            {
                _inputActions.Dispose();
                _compositeDisposable.Clear();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void OnDestroy()
        {
            if (!StopVibration())
                Debug.LogError("振動停止の失敗");
        }

        /// <summary>
        /// 振動の再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayVibration()
        {
            try
            {
                if (isVibrationEnabled)
                {
                    if (_gamepad != null)
                        _gamepad.SetMotorSpeeds(leftMotor, rightMotor);
                    DOVirtual.DelayedCall(delayTime, () =>
                    {
                        if (!StopVibration())
                            Debug.LogError("振動停止の失敗");
                    });
                }
                else
                    Debug.Log("振動オフ設定済み");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// 振動停止
        /// </summary>
        /// <returns>成功／失敗</returns>
        private bool StopVibration()
        {
            try
            {
                if (_currentInputMode != null)
                {
                    if (_currentInputMode.Value == (int)InputMode.Gamepad)
                        _gamepad.ResetHaptics();
                    else
                        Debug.LogWarning($"振動機能なしデバイスを使用: [{(InputMode)_currentInputMode.Value}]");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }

    /// <summary>
    /// 各インプットのインターフェース
    /// </summary>
    public interface IInputSystemsOwner
    {
        /// <summary>
        /// 全ての入力をリセット
        /// </summary>
        public void DisableAll();
    }

    /// <summary>
    /// 入力モード
    /// </summary>
    public enum InputMode
    {
        /// <summary>コントローラー</summary>
        Gamepad,
        /// <summary>キーボード</summary>
        Keyboard,
        /// <summary>DDJ-200</summary>
        MidiJackDDJ200,
    }
}
