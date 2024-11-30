using Main.Model;
using Main.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Test.Driver
{
    public class PauseViewTest : MonoBehaviour
    {
        [SerializeField] private PauseView pauseView;

        private void Reset()
        {
            pauseView = GameObject.FindObjectOfType<PauseView>();
        }

        private void OnGUI()
        {
            // ボタンの配置やサイズを決定（x, y, width, height）
            if (GUI.Button(new Rect(10, 10, 100, 50), "DemoTrue"))
            {
                pauseView.SetControllEnabled(true);
            }

            if (GUI.Button(new Rect(10, 70, 100, 50), "DemoPause"))
            {
                pauseView.SetControllEnabled(false);
            }

            if (GUI.Button(new Rect(10, 130, 100, 50), "Empty"))
            {
            }
        }
    }
}
