using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Common;
using UnityEngine.Audio;
using Universal.Template;
using Universal.Bean;
using Universal.Common;

namespace Title.Audio
{
    /// <summary>
    /// オーディオミキサー
    /// </summary>
    [RequireComponent(typeof(AudioMixer))]
    public class AudioMixerController : MonoBehaviour, ITitleGameManager, IAudioMixerController
    {
        /// <summary>ミキサー</summary>
        [SerializeField] private AudioMixer audioMixer;
        /// <summary>音量調整の間隔</summary>
        [SerializeField] private float volumeSpan = 10f;

        public void OnStart()
        {
            var temp = new TemplateResourcesAccessory();
            var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            if (!OutPutAudios(datas.bgmVolumeIndex, ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
            if (!OutPutAudios(datas.seVolumeIndex, ConstAudioMixerGroupsNames.GROUP_NAME_SE))
                Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_SE}設定呼び出しの失敗");
        }

        public bool SetVolume(UserBean userBean)
        {
            try
            {
                if (!OutPutAudios(userBean.bgmVolumeIndex, ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
                if (!OutPutAudios(userBean.seVolumeIndex, ConstAudioMixerGroupsNames.GROUP_NAME_SE))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_SE}設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool ReLoadAudios()
        {
            try
            {
                var temp = new TemplateResourcesAccessory();
                var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
                if (!OutPutAudios(datas.bgmVolumeIndex, ConstAudioMixerGroupsNames.GROUP_NAME_BGM))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_BGM}設定呼び出しの失敗");
                if (!OutPutAudios(datas.seVolumeIndex, ConstAudioMixerGroupsNames.GROUP_NAME_SE))
                    Debug.LogError($"{ConstAudioMixerGroupsNames.GROUP_NAME_SE}設定呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// オーディオ情報を保存
        /// </summary>
        /// <param name="userBean">ユーザー情報</param>
        /// <returns>成功／失敗</returns>
        public bool SaveAudios(UserBean userBean)
        {
            try
            {
                var tTResources = new TemplateResourcesAccessory();
                if (!tTResources.SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, userBean))
                    Debug.LogError("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// ミキサーへ反映
        /// </summary>
        /// <param name="value">音量の値</param>
        /// <param name="groupsName">オーディオグループ名</param>
        /// <returns>成功／失敗</returns>
        private bool OutPutAudios(float value, string groupsName)
        {
            try
            {
                //x段階補正
                value /= volumeSpan;
                //-80~0に変換
                var volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f);
                //audioMixerに代入
                audioMixer.SetFloat(groupsName, volume);

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
    /// オーディオミキサー
    /// インターフェース
    /// </summary>
    public interface IAudioMixerController
    {
        /// <summary>
        /// ボリュームをセット
        /// </summary>
        /// <param name="userBean">ユーザー情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetVolume(UserBean userBean);
        /// <summary>
        /// オーディオ情報をリロード
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ReLoadAudios();
    }
}
