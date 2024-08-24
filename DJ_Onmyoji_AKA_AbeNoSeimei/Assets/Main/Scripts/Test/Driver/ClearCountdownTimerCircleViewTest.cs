using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Main.View;
using Main.Common;

namespace Main.Test.Driver
{
    public class ClearCountdownTimerCircleViewTest : MonoBehaviour
    {
        private void OnEnable()
        {
            View.ClearCountdownTimerCircleView clearCountdownTimerCircleView = GameObject.Find("SunMoonStateCircleGauge").GetComponent<View.ClearCountdownTimerCircleView>();
            clearCountdownTimerCircleView.SetAngle(0f, 1f);
            Observable.FromCoroutine<bool>(observer => clearCountdownTimerCircleView.PlayRepairAngleAnimation(observer, (int)IsTimeOutState.Infinite))
                .Subscribe(_ => {})
                .AddTo(gameObject);
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
