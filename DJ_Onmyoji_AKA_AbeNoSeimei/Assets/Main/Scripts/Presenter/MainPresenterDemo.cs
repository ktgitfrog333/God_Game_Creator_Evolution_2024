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

namespace Main.Presenter
{
    public class MainPresenterDemo : MonoBehaviour, IMainGameManager
    {
        [SerializeField] private PentagramSystemModel pentagramSystemModel;
        [SerializeField] private PentagramTurnTableView pentagramTurnTableView;
        public Demo _demo = new Demo();
        public int ObserveEveryValueChangedCnt {get; private set;}
        [SerializeField] private ClearCountdownTimerSystemModel clearCountdownTimerSystemModel;
        [SerializeField] private ClearCountdownTimerCircleView clearCountdownTimerCircleView;
        [SerializeField] private ClearCountdownTimerCircleView clearCountdownTimerCircleView_1;
        [SerializeField] private ClearCountdownTimerGaugeView clearCountdownTimerGaugeView;
        [SerializeField] private ClearCountdownTimerTextView clearCountdownTimerTextView;
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private EnemyModel enemyModel;
        [SerializeField] private SunMoonSystemModel sunMoonSystemModel;
        [SerializeField] private SunMoonStateIconView sunMoonStateIconView;
        [SerializeField] private SunMoonStateIconViewDemo sunMoonStateIconViewDemo;
        public void OnStart()
        {
            // playerModel.IsInstanced.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (x)
            //             if (!pentagramTurnTableView.CalibrationToTarget(playerModel.transform))
            //                 Debug.LogError("CalibrationToTarget");
            //     });
            // playerModel.IsDead.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (x)
            //         {
            //             Debug.Log("プレイヤーの死亡");
            //         }
            //     });
            // playerModel.IsHit.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (x)
            //             Debug.Log("ヒット_無敵中");
            //         else
            //             Debug.Log("無敵リセット");
            //     });

            // IClearCountdownTimerViewAdapter circleView = new ClearCountdownTimerCircleViewAdapter(clearCountdownTimerCircleView);
            // playerModel.State.HP.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!circleView.Set(x, playerModel.State.HPMax))
            //             Debug.LogError("Set");
            //     });
            // IClearCountdownTimerViewAdapter circleView_1 = new ClearCountdownTimerCircleViewAdapter(clearCountdownTimerCircleView_1);
            // enemyModel.State.HP.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!circleView_1.Set(x, enemyModel.State.HPMax))
            //             Debug.LogError("Set");
            //     });
            // IClearCountdownTimerViewAdapter gaugeView = new ClearCountdownTimerGaugeViewAdapter(clearCountdownTimerGaugeView);
            // IClearCountdownTimerViewAdapter textView = new ClearCountdownTimerTextViewAdapter(clearCountdownTimerTextView);

            // clearCountdownTimerSystemModel.TimeSec.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!circleView.Set(x, clearCountdownTimerSystemModel.LimitTimeSecMax))
            //             Debug.LogError("SetAngle");
            //         if (!gaugeView.Set(x, clearCountdownTimerSystemModel.LimitTimeSecMax))
            //             Debug.LogError("SetHorizontal");
            //         if (!textView.Set(x, clearCountdownTimerSystemModel.LimitTimeSecMax))
            //             Debug.LogError("SetTextImport");
            //     });
            // clearCountdownTimerSystemModel.IsTimeOut.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (x)
            //         {
            //             if (!clearCountdownTimerSystemModel.isActiveAndEnabled)
            //                 clearCountdownTimerSystemModel.enabled = false;
            //             if (!circleView.Set(0f, clearCountdownTimerSystemModel.LimitTimeSecMax))
            //                 Debug.LogError("SetAngle");
            //             if (!gaugeView.Set(0f, clearCountdownTimerSystemModel.LimitTimeSecMax))
            //                 Debug.LogError("SetHorizontal");
            //             if (!textView.Set(0f, clearCountdownTimerSystemModel.LimitTimeSecMax))
            //                 Debug.LogError("SetTextImport");
            //         }
            //     });

            // var inputValues = new List<float>();
            // BgmConfDetails bgmConfDetails = new BgmConfDetails();
            // pentagramSystemModel.InputValue.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         ObserveEveryValueChangedCnt++;
            //         bgmConfDetails.InputValue = x;
            //         if (!pentagramTurnTableView.MoveSpin(bgmConfDetails))
            //             Debug.LogError("MoveSpin");
            //     });
            sunMoonSystemModel.OnmyoState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x => sunMoonStateIconView.SetRotate(x));
            // sunMoonStateIconViewDemo.OnmyoState.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         Debug.Log($"入力値:[{x}]");
            //         var result = sunMoonStateIconView.SetRotate(x);
            //         Debug.Log($"角度：[{result}]");
            //     });
        }

        private void Reset()
        {
            // pentagramSystemModel = GameObject.Find("PentagramSystem").GetComponent<PentagramSystemModel>();
            // pentagramTurnTableView = GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableView>();
            // clearCountdownTimerSystemModel = GameObject.Find("ClearCountdownTimerSystem").GetComponent<ClearCountdownTimerSystemModel>();
            // clearCountdownTimerCircleView = GameObject.Find("ClearCountdownTimerCircle").GetComponent<ClearCountdownTimerCircleView>();
            // clearCountdownTimerCircleView_1 = GameObject.Find("ClearCountdownTimerCircle").GetComponent<ClearCountdownTimerCircleView>();
            // clearCountdownTimerGaugeView = GameObject.Find("ClearCountdownTimerGauge").GetComponent<ClearCountdownTimerGaugeView>();
            // clearCountdownTimerTextView = GameObject.Find("ClearCountdownTimerText").GetComponent<ClearCountdownTimerTextView>();
            // playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();
            // onmyoTurretModel = GameObject.Find("OnmyoTurret").GetComponent<OnmyoTurretModel>();
            // enemyModel = GameObject.Find("Enemy").GetComponent<EnemyModel>();
            sunMoonSystemModel = GameObject.Find("SunMoonSystem").GetComponent<SunMoonSystemModel>();
            sunMoonStateIconView = GameObject.Find("SunMoonStateIcon").GetComponent<SunMoonStateIconView>();
            sunMoonStateIconViewDemo = GetComponent<SunMoonStateIconViewDemo>();
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

