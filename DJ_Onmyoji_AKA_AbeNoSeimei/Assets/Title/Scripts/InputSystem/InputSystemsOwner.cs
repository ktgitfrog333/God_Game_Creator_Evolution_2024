using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Title.Common;
using UnityEngine.InputSystem;
using UniRx;
using Universal.Common;
using Title.Utility;

namespace Title.InputSystem
{
    /// <summary>
    /// InputSystemのオーナー
    /// </summary>
    public class InputSystemsOwner : MonoBehaviour, ITitleGameManager
    {
        /// <summary>ゲームパッド</summary>
        private Gamepad _gamepad;
        /// <summary>左モーター（低周波）の回転数</summary>
        [SerializeField] private float leftMotor = .8f;
        /// <summary>右モーター（高周波）の回転数</summary>
        [SerializeField] private float rightMotor = 0f;
        /// <summary>振動を停止するまでの時間</summary>
        [SerializeField] private float delayTime = .3f;
        /// <summary>現在の入力モード（コントローラー／キーボード）</summary>
        private IntReactiveProperty _currentInputMode;
        /// <summary>現在の入力モード（コントローラー／キーボード）</summary>
        public IntReactiveProperty CurrentInputMode => _currentInputMode;
        /// <summary>MIDIJack（DDJ-200）の入力を取得</summary>
        [SerializeField] private InputMidiJackDDJ200 inputMidiJackDDJ200;
        /// <summary>MIDIJack（DDJ-200）の入力を取得</summary>
        public InputMidiJackDDJ200 InputMidiJackDDJ200 => inputMidiJackDDJ200;

        private void Reset()
        {
            inputMidiJackDDJ200 = GetComponent<InputMidiJackDDJ200>();
        }

        public void OnStart()
        {
            // ゲームパッドの情報をセット
            _gamepad = Gamepad.current;
            var utility = new TitleCommonUtility();
            var userDataSingleton = utility.UserDataSingleton;
            _currentInputMode = new IntReactiveProperty(userDataSingleton.UserBean.inputMode);
        }

        /// <summary>
        /// 振動の再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayVibration()
        {
            try
            {
                if (_gamepad != null)
                    _gamepad.SetMotorSpeeds(leftMotor, rightMotor);
                DOVirtual.DelayedCall(delayTime, () =>
                {
                    if (!StopVibration())
                        Debug.LogError("振動停止の失敗");
                });
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
                _gamepad.ResetHaptics();

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
