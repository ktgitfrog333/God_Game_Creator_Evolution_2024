using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Template;
using Universal.Common;
using DG.Tweening;

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

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
        }

        public void PlayBGM(ClipToPlayBGM clipToPlay)
        {
            PlayAudioSource(clipToPlay);
        }

        public void OnStartAndPlayBGM()
        {
            var temp = new TemplateResourcesAccessory();
            // ステージIDの取得
            var userDatas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            // ステージ共通設定の取得
            var adminDatas = temp.LoadSaveDatasJsonOfAdminBean(ConstResorcesNames.ADMIN_DATA);
            var clipToPlay = adminDatas.playBgmNames[userDatas.sceneId - 1] - 1;

            PlayAudioSource((ClipToPlayBGM)clipToPlay);
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
            float newTime = audioSource.time - totalReverseTime;
            if (newTime < 0) newTime = 0; // オーディオの開始時間より前には戻らないようにする
            audioSource.time = newTime;
            audioSource.volume = 1f;
        }
    }
}
