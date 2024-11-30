using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Template;
using Universal.Common;
using DG.Tweening;
using Main.Common;
using System.Linq;
using Main.Utility;

namespace Main.Audio
{
    /// <summary>
    /// BGMのプレイヤー
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class BgmPlayer : MonoBehaviour, IBgmPlayer
    {
        /// <summary>オーディオソース</summary>
        [SerializeField] private AudioSource audioSource;
        /// <summary>効果音のクリップ</summary>
        [SerializeField] private AudioClip[] clip;
        /// <summary>再生時間を戻す時間（秒）</summary>
        [SerializeField] private float reverseTLTimeSec = 21f;
        /// <summary>再生時間を戻す時間（秒）のランダム加算値</summary>
        [SerializeField] private float reverseTLTimeSecAddRandomRangeMax = 2f;
        /// <summary>処理場で必要なBGMの情報</summary>
        [SerializeField] private BGMInfo[] bGMInfos;
        /// <summary>
        /// ステージごとのBGMの組み合わせマップ
        /// ここの設定は AdminData.json の playBgmNames と連動していないため
        /// 上記を変更した場合はインスペクタからここの設定も変更する
        /// ※オーディオファイルの紐づけを変えてしまった方が早いかもしれない
        /// </summary>
        /// <see cref="Assets/SaveDatas/AdminData.json"/>
        [SerializeField] private BGMDayOrNightMap[] bGMDayOrNightMaps;
        /// <summary>BGM開始時の（秒）</summary>
        private float startBGMTimeSec = 0f;
        /// <summary>共通のユーティリティ</summary>
        private MainCommonUtility _mainCommonUtility = new MainCommonUtility();

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = false;
        }

        public void PlayBGM(ClipToPlayBGM clipToPlay)
        {
            PlayAudioSource(clipToPlay);
        }

        public void OnStartAndPlayBGM()
        {
            startBGMTimeSec = Time.time;
            // clipToPlayBGMNight
            SwitchClip(bGMDayOrNightMaps.Where(q => q.sceneId == _mainCommonUtility.UserDataSingleton.UserBean.sceneId)
                .Select(q => q.clipToPlayBGMNight)
                .ToArray());
            // clipToPlayBGMDay
            SwitchClip(bGMDayOrNightMaps.Where(q => q.sceneId == _mainCommonUtility.UserDataSingleton.UserBean.sceneId)
                .Select(q => q.clipToPlayBGMDay)
                .ToArray());
        }

        /// <summary>
        /// シーンIDを元にBGMを取得
        /// </summary>
        /// <returns>BGM</returns>
        private int GetClipToPlayFromSceneId()
        {
            var utility = new MainCommonUtility();
            // ステージIDの取得
            var userDatas = utility.UserDataSingleton.UserBean;
            // ステージ共通設定の取得
            var adminDatas = utility.AdminDataSingleton.AdminBean;
            return adminDatas.playBgmNames[userDatas.sceneId - 1] - 1;
        }

        /// <summary>
        /// 指定されたBGMを再生する
        /// </summary>
        /// <param name="clipToPlay">BGM</param>
        private void PlayAudioSource(ClipToPlayBGM clipToPlay)
        {
            try
            {
                if ((int)clipToPlay <= (clip.Length - 1))
                {
                    audioSource.clip = clip[(int)clipToPlay];
                    var sceneId = _mainCommonUtility.UserDataSingleton.UserBean.sceneId;
                    // シーン8（チュートリアルステージ）ならループ
                    audioSource.loop = sceneId == 8;

                    // BGMを再生
                    audioSource.Play();
                }
                else
                    throw new System.Exception($"対象のファイルが見つかりません:[{clipToPlay}]");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public IEnumerator PlayFadeOut(System.IObserver<bool> observer, float duration)
        {
            // DOTweenを使用してボリュームを0にする
            audioSource.DOFade(0f, duration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => observer.OnNext(true));
            yield return null;
        }

        public void PlayBack()
        {
            float totalReverseTime = reverseTLTimeSec + Random.Range(0, reverseTLTimeSecAddRandomRangeMax);
            PlayBackCommon(totalReverseTime);
            audioSource.volume = 1f;
        }

        public void PlayBack(InputSlipLoopState inputSlipLoopState)
        {

            float beat = GetBeat();
            float totalReverseTime = BeatLengthApp.GetTotalReverse(inputSlipLoopState, beat);
            PlayBackCommon(totalReverseTime);
        }

        /// <summary>
        /// 拍を取得
        /// </summary>
        /// <returns>拍</returns>
        private float GetBeat()
        {
            var clipToPlayBGM = clip.Select((p, i) => new { Content = p, Index = i })
                .Where(q => q.Content.Equals(audioSource.clip))
                .Select(q => (ClipToPlayBGM)q.Index)
                .ToArray();
            if (clipToPlayBGM.Length < 1)
                throw new System.ArgumentNullException($"再生中のBGMがClipToPlayBGMに存在しない:[{audioSource.clip}]");

            var bpm = bGMInfos.Where(q => q.clipToPlayBGM.Equals(clipToPlayBGM[0]))
                .Select(q => q.bpm)
                .ToArray();
            if (bpm.Length < 1)
                throw new System.ArgumentNullException($"対象のBPMがBGM情報に存在しない:[{clipToPlayBGM[0]}]");

            return 60f / bpm[0];
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="totalReverseTime">指定リターン時間</param>
        private void PlayBackCommon(float totalReverseTime)
        {
            float newTime = audioSource.time - totalReverseTime;
            if (newTime < 0) newTime = 0; // オーディオの開始時間より前には戻らないようにする
            audioSource.time = newTime;
        }

        public float GetBeatBGM()
        {
            return GetBeat();
        }

        public bool SetVolumeOn()
        {
            try
            {
                audioSource.volume = 1f;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool AdjustBGM()
        {
            try
            {
                float time = Time.time - startBGMTimeSec;
                if (time > audioSource.clip.length)
                {
                    Debug.LogWarning($"BGMの再生時間の限界を超過: [{time}]");
                    time = audioSource.clip.length;
                }
                audioSource.time = time;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SwitchClipDay()
        {
            try
            {
                return SwitchClip(bGMDayOrNightMaps.Where(q => q.sceneId == _mainCommonUtility.UserDataSingleton.UserBean.sceneId)
                    .Select(q => q.clipToPlayBGMDay)
                    .ToArray());
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SwitchClipNight()
        {
            try
            {
                return SwitchClip(bGMDayOrNightMaps.Where(q => q.sceneId == _mainCommonUtility.UserDataSingleton.UserBean.sceneId)
                    .Select(q => q.clipToPlayBGMNight)
                    .ToArray());
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// オーディオクリップ切替
        /// </summary>
        /// <param name="clipToPlayBGMs">BGMオーディオクリップリストのインデックス</param>
        /// <returns>成功／失敗</returns>
        private bool SwitchClip(ClipToPlayBGM[] clipToPlayBGMs)
        {
            try
            {
                if (0 < clipToPlayBGMs.Length)
                {
                    var time = audioSource.time;
                    PlayAudioSource(clipToPlayBGMs[0]);
                    audioSource.time = time;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void StopBGM()
        {
            audioSource.Stop();
        }

        public void Pause()
        {
            audioSource.Pause();
        }

        public void UnPause()
        {
            audioSource.UnPause();
        }
    }

    /// <summary>
    /// 処理場で必要なBGMの情報
    /// </summary>
    [System.Serializable]
    public struct BGMInfo
    {
        /// <summary>オーディオクリップ</summary>
        public ClipToPlayBGM clipToPlayBGM;
        /// <summary>BPM</summary>
        public float bpm;
    }

    /// <summary>
    /// ステージごとのBGMの組み合わせマップ
    /// </summary>
    [System.Serializable]
    public struct BGMDayOrNightMap
    {
        /// <summary>シーンID</summary>
        public int sceneId;
        /// <summary>オーディオクリップ</summary>
        public ClipToPlayBGM clipToPlayBGMDay;
        /// <summary>オーディオクリップ</summary>
        public ClipToPlayBGM clipToPlayBGMNight;
    }
}
