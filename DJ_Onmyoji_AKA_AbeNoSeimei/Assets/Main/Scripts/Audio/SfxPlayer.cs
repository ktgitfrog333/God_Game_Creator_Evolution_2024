using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

namespace Main.Audio
{
    /// <summary>
    /// SEのプレイヤー
    /// </summary>
    public class SfxPlayer : MonoBehaviour, IMainGameManager, ISfxPlayer
    {
        /// <summary>効果音のクリップ</summary>
        [SerializeField] private AudioClip[] clip;

        /// <summary>オーディオソース用のプレハブ</summary>
        [SerializeField] private GameObject sFXChannelPrefab;
        /// <summary>プール用</summary>
        private Transform _transform;
        /// <summary>プール済みのオーディオ情報マップ</summary>
        private Dictionary<ClipToPlay, int> _sfxIdxDictionary = new Dictionary<ClipToPlay, int>();
        /// <summary>ピッチ調整の一時ロック</summary>
        private bool _isLookUpdOfPitch = false;
        /// <summary>様々な遅延時間</summary>
        [SerializeField] private float[] delaies = { 0.1f };
        /// <summary>効果音のクリップ（反転）</summary>
        private AudioClip _reversedClip;
        /// <summary>効果音のクリップ名（反転）</summary>
        private readonly string REVERSED_CLIP = "ReversedClip";

        public void OnStart()
        {
            if (_transform == null)
                _transform = transform;
        }

        public void PlaySFX(ClipToPlay clipToPlay)
        {
            PlaySFX(clipToPlay, false);
        }

        public void PlaySFX(ClipToPlay clipToPlay, bool isLoopmode)
        {
            try
            {
                if ((int)clipToPlay <= (clip.Length - 1))
                {
                    var audio = GetSFXSource(clipToPlay);
                    audio.clip = clip[(int)clipToPlay];
                    audio.loop = isLoopmode;

                    // SEを再生
                    audio.Play();
                }
                else
                    throw new System.Exception($"対象のファイルが見つかりません:[{clipToPlay}]");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public void StopSFX(ClipToPlay clipToPlay)
        {
            try
            {
                if ((int)clipToPlay <= (clip.Length - 1))
                {
                    var audio = GetSFXSource(clipToPlay);
                    audio.clip = clip[(int)clipToPlay];
                    audio.loop = false;

                    // SEを停止
                    audio.Stop();
                }
                else
                    throw new System.Exception($"対象のファイルが見つかりません:[{clipToPlay}]");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        /// <summary>
        /// 再生データを反転させたオーディオクリップをセットする
        /// </summary>
        /// <param name="audioSource">オーディオソース</param>
        /// <returns>オーディオクリップ（反転）</returns>
        private AudioClip SetReverseClips(AudioSource audioSource)
        {
            var defaultClip = audioSource.clip;
            if (defaultClip == null)
                return null;
            float[] samples = new float[defaultClip.samples * defaultClip.channels];
            defaultClip.GetData(samples, 0);

            // サンプルデータを逆順にする
            System.Array.Reverse(samples);

            // 逆順にしたサンプルデータから新しいAudioClipを作成
            var reversedClip = AudioClip.Create(REVERSED_CLIP, defaultClip.samples, defaultClip.channels, defaultClip.frequency, false);
            reversedClip.SetData(samples, 0);

            return reversedClip;
        }

        public bool ChangeSpeed(ClipToPlay clipToPlay, BgmConfDetails bgmConfDetails)
        {
            try
            {
                if ((int)clipToPlay <= (clip.Length - 1))
                {
                    if (!_isLookUpdOfPitch)
                    {
                        _isLookUpdOfPitch = true;
                        var audio = GetSFXSource(clipToPlay);
                        audio.loop = false;
                        var pbSpeed = bgmConfDetails.PbSpeed;
                        var updateClip = ChangeAndGetClip(pbSpeed, audio, clipToPlay);
                        if (updateClip == null)
                        {
                            _isLookUpdOfPitch = false;
                            return true;
                        }
                        if (audio.clip == null ||
                        !audio.clip.name.Equals(updateClip.name))
                            audio.clip = updateClip;
                        audio.pitch = Mathf.Abs(pbSpeed);
                        DOVirtual.DelayedCall(delaies[0], () => _isLookUpdOfPitch = false);
                        if (!audio.isPlaying)
                            audio.Play();
                    }
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
        /// クリップの変更と取得
        /// </summary>
        /// <param name="pbSpeed">再生速度</param>
        /// <param name="audio">オーディオソース</param>
        /// <param name="clipToPlay">再生するクリップ</param>
        /// <returns>変更後のAudioClip</returns>
        private AudioClip ChangeAndGetClip(float pbSpeed, AudioSource audio, ClipToPlay clipToPlay)
        {
            if (pbSpeed < -.9f)
            {
                if (_reversedClip == null)
                {
                    var clip = SetReverseClips(audio);
                    if (clip != null)
                        _reversedClip = clip;
                    else
                        return null;
                }
                if (!audio.clip.name.Equals(REVERSED_CLIP))
                    return _reversedClip;
                else
                    return audio.clip;
            }
            else if (.9f < pbSpeed)
                return clip[(int)clipToPlay];
            else
                return null;
        }

        /// <summary>
        /// SFXのキーから対象のオーディオソースを取得する
        /// </summary>
        /// <param name="key">ClipToPlayのキー</param>
        /// <returns>オーディオソース</returns>
        private AudioSource GetSFXSource(ClipToPlay key)
        {
            if (!_sfxIdxDictionary.ContainsKey(key))
            {
                var sfx = Instantiate(sFXChannelPrefab);
                sfx.transform.parent = _transform;
                _sfxIdxDictionary.Add(key, _transform.childCount - 1);
                return sfx.GetComponent<AudioSource>();
            }
            return _transform.GetChild(_sfxIdxDictionary[key]).GetComponent<AudioSource>();
        }
    }

}
