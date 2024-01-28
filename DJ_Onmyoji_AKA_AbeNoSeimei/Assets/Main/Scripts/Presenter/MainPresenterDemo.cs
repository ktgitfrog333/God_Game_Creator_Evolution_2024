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
        [SerializeField] private Test.ClearCountdownTimerCircleView clearCountdownTimerCircleViewTest;
        [SerializeField] private ShikigamiSkillSystemModel shikigamiSkillSystemModel;
        [SerializeField] private FaderUniversalView[] faderUniversalViews;
        [SerializeField] private SpGaugeView spGaugeView;
        [SerializeField] private PentagramTurnTableModel pentagramTurnTableModel;
        public void OnStart()
        {
            // _demo.types = new ShikigamiType[3];
            // _demo.tempoLevels = new float[3];
            foreach (var item in shikigamiSkillSystemModel.ShikigamiInfos.Select((p, i) => new { Content = p, Index = i }))
                item.Content.state.tempoLevel.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        // _demo.types[item.Index] = item.Content.prop.type;
                        // _demo.tempoLevels[item.Index] = x;
                        // Debug.Log($"{item.prop.type}:[{x}]");
                        foreach (var faderUniversalView in faderUniversalViews)
                        {
                            if (!faderUniversalView.SetSliderValue(x, item.Content.prop.type))
                                Debug.LogError("SetSliderValue");
                        }
                        if (!pentagramTurnTableModel.UpdateTempoLvValues(x, item.Content.prop.type))
                            Debug.LogError("UpdateTempoLvValues");
                    });
            shikigamiSkillSystemModel.CandleInfo.CandleResource.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // _demo.candleResource = x;
                    // Debug.Log($"CandleResource:[{x}]");
                    if (!spGaugeView.SetVertical(x, shikigamiSkillSystemModel.CandleInfo.LimitCandleResorceMax))
                        Debug.LogError("SetVertical");
                });
            shikigamiSkillSystemModel.CandleInfo.IsOutCost.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // _demo.isOutCost = x;
                    // Debug.Log($"IsOutCost:[{x}]");
                });
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
            // clearCountdownTimerCircleViewTest.TimeSec.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!circleView.Set(x, clearCountdownTimerCircleViewTest.LimitTimeSecMax))
            //             Debug.LogError("Set");
            //     });
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
            pentagramSystemModel.InputValue.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // ObserveEveryValueChangedCnt++;
                    // bgmConfDetails.InputValue = x;
                    // if (!pentagramTurnTableView.MoveSpin(bgmConfDetails))
                    //     Debug.LogError("MoveSpin");
                });
            // pentagramSystemModel.JockeyCommandType.ObserveEveryValueChanged(x => x.Value)
            //     .Pairwise()
            //     .Subscribe(pair =>
            //     {
            //         if (!shikigamiSkillSystemModel.UpdateCandleResource((JockeyCommandType)pair.Current, (JockeyCommandType)pair.Previous))
            //             Debug.LogError("UpdateCandleResource");
            //         if (!pentagramTurnTableModel.BuffAllTurrets((JockeyCommandType)pair.Current))
            //             Debug.LogError("BuffAllTurrets");
            //         if (!shikigamiSkillSystemModel.ForceZeroAndRapidRecoveryCandleResource((JockeyCommandType)pair.Current))
            //             Debug.LogError("ForceZeroAndRapidRecoveryCandleResource");
            //     });
            // sunMoonSystemModel.OnmyoState.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x => sunMoonStateIconView.SetRotate(x));
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
            pentagramTurnTableModel = GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableModel>();
            spGaugeView = GameObject.Find("SpGauge").GetComponent<SpGaugeView>();
            faderUniversalViews = new FaderUniversalView[]
            {
                GameObject.Find($"Fader{ShikigamiType.Wrap}").GetComponent<FaderUniversalView>(),
                GameObject.Find($"Fader{ShikigamiType.Dance}").GetComponent<FaderUniversalView>(),
                GameObject.Find($"Fader{ShikigamiType.Graffiti}").GetComponent<FaderUniversalView>(),
            };
            shikigamiSkillSystemModel = GameObject.Find("ShikigamiSkillSystem").GetComponent<ShikigamiSkillSystemModel>();
            pentagramSystemModel = GameObject.Find("PentagramSystem").GetComponent<PentagramSystemModel>();
            // pentagramTurnTableView = GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableView>();
            // clearCountdownTimerSystemModel = GameObject.Find("ClearCountdownTimerSystem").GetComponent<ClearCountdownTimerSystemModel>();
            // clearCountdownTimerCircleView = GameObject.Find("ClearCountdownTimerCircle").GetComponent<ClearCountdownTimerCircleView>();
            // clearCountdownTimerCircleView_1 = GameObject.Find("ClearCountdownTimerCircle").GetComponent<ClearCountdownTimerCircleView>();
            // clearCountdownTimerGaugeView = GameObject.Find("ClearCountdownTimerGauge").GetComponent<ClearCountdownTimerGaugeView>();
            // clearCountdownTimerTextView = GameObject.Find("ClearCountdownTimerText").GetComponent<ClearCountdownTimerTextView>();
            // playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();
            // onmyoTurretModel = GameObject.Find("OnmyoTurret").GetComponent<OnmyoTurretModel>();
            // enemyModel = GameObject.Find("Enemy").GetComponent<EnemyModel>();
            // sunMoonSystemModel = GameObject.Find("SunMoonSystem").GetComponent<SunMoonSystemModel>();
            // sunMoonStateIconView = GameObject.Find("SunMoonStateIcon").GetComponent<SunMoonStateIconView>();
            // sunMoonStateIconViewDemo = GetComponent<SunMoonStateIconViewDemo>();
            // clearCountdownTimerCircleViewTest = GetComponent<Test.ClearCountdownTimerCircleView>();
        }
    }

    [System.Serializable]
    public struct Demo
    {
        // [Range(0f, 1f)] public float inputValue;
        // public float inputValuesAverage;
        // public bool isOver;
        // public int overCnt;
        // [Range(-360f, 360f)] public float angle;
        public ShikigamiType[] types;
        public float[] tempoLevels;
        public float candleResource;
        public bool isOutCost;
    }
}

