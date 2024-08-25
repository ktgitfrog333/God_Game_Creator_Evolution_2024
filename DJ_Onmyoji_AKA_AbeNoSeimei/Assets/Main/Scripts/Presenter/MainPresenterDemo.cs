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
        [SerializeField] private EnemiesSpawnModel enemiesSpawnModel;
        [SerializeField] private FadersGroupView fadersGroupView;
        [SerializeField] private FadersGroupViewTest fadersGroupViewTest;
        [SerializeField] private SpawnSoulMoneyModel spawnSoulMoneyModel;
        [SerializeField] private SoulWalletModel soulWalletModel;
        [SerializeField] private ClearView clearView;
        [SerializeField] private ClearViewTest clearViewTest;
        [SerializeField] private PentagramTurnTableOnModalView pentagramTurnTableOnModalView;
        [SerializeField] private RewardSelectViewTest rewardSelectViewTest;
        [SerializeField] private RewardSelectView rewardSelectView;
        [SerializeField] private CursorIconTest cursorIconTest;
        [SerializeField] private CursorIconView cursorIconView;
        [SerializeField] private RewardSelectModel rewardSelectModel;
        [SerializeField] private RewardSelectModelTest rewardSelectModelTest;
        [SerializeField] private GameProceedButtonModel gameProceedButtonModel;
        [SerializeField] private GameRetryButtonModel gameRetryButtonModel;
        [SerializeField] private GameSelectButtonModel gameSelectButtonModel;
        [SerializeField] private SoulWalletModelTeest soulWalletModelTeest;

        public void OnStart()
        {
            gameSelectButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            break;
                        case EnumEventCommand.Selected:
                            // 選択された時の処理
                            if (!cursorIconView.SetSelectAndScale(gameSelectButtonModel.transform.position, (gameSelectButtonModel.transform as RectTransform).sizeDelta))
                                Debug.LogError("SetSelectAndScale");

                            break;
                        case EnumEventCommand.DeSelected:
                            // 選択解除された時の処理
                            break;
                        case EnumEventCommand.Submited:
                            // 実行された時の処理
                            break;
                        case EnumEventCommand.Canceled:
                            // キャンセルされた時の処理
                            break;
                        default:
                            break;
                    }
                });
            rewardSelectModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (!rewardSelectView.SetContents(rewardSelectModel.RewardContentProps))
                        Debug.LogError("SetContents");
                });
            soulWalletModel.SoulMoney.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (!rewardSelectView.SetContents(new ClearRewardContentsState()
                    {
                        soulMoney = x,
                    }))
                        Debug.LogError("SetContents");
                    if (!rewardSelectModel.DiffCostVsResorceAndDisabled(new ClearRewardContentsState()
                    {
                        soulMoney = x,
                    }))
                        Debug.LogError("DiffCostVsResorceAndDisabled");
                    // if (!rewardSelectView.UpdateCheckState(new ClearRewardContentsState()
                    // {
                    //     soulMoney = x,
                    // }, rewardSelectModel.RewardContentModels.Select(q => q.RewardContentProp).ToArray()))
                    //     Debug.LogError("UpdateCheckState");
                });
            // *** 動作確認用 ***
            DOVirtual.DelayedCall(0.5f, () => rewardSelectModel.RewardContentModels[0].SetSelectedGameObject());
            foreach (var item in rewardSelectModel.RewardContentModels.Select((p, i) => new { Content = p, Index = i}))
            {
                item.Content.CheckState.ObserveEveryValueChanged(x => x.Value)
                    .Pairwise()
                    .Subscribe(pair =>
                    {
                        switch ((CheckState)pair.Current)
                        {
                            case CheckState.UnCheck:
                                // Check⇒UnCheckの場合は金額計算を行う
                                if (pair.Previous == (int)CheckState.Check)
                                {
                                    if (soulWalletModel.AddSoulMoney(1 * item.Content.RewardContentProp.soulMoney) < 0)
                                        Debug.LogError("AddSoulMoney");
                                    if (!MainGameManager.Instance.LevelOwner.AddRewardID(item.Index, true))
                                        Debug.LogError("AddRewardID");
                                }
                                // Disabled⇒UnCheckの場合は金額計算を行わない
                                else if (pair.Previous == (int)CheckState.Disabled)
                                {

                                }
                                if (!rewardSelectView.UnCheck(item.Index))
                                    Debug.LogError("UnCheck");

                                break;
                            case CheckState.Check:
                                if (soulWalletModel.AddSoulMoney(-1 * item.Content.RewardContentProp.soulMoney) < 0)
                                    Debug.LogError("AddSoulMoney");
                                if (!rewardSelectView.Check(item.Index))
                                    Debug.LogError("Check");
                                if (!MainGameManager.Instance.LevelOwner.AddRewardID(item.Index))
                                    Debug.LogError("AddRewardID");

                                break;
                            case CheckState.Disabled:
                                if (!rewardSelectView.Disabled(item.Index))
                                    Debug.LogError("Disabled");

                                break;
                            default:
                                break;
                        }
                    });
                item.Content.EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                // 処理無し
                                break;
                            case EnumEventCommand.Selected:
                                if (!rewardSelectView.SetContents(item.Content.RewardContentProp))
                                    Debug.LogError("SetContents");
                                if (!rewardSelectView.ScaleUp(item.Index))
                                    Debug.LogError("ScaleUp");
                                if (!cursorIconView.SetSelectAndScale(item.Content.transform.position, item.Content.CurrentSizeDelta))
                                    Debug.LogError("SetSelectAndScale");

                                break;
                            case EnumEventCommand.DeSelected:
                                if (!rewardSelectView.ScaleDown(item.Index))
                                    Debug.LogError("ScaleDown");

                                break;
                            case EnumEventCommand.Submited:
                                if (!rewardSelectModel.Check(item.Index))
                                    Debug.LogError("Check");

                                break;
                            case EnumEventCommand.Canceled:
                                if (!rewardSelectModel.UnCheck(item.Index))
                                    Debug.LogError("UnCheck");

                                break;
                            default:
                                // 処理無し
                                break;
                        }
                    });
            }
            // クリア画面 -> ステージ選択画面へ戻る
            gameSelectButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:

                            break;
                        case EnumEventCommand.DeSelected:
                            break;
                        case EnumEventCommand.Submited:
                            if (!MainGameManager.Instance.LevelOwner.SetSlots())
                                Debug.LogError("SetSlots");
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });

            // cursorIconTest.UpdateAsObservable()
            //     .Select(x => cursorIconTest.Position.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!cursorIconView.SetSelectAndScale(x, cursorIconTest.SizeDelta))
            //             Debug.LogError("SetSelectAndScale");

            //     });
            // cursorIconTest.Position.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!cursorIconView.SetSelect(x))
            //             Debug.LogError("SetSelect");
            //     });
            // rewardSelectViewTest.SoulMoney.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!rewardSelectView.SetContents(new ClearRewardContentsState()
            //         {
            //             soulMoney = x,
            //         }))
            //             Debug.LogError("SetContents");
            //     });
            // rewardSelectViewTest.UpdateAsObservable()
            //     .Select(x => rewardSelectViewTest.RewardContentProps)
            //     .Where(x => x != null)
            //     .Take(1)
            //     .Subscribe(x =>
            //     {
            //         if (!rewardSelectView.SetContents(x))
            //             Debug.LogError("SetContents");
            //     });
            // rewardSelectViewTest.UpdateAsObservable()
            //     .Select(x => rewardSelectViewTest.ClearRewardType)
            //     .Subscribe(x =>
            //     {
            //         if (!rewardSelectView.SetContents(x))
            //             Debug.LogError("SetContents");
            //     });
            // rewardSelectViewTest.IsScaleUpIndex.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!rewardSelectView.ScaleUp(x))
            //             Debug.LogError("ScaleUp");
            //     });
            // rewardSelectViewTest.IsScaleDownIndex.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!rewardSelectView.ScaleDown(x))
            //             Debug.LogError("ScaleDown");
            //     });
            // rewardSelectViewTest.CheckIndex.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!rewardSelectView.Check(x))
            //             Debug.LogError("Check");
            //     });
            // rewardSelectViewTest.UnCheckIndex.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!rewardSelectView.UnCheck(x))
            //             Debug.LogError("UnCheck");
            //     });
            // clearViewTest.TimeSec.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!clearView.SetContents(new ClearResultContentsState()
            //         {
            //             timeSec = x,
            //         }))
            //             Debug.LogError("SetContents");
            //     });
            // clearViewTest.SoulMoney.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         if (!clearView.SetContents(new ClearResultContentsState()
            //         {
            //             soulMoney = x,
            //         }))
            //             Debug.LogError("SetContents");
            //     });
            // _demo.types = new ShikigamiType[3];
            // _demo.tempoLevels = new float[3];
            // foreach (var item in shikigamiSkillSystemModel.ShikigamiInfos.Select((p, i) => new { Content = p, Index = i }))
            //     item.Content.state.tempoLevel.ObserveEveryValueChanged(x => x.Value)
            //         .Subscribe(x =>
            //         {
            //             foreach (var faderUniversalView in faderUniversalViews)
            //             {
            //                 if (!faderUniversalView.SetSliderValue(x, item.Content.prop.type))
            //                     Debug.LogError("SetSliderValue");
            //             }
            //             if (!pentagramTurnTableModel.UpdateTempoLvValues(x, item.Content.prop.type))
            //                 Debug.LogError("UpdateTempoLvValues");
            //         });
            // shikigamiSkillSystemModel.CandleInfo.CandleResource.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            // _demo.candleResource = x;
            // Debug.Log($"CandleResource:[{x}]");
            // if (!spGaugeView.SetVertical(x, shikigamiSkillSystemModel.CandleInfo.LimitCandleResorceMax))
            //     Debug.LogError("SetVertical");
            // });
            // shikigamiSkillSystemModel.CandleInfo.IsOutCost.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            // _demo.isOutCost = x;
            // Debug.Log($"IsOutCost:[{x}]");
            // });
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
            //enemyModel.State.IsDead.ObserveEveryValueChanged(x => x.Value)
            //    .Subscribe(x =>
            //    {
            //        if (x)
            //        {
            //            if (!spawnSoulMoneyModel.InstanceCloneObjects(enemyModel.transform.position, enemyModel.EnemiesProp))
            //                Debug.LogError("InstanceCloneObjects");
            //        }
            //    });
            // SpawnSoulMoneyModelのOnSoulMoneyGetedを購読
            //spawnSoulMoneyModel.OnSoulMoneyGeted
            //    .Subscribe(soulMoney =>
            //    {
            //        if (soulMoney.IsGeted.Value)
            //        {
            //            var soulMoneyPoint = soulWalletModel.AddSoulMoney(soulMoney.EnemiesProp.soulMoneyPoint);
            //            if (soulMoneyPoint < 0)
            //                Debug.LogError("AddSoulMoney");
            //        }
            //        // ここでsoulMoney.IsGetedの変更に応じた処理を行う
            //        // Debug.Log($"SoulMoney Geted: {soulMoney.IsGeted}");
            //    })
            //    .AddTo(gameObject); // UniRxのAddToを使用して、このGameObjectが破棄されたときに購読を自動的に解除
            //soulWalletModel.SoulMoney.ObserveEveryValueChanged(x => x.Value)
            //    .Subscribe(x =>
            //    {
            //        Debug.Log($"経験値：[{x}]");
            //    });
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
            // ObserveEveryValueChangedCnt++;
            // bgmConfDetails.InputValue = x;
            // if (!pentagramTurnTableView.MoveSpin(bgmConfDetails))
            //     Debug.LogError("MoveSpin");
            // });
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
            //     .Subscribe(x =>
            //     {
            //         // sunMoonStateIconView.SetRotate(x);
            //         if (!enemiesSpawnModel.SetOnmyoState(x))
            //             Debug.LogError("SetOnmyoState");
            //     });
            // sunMoonStateIconViewDemo.OnmyoState.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         Debug.Log($"入力値:[{x}]");
            //         var result = sunMoonStateIconView.SetRotate(x);
            //         Debug.Log($"角度：[{result}]");
            //     });
            // fadersGroupViewTest.IsOpen.ObserveEveryValueChanged(x => x.Value)
            //     .Subscribe(x =>
            //     {
            //         Observable.FromCoroutine<bool>(observer => fadersGroupView.PlayMoveAnchorsBasedOnHeight(observer, EnumFadeState.Open))
            //             .Subscribe(_ => { })
            //             .AddTo(gameObject);
            //     });
        }

        private void Reset()
        {
            gameSelectButtonModel = GameObject.Find("GameSelectButton").GetComponent<GameSelectButtonModel>();
            rewardSelectModel = GameObject.Find("RewardSelect").GetComponent<RewardSelectModel>();
            cursorIconView = GameObject.Find("CursorIcon").GetComponent<CursorIconView>();
            // cursorIconTest = GameObject.Find("CursorIconTest").GetComponent<CursorIconTest>();
            rewardSelectView = GameObject.Find("RewardSelect").GetComponent<RewardSelectView>();
            // rewardSelectViewTest = GameObject.Find("RewardSelectViewTest").GetComponent<RewardSelectViewTest>();
            // pentagramTurnTableOnModalView = GameObject.Find("PentagramTurnTableOnModal").GetComponent<PentagramTurnTableOnModalView>();
            // clearViewTest = GameObject.Find("ClearViewTest").GetComponent<ClearViewTest>();
            // clearView = GameObject.Find("Clear").GetComponent<ClearView>();
            soulWalletModel = GameObject.Find("SoulWallet").GetComponent<SoulWalletModel>();
            // spawnSoulMoneyModel = GameObject.Find("SpawnSoulMoney").GetComponent<SpawnSoulMoneyModel>();
            // fadersGroupViewTest = GameObject.Find("FadersGroupViewTest").GetComponent<FadersGroupViewTest>();
            // fadersGroupView = GameObject.Find("FadersGroup").GetComponent<FadersGroupView>();
            // enemiesSpawnModel = GameObject.Find("EnemiesSpawn").GetComponent<EnemiesSpawnModel>();
            // pentagramTurnTableModel = GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableModel>();
            // spGaugeView = GameObject.Find("SpGauge").GetComponent<SpGaugeView>();
            // faderUniversalViews = new FaderUniversalView[]
            // {
            //     GameObject.Find($"Fader{ShikigamiType.Wrap}").GetComponent<FaderUniversalView>(),
            //     GameObject.Find($"Fader{ShikigamiType.Dance}").GetComponent<FaderUniversalView>(),
            //     GameObject.Find($"Fader{ShikigamiType.Graffiti}").GetComponent<FaderUniversalView>(),
            // };
            // shikigamiSkillSystemModel = GameObject.Find("ShikigamiSkillSystem").GetComponent<ShikigamiSkillSystemModel>();
            // pentagramSystemModel = GameObject.Find("PentagramSystem").GetComponent<PentagramSystemModel>();
            // pentagramTurnTableView = GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableView>();
            // clearCountdownTimerSystemModel = GameObject.Find("ClearCountdownTimerSystem").GetComponent<ClearCountdownTimerSystemModel>();
            // clearCountdownTimerCircleView = GameObject.Find("ClearCountdownTimerCircle").GetComponent<ClearCountdownTimerCircleView>();
            // clearCountdownTimerCircleView_1 = GameObject.Find("ClearCountdownTimerCircle").GetComponent<ClearCountdownTimerCircleView>();
            // clearCountdownTimerGaugeView = GameObject.Find("ClearCountdownTimerGauge").GetComponent<ClearCountdownTimerGaugeView>();
            // clearCountdownTimerTextView = GameObject.Find("ClearCountdownTimerText").GetComponent<ClearCountdownTimerTextView>();
            // playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();
            // onmyoTurretModel = GameObject.Find("OnmyoTurret").GetComponent<OnmyoTurretModel>();
            // enemyModel = GameObject.Find("EnemyA").GetComponent<EnemyModel>();
            // sunMoonSystemModel = GameObject.Find("SunMoonSystem").GetComponent<SunMoonSystemModel>();
            // sunMoonStateIconView = GameObject.Find("SunMoonStateIcon").GetComponent<SunMoonStateIconView>();
            // sunMoonStateIconViewDemo = GameObject.Find("SunMoonStateIconViewDemo").GetComponent<SunMoonStateIconViewDemo>();
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

