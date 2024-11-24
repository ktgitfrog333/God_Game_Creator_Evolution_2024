using Main.Common;
using Main.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Test.Driver
{
    public class GuideMessageTest : MonoBehaviour
    {
        [SerializeField] private GuideMessageView guideMessageView;
        [SerializeField] private bool isButtonEnabled;
        [SerializeField] private MissionID missionID;
        [SerializeField] private int killedEnemyCount;
        [SerializeField] private int killedEnemyCountMax;

        private void Reset()
        {
            guideMessageView = GameObject.Find("SayDialog").GetComponent<GuideMessageView>();
        }

        private void OnGUI()
        {
            // ボタンの配置やサイズを決定（x, y, width, height）
            if (GUI.Button(new Rect(10, 10, 100, 50), "SetButtonEnabled"))
            {
                guideMessageView.SetButtonEnabled(isButtonEnabled);
            }

            if (GUI.Button(new Rect(10, 70, 100, 50), "UpdateSentence"))
            {
                guideMessageView.UpdateSentence(missionID, killedEnemyCount, killedEnemyCountMax);
            }

            if (GUI.Button(new Rect(10, 130, 100, 50), "UpdateSentence"))
            {
            }
        }
    }
}
