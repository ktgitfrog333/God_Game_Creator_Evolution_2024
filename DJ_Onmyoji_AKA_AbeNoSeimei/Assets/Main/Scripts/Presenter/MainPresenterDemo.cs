using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Main.Model;
using UniRx;
using Main.View;
using Main.Audio;
using UniRx.Triggers;
using Unity.Mathematics;
using System.Linq;
using Fungus;
using Unity.Collections;
using Main.Test.Driver;
using DG.Tweening;

namespace Main.Presenter
{
    public class MainPresenterDemo : MonoBehaviour, IMainGameManager
    {
        [SerializeField] private CountdownLogosView countdownLogosView;

        private void Reset()
        {
            countdownLogosView = GameObject.Find("CountdownLogos").GetComponent<CountdownLogosView>();
        }

        public void OnStart()
        {
            //// TODO: UnityのデモUIボタン押下時に実行されるように条件を追加
            //Observable.FromCoroutine<bool>(observer => countdownLogosView.PlayCountDownDirection(observer, 3))
            //    .Subscribe(_ => { })
            //    .AddTo(gameObject);
        }

        private void OnGUI()
        {
            // ボタンの配置やサイズを決定（x, y, width, height）
            if (GUI.Button(new Rect(10, 10, 100, 50), "Button 1"))
            {
                StartCountdown(1);  // ボタン1が押された場合、1を渡す
            }

            if (GUI.Button(new Rect(10, 70, 100, 50), "Button 2"))
            {
                StartCountdown(2);  // ボタン2が押された場合、2を渡す
            }

            if (GUI.Button(new Rect(10, 130, 100, 50), "Button 3"))
            {
                StartCountdown(3);  // ボタン3が押された場合、3を渡す
            }
        }

        // ボタンを押すとカウントダウンを開始する
        private void StartCountdown(int number)
        {
            Observable.FromCoroutine<bool>(observer => countdownLogosView.PlayCountDownDirection(observer, number))
                .Subscribe(_ => { })
                .AddTo(gameObject);
        }
    }
}
