using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Model
{
    public class ClearCountdownTimerSystemModelDemo : MonoBehaviour
    {
        [SerializeField] private ClearCountdownTimerSystemModel clearCountdownTimerSystemModel;
        [SerializeField] private Text text;
        [SerializeField] private Button button;
        [SerializeField] private TMP_InputField tMP_InputField;
        public float TMP_InputFieldValue => float.Parse(tMP_InputField.text);

        private void Reset()
        {
            clearCountdownTimerSystemModel = GameObject.Find("ClearCountdownTimerSystem").GetComponent<ClearCountdownTimerSystemModel>();
            text = GetComponent<Text>();
            button = GetComponent<Button>();
            tMP_InputField = GameObject.Find("panel_4_timer_sec").GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            // clearCountdownTimerSystemModel.ObserveEveryValueChanged(x => x.enabled)
            //     .Subscribe(x =>
            //     {
            //         if (!x)
            //         {
            //             if (clearCountdownTimerSystemModel.SetLimitTimeSecMax(5f) < 0f)
            //                 Debug.LogError("SetLimitTimeSecMax");
            //             clearCountdownTimerSystemModel.enabled = true;
            //             clearCountdownTimerSystemModel.TimeSec.ObserveEveryValueChanged(x => x.Value)
            //                 .Subscribe(x =>
            //                 {
            //                     text.text = $"{x}";
            //                 });
            //             clearCountdownTimerSystemModel.IsTimeOut.ObserveEveryValueChanged(x => x.Value)
            //                 .Subscribe(x =>
            //                 {
            //                     if (x)
            //                     {
            //                         if (clearCountdownTimerSystemModel.isActiveAndEnabled)
            //                             clearCountdownTimerSystemModel.enabled = false;
            //                         text.text = "0";
            //                     }
            //                 });
            //         }
            //     });
        }

        private bool _isOnly;
        public void StartTimer()
        {
            if (!_isOnly)
            {
                _isOnly = true;
                if (!clearCountdownTimerSystemModel.isActiveAndEnabled)
                    clearCountdownTimerSystemModel.enabled = true;
            }
        }
    }
}
