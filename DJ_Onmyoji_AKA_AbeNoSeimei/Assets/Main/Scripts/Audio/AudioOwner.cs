using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;

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

        public IEnumerator PlayFadeOut(System.IObserver<bool> observer, float duration)
        {
            Observable.FromCoroutine<bool>(observer => bgmPlayer.PlayFadeOut(observer, duration))
                .Subscribe(_ => observer.OnNext(true))
                .AddTo(gameObject);
            yield return null;
        }

        public void PlayBack()
        {
            bgmPlayer.PlayBack();
        }

        public void PlayBack(InputSlipLoopState inputSlipLoopState)
        {
            bgmPlayer.PlayBack(inputSlipLoopState);
        }

        public float GetBeatBGM()
        {
            return bgmPlayer.GetBeatBGM();
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
        /// <summary>DJのスクラッチ2</summary>
        se_backspin,
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
        /// <summary>
        /// BGMをフェードアウト
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="duration">再生時間</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeOut(System.IObserver<bool> observer, float duration);
        /// <summary>
        /// BGMを再生する
        /// </summary>
        public void PlayBack();
        /// <summary>
        /// BGMを再生する
        /// 引数を元にあるポイントへ位置を移動させる
        /// </summary>
        /// <param name="inputSlipLoopState">スリップループの入力情報</param>
        public void PlayBack(InputSlipLoopState inputSlipLoopState);
        /// <summary>
        /// 拍を取得
        /// </summary>
        /// <returns>拍</returns>
        public float GetBeatBGM();
    }

    /// <summary>
    /// BGMオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlayBGM
    {
        /// <summary>ステージ1（昼）のBGM</summary>
        bgm_stage_vol1,
        /// <summary>ステージ2（昼）のBGM</summary>
        bgm_stage_vol2,
        /// <summary>ステージ3（昼）のBGM</summary>
        bgm_stage_vol3,
        /// <summary>ステージ4（昼）のBGM</summary>
        bgm_stage_vol4,
        /// <summary>ステージ5（昼）のBGM</summary>
        bgm_stage_vol5,
        /// <summary>ステージ6（昼）のBGM</summary>
        bgm_stage_vol6,
        /// <summary>ステージ7（昼）のBGM</summary>
        bgm_stage_vol7,
        /// <summary>ステージ1（夜）のBGM</summary>
        bgm_stage_vol8,
        /// <summary>ステージ2（夜）のBGM</summary>
        bgm_stage_vol9,
        /// <summary>ステージ3（夜）のBGM</summary>
        bgm_stage_vol10,
        /// <summary>ステージ4（夜）のBGM</summary>
        bgm_stage_vol11,
        /// <summary>ステージ5（夜）のBGM</summary>
        bgm_stage_vol12,
        /// <summary>ステージ6（夜）のBGM</summary>
        bgm_stage_vol13,
        /// <summary>ステージ7（夜）のBGM</summary>
        bgm_stage_vol14,
        /// <summary>ボスのBGM</summary>
        bgm_stage_vol15,
    }
}
