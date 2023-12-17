using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Select.Common;
using Universal.Template;
using Universal.Common;

namespace Select.Audio
{
    /// <summary>
    /// オーディオミキサー
    /// </summary>
    public class AudioMixerController : MonoBehaviour, ISelectGameManager
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
}
