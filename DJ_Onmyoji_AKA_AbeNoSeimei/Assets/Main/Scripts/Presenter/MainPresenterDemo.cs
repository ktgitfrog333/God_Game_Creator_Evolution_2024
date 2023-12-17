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

namespace Main.Presenter
{
    public class MainPresenterDemo : MonoBehaviour, IMainGameManager
    {
        [SerializeField] private PentagramSystemModel pentagramSystemModel;
        [SerializeField] private PentagramTurnTableView pentagramTurnTableView;
        public Demo _demo = new Demo();
        public int ObserveEveryValueChangedCnt {get; private set;}
        public void OnStart()
        {
            var inputValues = new List<float>();
            BgmConfDetails bgmConfDetails = new BgmConfDetails();
            pentagramSystemModel.InputValue.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // _demo.inputValue = Mathf.Abs(x);
                    // _demo.isOver = 1f <= _demo.inputValue;
                    // inputValues.Add(_demo.inputValue);
                    // _demo.inputValuesAverage = inputValues.TakeLast(10).Average();
                    // if (_demo.isOver)
                    //     _demo.overCnt++;
                    
                    // Debug.Log($"ObserveEveryValueChanged");
                    ObserveEveryValueChangedCnt++;
                    bgmConfDetails.InputValue = x;
                    if (!pentagramTurnTableView.MoveSpin(bgmConfDetails))
                        Debug.LogError("MoveSpin");
                });
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    // Debug.Log($"ObserveEveryValueChanged");
                    // bgmConfDetails.InputValue = pentagramSystemModel.InputValue.Value;
                    // if (!pentagramTurnTableView.MoveSpin(bgmConfDetails))
                    //     Debug.LogError("MoveSpin");
                });
        }

        private void Reset()
        {
            pentagramSystemModel = GameObject.Find("PentagramSystem").GetComponent<PentagramSystemModel>();
            pentagramTurnTableView = GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableView>();
        }
    }

    [System.Serializable]
    public struct Demo
    {
        [Range(0f, 1f)] public float inputValue;
        public float inputValuesAverage;
        public bool isOver;
        public int overCnt;
        // [Range(-360f, 360f)] public float angle;
    }
}
