using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Model;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class ClearCountdownTimerSystemModelTest : MonoBehaviour
    {
        [SerializeField] private ClearCountdownTimerSystemModel clearCountdownTimerSystemModel;
        private void Reset()
        {
            clearCountdownTimerSystemModel = GameObject.Find("ClearCountdownTimerSystem").GetComponent<ClearCountdownTimerSystemModel>();
        }
        private void OnGUI()
        {
            if (GUI.Button(new Rect(20,40,80,20), $"{BossDirectionPhase.Wait}"))
                Observable.FromCoroutine<bool>(observer => clearCountdownTimerSystemModel.SetIsTimeOut(observer, (int)BossDirectionPhase.Wait))
                    .Subscribe(_ => {})
                    .AddTo(gameObject);
            if (GUI.Button(new Rect(20,70,80,20), $"{BossDirectionPhase.Entrance}"))
                Observable.FromCoroutine<bool>(observer => clearCountdownTimerSystemModel.SetIsTimeOut(observer, (int)BossDirectionPhase.Entrance))
                    .Subscribe(_ => {})
                    .AddTo(gameObject);
            if (GUI.Button(new Rect(20,100,80,20), $"{BossDirectionPhase.Exit}"))
                Observable.FromCoroutine<bool>(observer => clearCountdownTimerSystemModel.SetIsTimeOut(observer, (int)BossDirectionPhase.Exit))
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
