using Main.Common;
using Main.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// FungusのSayDialogを管理
    /// ビュー
    /// </summary>
    public class GuideMessageView : MonoBehaviour, IGuideMessageView, IButtonEventTriggerModel
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool SetButtonEnabled(bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateSentence(MissionID missionID)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// FungusのSayDialogを管理
    /// インターフェース
    /// </summary>
    public interface IGuideMessageView
    {
        /// <summary>
        /// ミッション用に表示文言を変更する
        /// </summary>
        /// <param name="missionID">ミッションID</param>
        /// <returns>成功／失敗</returns>
        public bool UpdateSentence(MissionID missionID);
    }
}
