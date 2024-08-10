using MidiJack;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace Title.InputSystem
{
    /// <summary>
    /// MidiJack
    /// DDJ-200を使用する前提
    /// </summary>
    public class InputMidiJackDDJ200 : MonoBehaviour
    {
        /// <summary>入力の制限時間</summary>
        private float _elapsedTime;
        /// <summary>入力の制限加算値（ポーズ時）</summary>
        private float fixDeltaTime = .01f;

        private void Start()
        {
            MidiMaster.knobDelegate += OnScratch;
            MidiMaster.noteOnDelegate += OnPads;
            MidiMaster.noteOnDelegate += OnUiButton;
            MidiMaster.knobDelegate += OnMixer8;
            MidiMaster.knobDelegate += OnMixers;
            FloatReactiveProperty elapsedTime = new FloatReactiveProperty();
            this.UpdateAsObservable()
                .Where(_ => _scratch != 0f)
                .Subscribe(_ =>
                {
                    _elapsedTime += Time.timeScale == 1f ? Time.deltaTime : fixDeltaTime;
                    if (userActionTime < _elapsedTime)
                        if (!ResetTime(ref _scratch, ref _elapsedTime))
                            Debug.LogError("ResetTime");
                });
        }

        /// <summary>
        /// 時間をリセット
        /// </summary>
        /// <param name="scratch">スクラッチ</param>
        /// <param name="elapsedTime">入力の制限時間</param>
        /// <returns>成功／失敗</returns>
        private bool ResetTime(ref float scratch, ref float elapsedTime)
        {
            try
            {
                elapsedTime = 0f;
                scratch = 0f;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void OnDestroy()
        {
            MidiMaster.knobDelegate -= OnScratch;
            MidiMaster.noteOnDelegate -= OnPads;
            MidiMaster.noteOnDelegate -= OnUiButton;
            MidiMaster.knobDelegate -= OnMixer8;
            MidiMaster.knobDelegate -= OnMixers;
            ResetAllKey();
        }

        /// <summary>
        /// 全てのキーをリセット
        /// </summary>
        public void DoResetAllKey()
        {
            ResetAllKey();
        }
        /// <summary>
        /// 全てのキーをリセット
        /// </summary>
        private void ResetAllKey()
        {
            _scratch = 0f;
            _pad1 = false;
            _pad2 = false;
            _pad3 = false;
            _pad4 = false;
            _pad5 = false;
            _pad6 = false;
            _pad7 = false;
            _pad8 = false;
            _mixer8 = 0f;
            _mixer1 = 0f;
            _playOrPause = false;
            _cue = false;
        }
        /// <summary>スクラッチ</summary>
        private float _scratch;
        /// <summary>スクラッチ</summary>
        public float Scratch => _scratch;
        /// <summary>スクラッチの値レベル</summary>
        [SerializeField] private float scratchLevel = 1f;
        /// <summary>ユーザの1入力を行う平均時間（0.2～0.5秒）</summary>
        [SerializeField] private float userActionTime = .2f;

        /// <summary>
        /// Scratchのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="channel">Midiチャンネル</param>
        /// <param name="knob">ノブ</param>
        /// <param name="value">値</param>
        public void OnScratch(MidiChannel channel, int knob, float value)
        {
            switch (channel)
            {
                case MidiChannel.Ch1:
                    break;
                case MidiChannel.Ch2:
                    break;
                default:
                    return;
            }

            switch ((MidiChannelKnob)knob)
            {
                case MidiChannelKnob.D1_T:
                    if (!UpdateScratch(value))
                        Debug.LogError("UpdateScratch");

                    break;
                case MidiChannelKnob.D1_S:
                    if (!UpdateScratch(value))
                        Debug.LogError("UpdateScratch");

                    break;
                default:
                    _scratch = 0f;

                    break;
            }
        }
        /// <summary>
        /// スクラッチの値を更新
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>成功／失敗</returns>
        private bool UpdateScratch(float value)
        {
            try
            {
                // valueが0（反時計回り）か1（時計回り）の場合、それに応じて_scratchを更新
                if (value == .496063f)
                {
                    // 反時計回りの場合、_scratchを減少させる
                    if (0f < _scratch)
                        if (!ResetTime(ref _scratch, ref _elapsedTime))
                            Debug.LogError("ResetTime");
                    _scratch -= scratchLevel; // この値は調整が必要かもしれません
                }
                else if (value == .511811f)
                {
                    if (_scratch < 0f)
                        if (!ResetTime(ref _scratch, ref _elapsedTime))
                            Debug.LogError("ResetTime");
                    // 時計回りの場合、_scratchを増加させる
                    _scratch += scratchLevel; // この値は調整が必要かもしれません
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>パッド1</summary>
        private bool _pad1;
        /// <summary>パッド1</summary>
        public bool Pad1 => _pad1;
        /// <summary>パッド2</summary>
        private bool _pad2;
        /// <summary>パッド2</summary>
        public bool Pad2 => _pad2;
        /// <summary>パッド3</summary>
        private bool _pad3;
        /// <summary>パッド3</summary>
        public bool Pad3 => _pad3;
        /// <summary>パッド4</summary>
        private bool _pad4;
        /// <summary>パッド4</summary>
        public bool Pad4 => _pad4;
        /// <summary>パッド5</summary>
        private bool _pad5;
        /// <summary>パッド5</summary>
        public bool Pad5 => _pad5;
        /// <summary>パッド6</summary>
        public bool _pad6;
        /// <summary>パッド6</summary>
        public bool Pad6 => _pad6;
        /// <summary>パッド7</summary>
        public bool _pad7;
        /// <summary>パッド7</summary>
        public bool Pad7 => _pad7;
        /// <summary>パッド8</summary>
        public bool _pad8;
        /// <summary>パッド8</summary>
        public bool Pad8 => _pad8;
        /// <summary>
        /// パッド5
        /// </summary>
        /// <param name="channel">Midiチャンネル</param>
        /// <param name="note">ノート</param>
        /// <param name="value">値</param>
        public void OnPads(MidiChannel channel, int note, float value)
        {
            switch (channel)
            {
                case MidiChannel.Ch8:
                    switch ((MidiChannelKnob)note)
                    {
                        case MidiChannelKnob.P1:
                            _pad1 = value == 1;

                            break;
                        case MidiChannelKnob.P2:
                            _pad2 = value == 1;

                            break;
                        case MidiChannelKnob.P3:
                            _pad3 = value == 1;

                            break;
                        case MidiChannelKnob.P4:
                            _pad4 = value == 1;

                            break;
                        case MidiChannelKnob.P5:
                            _pad5 = value == 1;

                            break;
                        case MidiChannelKnob.P6:
                            _pad6 = value == 1;

                            break;
                        case MidiChannelKnob.P7:
                            _pad7 = value == 1;

                            break;
                        case MidiChannelKnob.P8:
                            _pad8 = value == 1;

                            break;
                        default:
                            _pad1 = false;
                            _pad2 = false;
                            _pad3 = false;
                            _pad4 = false;
                            _pad5 = false;
                            _pad6 = false;
                            _pad7 = false;
                            _pad8 = false;

                            break;
                    }

                    break;
            }
        }

        /// <summary>クロスフェーダー</summary>
        private float _mixer8 = 1f;
        /// <summary>クロスフェーダー</summary>
        public float Mixer8 => _mixer8;

        /// <summary>
        /// クロスフェーダー
        /// </summary>
        /// <param name="channel">Midiチャンネル</param>
        /// <param name="knob">ノブ</param>
        /// <param name="value">値</param>
        public void OnMixer8(MidiChannel channel, int knob, float value)
        {
            switch (channel)
            {
                case MidiChannel.Ch7:
                    switch ((MidiChannelKnob)knob)
                    {
                        case MidiChannelKnob.M8:
                            _mixer8 = value;

                            break;
                    }

                    break;
            }
        }

        /// <summary>イコライザ（HI）</summary>
        private float _mixer1 = .5f;
        /// <summary>イコライザ（HI）</summary>
        public float Mixer1 => _mixer1;
        /// <summary>イコライザ（MID）</summary>
        private float _mixer2 = .5f;
        /// <summary>イコライザ（MID）</summary>
        public float Mixer2 => _mixer2;
        /// <summary>イコライザ（LOW）</summary>
        private float _mixer3 = .5f;
        /// <summary>イコライザ（LOW）</summary>
        public float Mixer3 => _mixer3;
        /// <summary>
        /// イコライザ（HI/MID/LOW）
        /// </summary>
        /// <param name="channel">Midiチャンネル</param>
        /// <param name="knob">ノブ</param>
        /// <param name="value">値</param>
        public void OnMixers(MidiChannel channel, int knob, float value)
        {
            switch (channel)
            {
                case MidiChannel.Ch1:
                    switch ((MidiChannelKnob)knob)
                    {
                        case MidiChannelKnob.M1:
                            _mixer1 = value;

                            break;
                        case MidiChannelKnob.M2:
                            _mixer2 = value;

                            break;
                        case MidiChannelKnob.M3:
                            _mixer3 = value;

                            break;
                    }

                    break;
            }
        }

        /// <summary>PLAY/PAUSE</summary>
        private bool _playOrPause;
        /// <summary>PLAY/PAUSE</summary>
        public bool PlayOrPause => _playOrPause;
        /// <summary>CUE</summary>
        private bool _cue;
        /// <summary>CUE</summary>
        public bool Cue => _cue;
        /// <summary>PLAY/PAUSE/CUE</summary>
        public void OnUiButton(MidiChannel channel, int note, float value)
        {
            switch (channel)
            {
                case MidiChannel.Ch1:
                    break;
                case MidiChannel.Ch2:
                    break;
                default:
                    return;
            }

            switch ((MidiChannelKnob)note)
            {
                case MidiChannelKnob.D4:
                    _playOrPause = value == 1f;

                    break;
                case MidiChannelKnob.D5:
                    _cue = value == 1f;

                    break;
                default:
                    _playOrPause = false;
                    _cue = false;

                    break;
            }

        }

        /// <summary>
        /// LayOUTがAutomat5のA～D,M
        /// </summary>
        private enum MidiChannelKnob
        {
            D1_T = 34,
            D1_S = 33,
            D4 = 11,
            D5 = 12,
            M1 = 7,
            M2 = 11,
            M3 = 15,
            P1 = 0,
            P2 = 1,
            P3 = 2,
            P4 = 3,
            P5 = 4,
            P6 = 5,
            P7 = 6,
            P8 = 7,
            // 63は別の法則による取得値のため無視する
            M8 = 31,
            //A = 0,
            //B = 1,
            //C = 2,
            //D = 3,
            //M = 26,
            //A_Submit = 29,
            //A_Pad_2 = 30,
        }

        [System.Serializable]
        /// <summary>
        /// Midi（ノブ）
        /// プロパティ
        /// </summary>
        private struct MidiKnobProp
        {
            /// <summary>Midiチャンネル</summary>
            public MidiChannel channel;
            /// <summary>ノブ</summary>
            public int knob;
            /// <summary>値</summary>
            public float value;
        }
    }
}
