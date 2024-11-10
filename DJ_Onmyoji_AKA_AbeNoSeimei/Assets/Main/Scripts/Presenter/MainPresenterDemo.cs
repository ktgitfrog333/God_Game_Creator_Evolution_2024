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
using Main.Utility;

namespace Main.Presenter
{
    public class MainPresenterDemo : MonoBehaviour, IMainGameManager
    {
        #region チュートリアル実装_3 メインシーンにてチュートリアルモード追加、会話システム追加など
        /// <summary>チュートリアルのモデル</summary>
        [SerializeField] private TutorialModel tutorialModel;
        /// <summary>チュートリアルのビュー</summary>
        [SerializeField] private TutorialView tutorialView;
        /// <summary>チュートリアル用の敵スポーン制御のモデル</summary>
        [SerializeField] private EnemiesSpawnTutorialModel enemiesSpawnTutorialModel;
        /// <summary>チュートリアルの中のミッションを管理するのモデル</summary>
        [SerializeField] private MissionsSystemTutorialModel missionsSystemTutorialModel;
        /// <summary>チュートリアルのガイドで扱うリソースの構造体</summary>
        [SerializeField] private TutorialGuideContentsStuct tutorialGuideContentsStuct;
        /// <summary>チュートリアルのミッションで扱うリソースの構造体</summary>
        [SerializeField] private TutorialMissionContentsStuct tutorialMissionContentsStuct;
        #endregion
        [SerializeField] private EnemyEventSystemModel enemyEventSystemModel;
        /// <summary>ペンダグラムシステムのモデル</summary>
        [SerializeField] private PentagramSystemModel pentagramSystemModel;
        /// <summary>式神スキル管理システムのモデル</summary>
        [SerializeField] private ShikigamiSkillSystemModel shikigamiSkillSystemModel;
        /// <summary>ペンダグラムターンテーブルのモデル</summary>
        [SerializeField] private PentagramTurnTableModel pentagramTurnTableModel;
        /// <summary>陰陽（昼夜）の切り替えのモデル</summary>
        [SerializeField] private SunMoonSystemModel sunMoonSystemModel;
        /// <summary>クリア条件を満たす要素を管理するシステムのモデル</summary>
        [SerializeField] private ClearCountdownTimerSystemModel clearCountdownTimerSystemModel;
        /// <summary>ポーズ画面のビュー</summary>
        [SerializeField] private PauseView pauseView;
        /// <summary>フェーダーグループのビュー</summary>
        [SerializeField] private FadersGroupView fadersGroupView;

        private void Reset()
        {
            #region チュートリアル実装_3 メインシーンにてチュートリアルモード追加、会話システム追加など
            tutorialModel = GameObject.Find("Tutorial").GetComponent<TutorialModel>();
            enemiesSpawnTutorialModel = GameObject.Find("EnemiesSpawnTutorial").GetComponent<EnemiesSpawnTutorialModel>();
            missionsSystemTutorialModel = GameObject.Find("MissionsSystemTutorial").GetComponent<MissionsSystemTutorialModel>();
            /*
             * 対象のコンポーネント
             * ●静的にアタッチ
             *  PentagramSystemModel
             *  ShikigamiSkillSystemModel
             *  SunMoonSystemModel
             *  ClearCountdownTimerSystemModel
             *  PauseView
             *  FadersGroupView
             *  GuideMessageView
             *  GuideUITheTurntableView
             *  GuideUITheEqualizerView
             *  GuideUITheEqualizerGageView
             *  GuideUITheDJEnergyView
             *  GuideUIThePlayerHPView
             *  GuideUITheClearCountdownTimerCircleView
             *  GuideUITheClearRewardTextContentsView
             *  MissionsSystemTutorialModel（※ダミー用）
             * ●非同期で動的にアタッチ
             *  DanceTurretModel
             *  GraffitiTurretModel
             *  OnmyoTurretModel
             *  WrapTurretModel
            */
            tutorialGuideContentsStuct = new TutorialGuideContentsStuct()
            {
                targetComponents = new Component[]
                {
                    pentagramSystemModel,
                    shikigamiSkillSystemModel,
                    sunMoonSystemModel,
                    clearCountdownTimerSystemModel,
                    pauseView,
                    fadersGroupView,
                    tutorialView.GuideMessageView,
                    tutorialView.GuideUITheTurntableView,
                    tutorialView.GuideUITheEqualizerGageView,
                    tutorialView.GuideUITheDJEnergyView,
                    tutorialView.GuideUIThePlayerHPView,
                    tutorialView.GuideUITheClearCountdownTimerCircleView,
                    tutorialView.GuideUITheClearRewardTextContentsView,
                },
                tutorialComponentMaps = new TutorialComponentMap[]
                {
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0000,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0001,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0002,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0003,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0004,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0005,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0006,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0007,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0008,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0009,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0010,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Enable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0011,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Enable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0012,
                        tutorialComponents = new TutorialComponent[]
                        {
                            new TutorialComponent()
                            {
                                component = pentagramSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = shikigamiSkillSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = sunMoonSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = clearCountdownTimerSystemModel,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = pauseView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = fadersGroupView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideMessageView,
                                componentState = ComponentState.Pause,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheTurntableView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheEqualizerGageView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheDJEnergyView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUIThePlayerHPView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearCountdownTimerCircleView,
                                componentState = ComponentState.Disable,
                            },
                            new TutorialComponent()
                            {
                                component = tutorialView.GuideUITheClearRewardTextContentsView,
                                componentState = ComponentState.Disable,
                            },
                        }
                    },
                },
            };
            #endregion
            enemyEventSystemModel = GameObject.Find("EnemyEventSystem").GetComponent<EnemyEventSystemModel>();
        }

        public void OnStart()
        {
            #region チュートリアル実装_3 メインシーンにてチュートリアルモード追加、会話システム追加など
            // チュートリアルモードであるかのフラグ（IsTutorialMode）を監視
            tutorialModel.IsTutorialMode.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        this.UpdateAsObservable()
                            .Where(_ => pentagramTurnTableModel.WrapTurretModel != null &&
                                pentagramTurnTableModel.DanceTurretModel != null &&
                                pentagramTurnTableModel.GraffitiTurretModel != null &&
                                pentagramTurnTableModel.OnmyoTurretModels != null &&
                                pentagramTurnTableModel.OnmyoTurretModels.Length == 2)
                            .Take(1)
                            .Subscribe(_ =>
                            {
                                // 動的アタッチとなるコンポーネントはこのタイミングでアタッチする
                                var utility = new MainTutorialsUtility();
                                if (!utility.InitializeTutorialGuideContentsOfPentagramTurnTableModel(tutorialGuideContentsStuct, pentagramTurnTableModel))
                                    Debug.LogError("InitializeTutorialGuideContentsOfPentagramTurnTableModel");
                                // TutorialModelコンポーネントのオブジェクトを有効化
                                tutorialModel.gameObject.SetActive(true);
                                // TutorialModelコンポーネント＞GuideMessageModelコンポーネントの呼び出されたガイドメッセージID（GuideMessageID）を監視
                                tutorialModel.GuideMessageModel.CalledGuideMessageID.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (!utility.DoTutorialGuideContents(x, tutorialGuideContentsStuct))
                                            Debug.LogError("DoTutorialGuideContents");
                                        if (!missionsSystemTutorialModel.SetCallMissionID(x))
                                            Debug.LogError("SetCallMissionID");
                                    });
                                // MissionsSystemTutorialModelコンポーネントの進行中のミッションID（CallMissionID）を監視
                                missionsSystemTutorialModel.CallMissionID.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        if (!utility.DoTutorialMissionContents(x, tutorialMissionContentsStuct))
                                            Debug.LogError("DoTutorialMissionContents");
                                    });
                            });
                    }
                    else
                    {
                        // TutorialModelコンポーネントのオブジェクトを無効化
                        tutorialModel.gameObject.SetActive(true);
                    }
                });
            #endregion
            // 敵の死亡を監視して購読した際に下記の処理を追加
            enemyEventSystemModel.OnEnemyDead.Subscribe(enemyModel =>
            {
                //for (int i = 0; i < enemyModel.EnemiesProp.soulMoneyPoint; i++)
                //{
                //    if (!spawnSoulMoneyModel.InstanceCloneObjects(enemyModel.transform.position, enemyModel.EnemiesProp))
                //        Debug.LogError("InstanceCloneObjects");
                //}
                #region チュートリアル実装_3 メインシーンにてチュートリアルモード追加、会話システム追加など
                if (!missionsSystemTutorialModel.UpdateKilledEnemyCount())
                    Debug.LogError("UpdateKilledEnemyCount");
                #endregion
            });
        }

        #region チュートリアル実装_3 メインシーンにてチュートリアルモード追加、会話システム追加など
        /// <summary>
        /// チュートリアルのガイドで扱うリソースの構造体
        /// </summary>
        [System.Serializable]
        public struct TutorialGuideContentsStuct
        {
            /// <summary>対象のコンポーネント</summary>
            public Component[] targetComponents;
            /// <summary>チュートリアルで扱うガイドIDとリソースの構造体</summary>
            public TutorialComponentMap[] tutorialComponentMaps;
        }

        /// <summary>
        /// チュートリアルで扱うガイドIDとリソースの構造体
        /// </summary>
        [System.Serializable]
        public struct TutorialComponentMap
        {
            /// <summary>ガイドメッセージID</summary>
            public GuideMessageID guideMessageID;
            /// <summary>チュートリアルで扱うリソースの構造体</summary>
            public TutorialComponent[] tutorialComponents;
        }

        /// <summary>
        /// チュートリアルで扱うリソースの構造体
        /// </summary>
        [System.Serializable]
        public struct TutorialComponent
        {
            /// <summary>コンポーネント</summary>
            public Component component;
            /// <summary>コンポーネント、【有効、無効、一時停止】</summary>
            public ComponentState componentState;
        }

        /// <summary>
        /// コンポーネント、【有効、無効、一時停止】
        /// </summary>
        public enum ComponentState
        {
            /// <summary>無効</summary>
            Disable,
            /// <summary>有効</summary>
            Enable,
            /// <summary>一時停止</summary>
            Pause,
        }

        /// <summary>
        /// チュートリアルのミッションで扱うリソースの構造体
        /// </summary>
        [System.Serializable]
        public struct TutorialMissionContentsStuct
        {
            /// <summary>チュートリアルの中のミッションを管理するモデル</summary>
            public MissionsSystemTutorialModel missionsSystemTutorialModel;
            /// <summary>FungusのSayDialogを管理ビュー</summary>
            public GuideMessageView guideMessageView;
            /// <summary>チュートリアル用の敵スポーン制御モデル</summary>
            public EnemiesSpawnTutorialModel enemiesSpawnTutorialModel;
        }
        #endregion

        private void OnGUI()
        {
            // ボタンの配置やサイズを決定（x, y, width, height）
            if (GUI.Button(new Rect(10, 10, 100, 50), "Button 1"))
            {
            }

            if (GUI.Button(new Rect(10, 70, 100, 50), "Button 2"))
            {
            }

            if (GUI.Button(new Rect(10, 130, 100, 50), "Button 3"))
            {
            }
        }
    }
}
