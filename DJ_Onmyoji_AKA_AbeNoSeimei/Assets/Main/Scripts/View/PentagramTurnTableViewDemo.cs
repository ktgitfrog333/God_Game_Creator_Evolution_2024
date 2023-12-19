using System.Collections;
using System.Collections.Generic;
using Main.Audio;
using Main.Model;
using Main.Presenter;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    public class PentagramTurnTableViewDemo : MonoBehaviour
    {
        [SerializeField] private PentagramTurnTableView pentagramTurnTableView;
        [SerializeField] private PentagramSystemModel pentagramSystemModel;
        [SerializeField] private MainPresenterDemo mainPresenterDemo;
        private void Reset()
        {
            pentagramTurnTableView = GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableView>();
            pentagramSystemModel = GameObject.Find("PentagramSystem").GetComponent<PentagramSystemModel>();
            mainPresenterDemo = GameObject.Find("MainGameManager").GetComponent<MainPresenterDemo>();
        }

        [SerializeField] private int caseNo = 0;
        [SerializeField] private Text text;
        public void OnSliderValueChanged(float value)
        {
            // Debug.Log(value);
            Cases(value);
        }
        private void Cases(float value=0f)
        {
            switch (caseNo)
            {
                case 0:
                    if (value != 0f)
                        pentagramTurnTableView.AngleCorrectionValue = value;
                    text.text = $"{pentagramTurnTableView.AngleCorrectionValue}";
                    break;
                case 1:
                    axis = value;
                    break;
                case 2:
                    // text = GetComponent<Text>();
                    text.text = $"Update:\r\n[{pentagramSystemModel.UpdateCnt}]";
                    break;
                case 3:
                    // text = GetComponent<Text>();
                    text.text = $"ObserveEveryValueChangedCnt:\r\n[{mainPresenterDemo.ObserveEveryValueChangedCnt}]";
                    break;
                case 4:
                    // text = GetComponent<Text>();
                    text.text = $"fps:\r\n[{1f / Time.deltaTime}]";
                    break;
                case 5:
                    // text = GetComponent<Text>();
                    if (value != 0f)
                    {
                        float[] durations = {value};
                        // pentagramTurnTableView.Durations = durations;
                    }
                    // text.text = $"durations:\r\n[{string.Join(", ", pentagramTurnTableView.Durations)}]";
                    break;
                default:
                    break;
            }
        }
        [SerializeField, Range(-1f, 1f)] private float axis = 0f;
        private void Update()
        {
            Cases();
            // var detal = new BgmConfDetails
            // {
            //     InputValue = axis
            // };
            // pentagramTurnTableView.MoveSpin(detal);
        }
    }
}
