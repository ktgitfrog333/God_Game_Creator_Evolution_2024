using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Universal.Template;
using Universal.Common;
using Main.Utility;

namespace Main.InputSystem
{
    /// <summary>
    /// InputSystemのオーナー
    /// </summary>
    public class InputSystemsOwner : MonoBehaviour, IMainGameManager
    {
        /// <summary>プレイヤー用のインプットイベント</summary>
        [SerializeField] private InputPlayer inputPlayer;
        /// <summary>プレイヤー用のインプットイベント</summary>
        public InputPlayer InputPlayer => inputPlayer;
        /// <summary>UI用のインプットイベント</summary>
        [SerializeField] private InputUI inputUI;
        /// <summary>UI用のインプットイベント</summary>
        public InputUI InputUI => inputUI;
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
        /// <summary>入力情報の履歴</summary>
        [SerializeField] private InputHistroy inputHistroy;
        /// <summary>入力情報の履歴</summary>
        public InputHistroy InputHistroy => inputHistroy;
        /// <summary>MIDIJack（DDJ-200）の入力を取得</summary>
        [SerializeField] private InputMidiJackDDJ200 inputMidiJackDDJ200;
        /// <summary>MIDIJack（DDJ-200）の入力を取得</summary>
        public InputMidiJackDDJ200 InputMidiJackDDJ200 => inputMidiJackDDJ200;

        private void Reset()
        {
            inputPlayer = GetComponent<InputPlayer>();
            inputUI = GetComponent<InputUI>();
            inputHistroy = GetComponent<InputHistroy>();
            inputMidiJackDDJ200 = GetComponent<InputMidiJackDDJ200>();
        }

        public void OnStart()
        {
            _inputActions = new FutureContents3D_Main();
            _inputActions.Player.MoveLeft.started += inputPlayer.OnMovedLeft;
            _inputActions.Player.MoveLeft.performed += inputPlayer.OnMovedLeft;
            _inputActions.Player.MoveLeft.canceled += inputPlayer.OnMovedLeft;
            _inputActions.Player.MoveRight.started += inputPlayer.OnMovedRight;
            _inputActions.Player.MoveRight.performed += inputPlayer.OnMovedRight;
            _inputActions.Player.MoveRight.canceled += inputPlayer.OnMovedRight;
            _inputActions.Player.Jump.started += inputPlayer.OnJumped;
            _inputActions.Player.Jump.performed += inputPlayer.OnJumped;
            _inputActions.Player.Jump.canceled += inputPlayer.OnJumped;
            _inputActions.UI.Pause.started += inputUI.OnPaused;
            _inputActions.UI.Pause.performed += inputUI.OnPaused;
            _inputActions.UI.Pause.canceled += inputUI.OnPaused;
            _inputActions.UI.Space.started += inputUI.OnSpaced;
            _inputActions.UI.Space.performed += inputUI.OnSpaced;
            _inputActions.UI.Space.canceled += inputUI.OnSpaced;
            _inputActions.UI.Undo.started += inputUI.OnUndoed;
            _inputActions.UI.Undo.canceled += inputUI.OnUndoed;
            _inputActions.UI.Select.started += inputUI.OnSelected;
            _inputActions.UI.Select.canceled += inputUI.OnSelected;
            _inputActions.UI.Manual.started += inputUI.OnManualed;
            _inputActions.UI.Manual.canceled += inputUI.OnManualed;
            _inputActions.UI.Scratch.started += inputUI.OnScratch;
            _inputActions.UI.Scratch.performed += inputUI.OnScratch;
            _inputActions.UI.Scratch.canceled += inputUI.OnScratch;
            _inputActions.UI.ChargeSun.started += inputUI.OnChargeSun;
            _inputActions.UI.ChargeSun.performed += inputUI.OnChargeSun;
            _inputActions.UI.ChargeSun.canceled += inputUI.OnChargeSun;
            _inputActions.UI.ChargeMoon.started += inputUI.OnChargeMoon;
            _inputActions.UI.ChargeMoon.performed += inputUI.OnChargeMoon;
            _inputActions.UI.ChargeMoon.canceled += inputUI.OnChargeMoon;
            _inputActions.UI.ChargeRFader.started += inputUI.OnChargeRFader;
            _inputActions.UI.ChargeRFader.performed += inputUI.OnChargeRFader;
            _inputActions.UI.ChargeRFader.canceled += inputUI.OnChargeRFader;
            _inputActions.UI.ChargeLFader.started += inputUI.OnChargeLFader;
            _inputActions.UI.ChargeLFader.performed += inputUI.OnChargeLFader;
            _inputActions.UI.ChargeLFader.canceled += inputUI.OnChargeLFader;
            _inputActions.UI.ReleaseRFader.started += inputUI.OnReleaseRFader;
            _inputActions.UI.ReleaseRFader.performed += inputUI.OnReleaseRFader;
            _inputActions.UI.ReleaseRFader.canceled += inputUI.OnReleaseRFader;
            _inputActions.UI.ReleaseLFader.started += inputUI.OnReleaseLFader;
            _inputActions.UI.ReleaseLFader.performed += inputUI.OnReleaseLFader;
            _inputActions.UI.ReleaseLFader.canceled += inputUI.OnReleaseLFader;
            _inputActions.UI.Navigate.started += inputUI.OnNavigated;
            _inputActions.UI.Navigate.performed += inputUI.OnNavigated;
            _inputActions.UI.Navigate.canceled += inputUI.OnNavigated;

            _inputActions.Enable();

            _compositeDisposable = new CompositeDisposable();
            var utility = new MainCommonUtility();
            var userDataSingleton = utility.UserDataSingleton;
            _currentInputMode = new IntReactiveProperty(userDataSingleton.UserBean.inputMode);
            // 入力モード 0:キーボード 1:コントローラー
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
                    {
                        _currentInputMode.Value = (int)InputMode.Keyboard;
                    }
                    else if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
                    {
                        _currentInputMode.Value = (int)InputMode.Gamepad;
                    }
                })
                .AddTo(_compositeDisposable);
            // ゲームパッドの情報をセット
            _gamepad = Gamepad.current;

            var temp = new TemplateResourcesAccessory();
            // ステージ共通設定の取得
            var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);

            isVibrationEnabled = datas.vibrationEnableIndex == 1;
            inputHistroy.OnStart();
        }

        public bool Exit()
        {
            try
            {
                _inputActions.Dispose();
                inputPlayer.DisableAll();
                inputUI.DisableAll();
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
