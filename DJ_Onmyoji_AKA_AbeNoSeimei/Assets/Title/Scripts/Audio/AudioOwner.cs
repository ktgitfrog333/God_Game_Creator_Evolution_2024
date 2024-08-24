using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Common;
using Universal.Template;
using Universal.Bean;
using Universal.Common;

namespace Title.Audio
{
    /// <summary>
    /// オーディオのオーナー
    /// </summary>
    public class AudioOwner : MonoBehaviour, ITitleGameManager, ISfxPlayer, IBgmPlayer
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
        }

        public void PlaySFX(ClipToPlay clipToPlay)
        {
            sfxPlayer.PlaySFX(clipToPlay);
        }

        public void PlayBGM(ClipToPlayBGM clipToPlay)
        {
            bgmPlayer.PlayBGM(clipToPlay);
        }

        /// <summary>
        /// ボリュームをセット
        /// </summary>
        /// <param name="userBean">ユーザー情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetVolume(UserBean userBean)
        {
            return audioMixer.SetVolume(userBean);
        }

        /// <summary>
        /// オーディオ情報をリロード
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ReLoadAudios()
        {
            return audioMixer.ReLoadAudios();
        }

        /// <summary>
        /// オーディオ情報を保存
        /// </summary>
        /// <param name="userBean">ユーザー情報</param>
        /// <returns>成功／失敗</returns>
        public bool SaveAudios(UserBean userBean)
        {
            return audioMixer.SaveAudios(userBean);
        }

        /// <summary>
        /// システム設定データの削除
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool DestroySystemConfig()
        {
            try
            {
                var temp = new TemplateResourcesAccessory();
                var bean = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
                var beanDefault = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, EnumLoadMode.Default);
                var beanUpdate = temp.UpdateAudioAndVibration(bean, beanDefault);
                if (beanUpdate == null)
                    throw new System.Exception("システム設定データ更新の失敗");
                if (!temp.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, beanUpdate))
                    throw new System.Exception("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
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
    }

    /// <summary>
    /// SFX・MEオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlay
    {
        /// <summary>キャンセル</summary>
        se_cancel,
        /// <summary>項目の決定</summary>
        se_decided,
        /// <summary>ゲームスタート</summary>
        se_game_start,
        /// <summary>ステージセレクト</summary>
        se_select,
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
    }

    /// <summary>
    /// BGMオーディオクリップリストのインデックス
    /// </summary>
    public enum ClipToPlayBGM
    {
        /// <summary>タイトル</summary>
        bgm_title,
    }
}
