using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Title.Common;
using Universal.Template;
using Title.Audio;
using Title.Template;
using Universal.Bean;
using Universal.Common;

namespace Title.Test
{
    /// <summary>
    /// テスト用
    /// オーディオの音量調整
    /// </summary>
    public class TestTitleAudioVolume : MonoBehaviour
    {
        //[SerializeField] private Slider slider;
        [SerializeField] private TestAudioMode mode;
        private UserBean bean = new UserBean();

        private void Start()
        {
            TitleGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_title);
            //slider.onValueChanged.AddListener(SetAudioMixer);
            var tTResources = new TemplateResourcesAccessory();
            bean = tTResources.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
        }

        public void SetAudioMixer(float value)
        {
            switch (mode)
            {
                case TestAudioMode.BGM:
                    bean.bgmVolumeIndex = (int)value;
                    break;
                case TestAudioMode.SE:
                    bean.seVolumeIndex = (int)value;
                    break;
                default:
                    break;
            }
            if (!TitleGameManager.Instance.AudioOwner.SetVolume(bean))
                Debug.LogError("ボリューム調整呼び出しの失敗");
        }

        public void OnClickCancel()
        {
            if (!TitleGameManager.Instance.AudioOwner.ReLoadAudios())
                Debug.LogError("オーディオ設定キャンセル呼び出しの失敗");
        }

        public void OnClickSubmit()
        {
            if (!TitleGameManager.Instance.AudioOwner.SaveAudios(bean))
                Debug.LogError("オーディオ設定保存呼び出しの失敗");
        }

        public void OnClickSample()
        {
            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
        }

        private enum TestAudioMode
        {
            BGM,
            SE,
        }
    }
}
