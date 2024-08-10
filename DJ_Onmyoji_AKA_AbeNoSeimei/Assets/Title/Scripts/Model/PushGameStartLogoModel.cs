using System.Collections;
using System.Collections.Generic;
using Title.Common;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using Title.InputSystem;
using DG.Tweening;

namespace Title.Model
{
    /// <summary>
    /// モデル
    /// プッシュゲームスタート
    /// </summary>
    public class PushGameStartLogoModel : UIEventController
    {
        /// <summary>入力無視の時間（秒）</summary>
        [SerializeField] private float unDeadTimeSec = .2f;

        /// <summary>
        /// プレゼンタから開始タイミングを制御
        /// </summary>
        public bool OnStart()
        {
            try
            {
                var ignoreInitialInput = new BoolReactiveProperty();
                var wasAnyKeysPreseed = new BoolReactiveProperty();
                this.UpdateAsObservable()
                    .Select(_ => TitleGameManager.Instance)
                    .Where(x => x != null)
                    .Select(x => x.InputSystemsOwner)
                    .Where(x => x.CurrentInputMode.Value == (int)InputMode.MidiJackDDJ200)
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        this.ObserveEveryValueChanged(x => x.isActiveAndEnabled)
                            .Where(x => x)
                            .Subscribe(_ =>
                            {
                                ignoreInitialInput.Value = true;
                                wasAnyKeysPreseed.Value = false;
                                DOVirtual.DelayedCall(unDeadTimeSec, () => ignoreInitialInput.Value = false);
                            });
                    });

                this.UpdateAsObservable()
                    .Select(_ => TitleGameManager.Instance)
                    .Where(x => x != null)
                    .Select(x => x.InputSystemsOwner)
                    .Subscribe(x =>
                    {
                        switch ((InputMode)x.CurrentInputMode.Value)
                        {
                            case InputMode.Gamepad:
                                if (!SetAnyKeysPushedOfGamePadAndKeyboard(_eventState))
                                    Debug.LogError("SetAnyKeysPushedOfGamePadAndKeyboard");

                                break;
                            case InputMode.Keyboard:
                                if (!SetAnyKeysPushedOfGamePadAndKeyboard(_eventState))
                                    Debug.LogError("SetAnyKeysPushedOfGamePadAndKeyboard");

                                break;
                            case InputMode.MidiJackDDJ200:
                                if (!SetAnyKeysPushedOfMidiJackDDJ200(ignoreInitialInput, wasAnyKeysPreseed, x.InputMidiJackDDJ200, _eventState))
                                    Debug.LogError("SetAnyKeysPushedOfMidiJackDDJ200");

                                break;
                            default:
                                break;
                        }
                    });

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// いずれかのキー設定
        /// ゲームパッドとキーボード
        /// </summary>
        /// <param name="eventState">実行イベントの監視</param>
        /// <returns>成功／失敗</returns>
        private bool SetAnyKeysPushedOfGamePadAndKeyboard(IntReactiveProperty eventState)
        {
            try
            {
                if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                    (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame ||
                        Gamepad.current.buttonNorth.wasPressedThisFrame ||
                        Gamepad.current.buttonEast.wasPressedThisFrame ||
                        Gamepad.current.buttonWest.wasPressedThisFrame ||
                        Gamepad.current.leftShoulder.wasPressedThisFrame ||
                        Gamepad.current.rightShoulder.wasPressedThisFrame ||
                        Gamepad.current.leftTrigger.wasPressedThisFrame ||
                        Gamepad.current.rightTrigger.wasPressedThisFrame ||
                        Gamepad.current.startButton.wasPressedThisFrame ||
                        Gamepad.current.selectButton.wasPressedThisFrame)))
                {
                    eventState.Value = (int)EnumEventCommand.AnyKeysPushed;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// いずれかのキー設定
        /// MidiJackDDJ200
        /// </summary>
        /// <param name="ignoreInitialInput">初期状態の入力無効</param>
        /// <param name="wasAnyKeysPreseed">いずれかのキー再入力制御</param>
        /// <param name="inputMidiJackDDJ200">MidiJackDDJ200</param>
        /// <param name="eventState">実行イベントの監視</param>
        /// <returns>成功／失敗</returns>
        private bool SetAnyKeysPushedOfMidiJackDDJ200(BoolReactiveProperty ignoreInitialInput, BoolReactiveProperty wasAnyKeysPreseed, InputMidiJackDDJ200 inputMidiJackDDJ200, IntReactiveProperty eventState)
        {
            try
            {
                if (!ignoreInitialInput.Value &&
                    !wasAnyKeysPreseed.Value &&
                    (inputMidiJackDDJ200.Cue ||
                    inputMidiJackDDJ200.PlayOrPause ||
                    inputMidiJackDDJ200.Pad1 ||
                    inputMidiJackDDJ200.Pad2 ||
                    inputMidiJackDDJ200.Pad3 ||
                    inputMidiJackDDJ200.Pad4 ||
                    inputMidiJackDDJ200.Pad5 ||
                    inputMidiJackDDJ200.Pad6 ||
                    inputMidiJackDDJ200.Pad7 ||
                    inputMidiJackDDJ200.Pad8))
                {
                    wasAnyKeysPreseed.Value = true;
                    eventState.Value = (int)EnumEventCommand.AnyKeysPushed;
                    inputMidiJackDDJ200.DoResetAllKey();
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
