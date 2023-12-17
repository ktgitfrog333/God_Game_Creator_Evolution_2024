using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.Audio
{
    /// <summary>
    /// オーディオのオーナー
    /// </summary>
    public class AudioOwner : MonoBehaviour, IMainGameManager, ISfxPlayer, IBgmPlayer
    {
        /// <summary>SEのプレイヤー</summary>
        [SerializeField] private SfxPlayer sfxPlayer;
        /// <summary>BGMのプレイヤー</summary>
        [SerializeField] private BgmPlayer bgmPlayer;
        /// <summary>オーディオミキサー</summary>
        [SerializeField] private AudioMixerController audioMixer;

        private void Reset()
        {
            sfxPlayer = GameObject.Find("SfxPlayer").GetComponent<SfxPlayer>();
            bgmPlayer = GameObject.Find("BgmPlayer").GetComponent<BgmPlayer>();
            audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioMixerController>();
        }

        public void OnStart()
        {
            sfxPlayer.OnStart();
            audioMixer.OnStart();
            bgmPlayer.OnStartAndPlayBGM();
        }

        public void PlaySFX(ClipToPlay clipToPlay)
        {
            sfxPlayer.PlaySFX(clipToPlay, false);
        }

        public void PlaySFX(ClipToPlay clipToPlay, bool isLoopmode)
        {
            sfxPlayer.PlaySFX(clipToPlay, isLoopmode);
        }

        public void PlayBGM(ClipToPlayBGM clipToPlay)
        {
            bgmPlayer.PlayBGM(clipToPlay);
        }

        public void StopSFX(ClipToPlay clipToPlay)
        {
            sfxPlayer.StopSFX(clipToPlay);
        }

        /// <summary>
        /// BGMを再生
        /// ※ステージ開始時に呼ばれる
        /// </summary>
        public void OnStartAndPlayBGM()
        {
            bgmPlayer.OnStartAndPlayBGM();
        }

        public bool ChangeSpeed(ClipToPlay clipToPlay, BgmConfDetails bgmConfDetails)
        {
            return sfxPlayer.ChangeSpeed(clipToPlay, bgmConfDetails);
        }
    }

    /// <summary>
    /// SE・ME用インターフェース
    /// </summary>
    public interface ISfxPlayer
    {
        /// <summary>
        /// 指定されたSEを再生する
        /// </summary>
        /// <param name="clipToPlay">SE</param>
        public void PlaySFX(ClipToPlay clipToPlay) { }
        /// <summary>
        /// 指定されたSEを再生する
        /// </summary>
        /// <param name="clipToPlay">SE</param>
        /// <param name="isLoopmode">ループモード</param>
        public void PlaySFX(ClipToPlay clipToPlay, bool isLoopmode) { }
        /// <summary>
        /// 指定されたSEを停止する
        /// </summary>
        /// <param name="clipToPlay">SE</param>
        public void StopSFX(ClipToPlay clipToPlay) { }
        /// <summary>
        /// BGMの再生速度を変更する
        /// </summary>
        /// <param name="clipToPlay">SE</param>
        /// <param name="bgmConfDetails">BGM設定の詳細</param>
        /// <returns>成功／失敗</returns>
        public bool ChangeSpeed(ClipToPlay clipToPlay, BgmConfDetails bgmConfDetails);
    }

    /// <summary>
    /// SFX・MEオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlay
    {
        /// <summary>ゲームクリア</summary>
        me_game_clear,
        /// <summary>キャンセル</summary>
        se_cancel,
        /// <summary>項目の決定</summary>
        se_decided,
        /// <summary>遊び方表_開く音</summary>
        se_play_open,
        /// <summary>圧死音</summary>
        se_player_dead,
        /// <summary>落下音</summary>
        se_player_fall,
        /// <summary>ジャンプ</summary>
        se_player_jump,
        /// <summary>リトライ</summary>
        se_retry,
        /// <summary>ステージセレクト</summary>
        se_select,
        /// <summary>スクラッチ音_1</summary>
        se_scratch_1,
    }

    /// <summary>
    /// BGM用インターフェース
    /// </summary>
    public interface IBgmPlayer
    {
        /// <summary>
        /// 指定されたBGMを再生する
        /// </summary>
        /// <param name="clipToPlay">BGM</param>
        public void PlayBGM(ClipToPlayBGM clipToPlay) { }
        /// <summary>
        /// BGMを再生
        /// ※ステージ開始時に呼ばれる
        /// </summary>
        public void OnStartAndPlayBGM();
    }

    /// <summary>
    /// BGMオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlayBGM
    {
        /// <summary>ステージ1～10のBGM</summary>
        bgm_stage_vol1,
        /// <summary>ステージ11～20のBGM</summary>
        bgm_stage_vol2,
        /// <summary>ステージ21～30のBGM</summary>
        bgm_stage_vol3,
        /// <summary>ステージ31～40のBGM</summary>
        bgm_stage_vol4,
        /// <summary>ステージ41～50のBGM</summary>
        bgm_stage_vol5,
    }
}
