using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Main.View;
using Main.Model;
using UniRx;
using UniRx.Triggers;
using Main.Audio;
using System.Linq;
using Main.InputSystem;
using System.Threading.Tasks;
using Main.Utility;

namespace Main.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class MainPresenter : MonoBehaviour, IMainGameManager
    {
        /// <summary>ポーズ画面のビュー</summary>
        [SerializeField] private PauseView pauseView;
        /// <summary>ポーズボタンのビュー</summary>
        [SerializeField] private GamePauseView gamePauseView;
        /// <summary>ポーズボタンのモデル</summary>
        [SerializeField] private GamePauseModel gamePauseModel;
        /// <summary>クリア画面のビュー</summary>
        [SerializeField] private ClearView clearView;
        /// <summary>ステージ選択へ戻るのビュー</summary>
        [SerializeField] private GameSelectButtonView gameSelectButtonView;
        /// <summary>ステージ選択へ戻るのモデル</summary>
        [SerializeField] private GameSelectButtonModel gameSelectButtonModel;
        /// <summary>カーソルのビュー</summary>
        [SerializeField] private CursorIconView cursorIconView;
        /// <summary>カーソルのモデル</summary>
        [SerializeField] private CursorIconModel cursorIconModel;
        /// <summary>ショートカットキー押下ゲージのビュー</summary>
        [SerializeField] private GameManualScrollView gameManualScrollView;
        /// <summary>遊び方確認ページのビュー</summary>
        [SerializeField] private GameManualViewPageView[] gameManualViewPageViews;
        /// <summary>遊び方確認ページのモデル</summary>
        [SerializeField] private GameManualViewPageModel[] gameManualViewPageModels;
        /// <summary>フェードのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>フェードのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;
        /// <summary>カウントダウンタイマーの情報に合わせてUIを変化させるビュー</summary>
        [SerializeField] private ClearCountdownTimerCircleView clearCountdownTimerCircleView;
        /// <summary>クリア条件を満たす要素を管理するシステムのモデル</summary>
        [SerializeField] private ClearCountdownTimerSystemModel clearCountdownTimerSystemModel;
        /// <summary>ペンダグラムシステムのモデル</summary>
        [SerializeField] private PentagramSystemModel pentagramSystemModel;
        /// <summary>ペンダグラムターンテーブルのビュー</summary>
        [SerializeField] private PentagramTurnTableView pentagramTurnTableView;
        /// <summary>式神スキル管理システムのモデル</summary>
        [SerializeField] private ShikigamiSkillSystemModel shikigamiSkillSystemModel;
        /// <summary>魂の財布、獲得したソウルの管理のモデル</summary>
        [SerializeField] private SoulWalletModel soulWalletModel;
        /// <summary>陰陽（昼夜）の切り替えのモデル</summary>
        [SerializeField] private SunMoonSystemModel sunMoonSystemModel;
        /// <summary>フェーダーのビュー</summary>
        [SerializeField] private FaderUniversalView[] faderUniversalViews;
        /// <summary>蝋燭リソースの情報に合わせてUIを変化させるビュー</summary>
        [SerializeField] private SpGaugeView spGaugeView;
        /// <summary>陰陽（昼夜）のアイコンビュー</summary>
        [SerializeField] private SunMoonStateIconView sunMoonStateIconView;
        /// <summary>ペンダグラムターンテーブルのモデル</summary>
        [SerializeField] private PentagramTurnTableModel pentagramTurnTableModel;
        /// <summary>フェーダーグループのビュー</summary>
        [SerializeField] private FadersGroupView fadersGroupView;
        /// <summary>敵イベントを管理するのモデル</summary>
        [SerializeField] private EnemyEventSystemModel enemyEventSystemModel;
        /// <summary>魂の経験値スポーンのモデル</summary>
        [SerializeField] private SpawnSoulMoneyModel spawnSoulMoneyModel;
        /// <summary>クリア報酬選択のビュー</summary>
        [SerializeField] private RewardSelectView rewardSelectView;
        /// <summary>クリア報酬選択のモデル</summary>
        [SerializeField] private RewardSelectModel rewardSelectModel;
        /// <summary>クリア報酬のコンテンツ</summary>
        [SerializeField] private ClearRewardTextContents clearRewardTextContents;
        /// <summary>レベル背景のビュー</summary>
        [SerializeField] private LevelBackgroundView levelBackgroundView;
        /// <summary>プレイヤーHPのビュー</summary>
        [SerializeField] private ClearCountdownTimerGaugeView clearCountdownTimerGaugeView;
        /// <summary>SP回復前のラップ保存値</summary>
        private float wrapTempoLevelBeforeRest;
        /// <summary>SP回復前のダンス保存値</summary>
        private float danceTempoLevelBeforeRest;
        /// <summary>SP回復前のグラフィティ保存値</summary>
        private float graffitiTempoLevelBeforeRest;
        /// <summary>プレイヤーのHP保存値 前回HPから減少した場合のみ、HP減少処理を実行すすために使用</summary>
        private float beforeHP = 0;
        /// <summary>ゲームオーバー画面のビュー</summary>
        [SerializeField] private GameOverView gameOverView;
        /// <summary>タイトルに戻るボタンのビュー</summary>
        [SerializeField] private GameTitleButtonView[] gameTitleButtonViews;
        /// <summary>タイトルに戻るボタンのモデル</summary>
        [SerializeField] private GameTitleButtonModel[] gameTitleButtonModels;
        /// <summary>コングラチュレーション画面のビュー</summary>
        [SerializeField] private CongratulationsView congratulationsView;
        /// <summary>カウントダウンロゴ（親）のビュー</summary>
        [SerializeField] private CountdownLogosView countdownLogosView;
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

        private void Reset()
        {
            pauseView = GameObject.Find("Pause").GetComponent<PauseView>();
            gamePauseView = GameObject.Find("GamePause").GetComponent<GamePauseView>();
            gamePauseModel = GameObject.Find("GamePause").GetComponent<GamePauseModel>();
            clearView = GameObject.Find("Clear").GetComponent<ClearView>();
            gameSelectButtonView = GameObject.Find("GameSelectButton").GetComponent<GameSelectButtonView>();
            gameSelectButtonModel = GameObject.Find("GameSelectButton").GetComponent<GameSelectButtonModel>();
            cursorIconView = GameObject.Find("CursorIcon").GetComponent<CursorIconView>();
            cursorIconModel = GameObject.Find("CursorIcon").GetComponent<CursorIconModel>();
            gameManualScrollView = GameObject.Find("GameManualScroll").GetComponent<GameManualScrollView>();
            var gmvPageVIdx = 0;
            var gmvPageMIdx = 0;
            gameManualViewPageViews = new GameManualViewPageView[4];
            gameManualViewPageModels = new GameManualViewPageModel[4];
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_1").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_1").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_2").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_2").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_3").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_3").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_4").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_4").GetComponent<GameManualViewPageModel>();
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();
            clearCountdownTimerCircleView = GameObject.Find("SunMoonStateCircleGauge").GetComponent<ClearCountdownTimerCircleView>();
            clearCountdownTimerSystemModel = GameObject.Find("ClearCountdownTimerSystem").GetComponent<ClearCountdownTimerSystemModel>();
            pentagramSystemModel = GameObject.Find("PentagramSystem").GetComponent<PentagramSystemModel>();
            pentagramTurnTableView = GameObject.Find(ConstGameObjectNames.GAMEOBJECT_NAME_PENTAGRAMTURNTABLE).GetComponent<PentagramTurnTableView>();
            pentagramTurnTableModel = GameObject.Find(ConstGameObjectNames.GAMEOBJECT_NAME_PENTAGRAMTURNTABLE).GetComponent<PentagramTurnTableModel>();
            shikigamiSkillSystemModel = GameObject.Find("ShikigamiSkillSystem").GetComponent<ShikigamiSkillSystemModel>();
            soulWalletModel = GameObject.Find("SoulWallet").GetComponent<SoulWalletModel>();
            sunMoonSystemModel = GameObject.Find("SunMoonSystem").GetComponent<SunMoonSystemModel>();
            faderUniversalViews = new FaderUniversalView[]
            {
                GameObject.Find($"Fader{ShikigamiType.Wrap}").GetComponent<FaderUniversalView>(),
                GameObject.Find($"Fader{ShikigamiType.Dance}").GetComponent<FaderUniversalView>(),
                GameObject.Find($"Fader{ShikigamiType.Graffiti}").GetComponent<FaderUniversalView>(),
            };
            spGaugeView = GameObject.Find("SpGauges").GetComponent<SpGaugeView>();
            sunMoonStateIconView = GameObject.Find("SunMoonStateIcon").GetComponent<SunMoonStateIconView>();
            fadersGroupView = GameObject.Find("FadersGroup").GetComponent<FadersGroupView>();
            enemyEventSystemModel = GameObject.Find("EnemyEventSystem").GetComponent<EnemyEventSystemModel>();
            spawnSoulMoneyModel = GameObject.Find("SpawnSoulMoney").GetComponent<SpawnSoulMoneyModel>();
            rewardSelectView = GameObject.Find("RewardSelect").GetComponent<RewardSelectView>();
            rewardSelectModel = GameObject.Find("RewardSelect").GetComponent<RewardSelectModel>();
            clearRewardTextContents = GameObject.Find("Resources").GetComponentInChildren<ClearRewardTextContents>();
            levelBackgroundView = GameObject.Find("LevelBackground").GetComponent<LevelBackgroundView>();
            clearCountdownTimerGaugeView = GameObject.Find("PlayerHP").GetComponent<ClearCountdownTimerGaugeView>();
            gameOverView = GameObject.Find("GameOver").GetComponent<GameOverView>();
            gameTitleButtonViews = GameObject.Find("Canvas").GetComponentsInChildren<GameTitleButtonView>();
            gameTitleButtonModels = GameObject.Find("Canvas").GetComponentsInChildren<GameTitleButtonModel>();
            congratulationsView = GameObject.Find("Congratulations").GetComponent<CongratulationsView>();
            countdownLogosView = GameObject.Find("CountdownLogos").GetComponent<CountdownLogosView>();
            tutorialModel = GameObject.Find("Tutorial").GetComponent<TutorialModel>();
            tutorialView = GameObject.FindObjectOfType<TutorialView>();
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
                    tutorialView.GuideUITheEqualizerView,
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
                    new TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0013,
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
                },
            };
            tutorialMissionContentsStuct = new TutorialMissionContentsStuct()
            {
                missionsSystemTutorialModel = GameObject.FindObjectOfType<MissionsSystemTutorialModel>(),
                guideMessageView = GameObject.FindObjectOfType<GuideMessageView>(),
                enemiesSpawnTutorialModel = GameObject.FindObjectOfType<EnemiesSpawnTutorialModel>(),
            };
        }

        public void OnStart()
        {
            // プレイヤー開始位置のビュー
            PlayerStartPointView playerStartPointView = null;
            // プレイヤーのビュー
            PlayerView playerView;
            // プレイヤーのモデル
            PlayerModel playerModel = null;

            var common = new MainPresenterCommon();

            // 初期設定
            pauseView.gameObject.SetActive(false);
            gameSelectButtonView.gameObject.SetActive(false);
            cursorIconView.gameObject.SetActive(false);
            gameManualScrollView.gameObject.SetActive(false);
            gameOverView.gameObject.SetActive(false);
            congratulationsView.gameObject.SetActive(false);
            foreach (var gameTitleButtonView in gameTitleButtonViews)
                gameTitleButtonView.gameObject.SetActive(false);

            MainGameManager.Instance.AudioOwner.OnStartAndPlayBGM();
            // T.B.D ステージ開始演出
            var isStartDirectionCompleted = new IntReactiveProperty();
            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // T.B.D ステージ開始演出
                    isStartDirectionCompleted.Value++;
                    Debug.Log($"フェード完了:[{isStartDirectionCompleted.Value}]");
                })
                .AddTo(gameObject);
            // ショートカットキーの押下（有効／無効）状態
            var isInputUIActionsEnabled = new BoolReactiveProperty();
            // ポーズボタンの押下（有効／無効）状態
            var isInputUIPausedEnabled = new BoolReactiveProperty();
            // T.B.D ステージ開始演出
            isStartDirectionCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x == 2)
                .Subscribe(_ =>
                {
                    // プレイヤーを開始ポイントへ生成
                    if (playerStartPointView != null)
                        if (!playerStartPointView.InstancePlayer())
                            Debug.LogError("プレイヤー生成呼び出しの失敗");
                    isInputUIActionsEnabled.Value = true;
                    isInputUIPausedEnabled.Value = true;
                });
            // ポーズ押下
            var inputUIPausedState = new BoolReactiveProperty();
            inputUIPausedState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // ポーズ画面が閉じている　かつ、
                    // クリア画面が閉じている
                    if (x &&
                        !pauseView.gameObject.activeSelf &&
                        !clearView.gameObject.activeSelf &&
                        !gameOverView.gameObject.activeSelf &&
                        !congratulationsView.gameObject.activeSelf)
                    {
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_play_open);
                        // 遊び方確認ページを開いているなら閉じる
                        if (gameManualScrollView.gameObject.activeSelf)
                        {
                            if (!gameManualViewPageModels[(int)EnumShortcuActionMode.CheckAction].SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameManualViewPageModels[(int)EnumShortcuActionMode.CheckAction].SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // 遊び方を確認クローズのアニメーション
                            Observable.FromCoroutine<bool>(observer => gameManualScrollView.PlayCloseAnimation(observer))
                                .Subscribe(_ =>
                                {
                                    gameManualScrollView.gameObject.SetActive(false);
                                })
                                .AddTo(gameObject);
                        }
                        pauseView.gameObject.SetActive(true);
                        gamePauseModel.SetSelectedGameObject();
                        if (!playerModel.SetInputBan(true))
                            Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                    }
                });
            // ポーズ画面表示中の操作
            gamePauseModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            // 処理無し
                            break;
                        case EnumEventCommand.DeSelected:
                            // 処理無し
                            break;
                        case EnumEventCommand.Submited:
                            // 処理無し
                            break;
                        case EnumEventCommand.Canceled:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            if (!gamePauseModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gamePauseModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // ポーズ画面クローズのアニメーション
                            Observable.FromCoroutine<bool>(observer => pauseView.PlayCloseAnimation(observer))
                                .Subscribe(_ =>
                                {
                                    pauseView.gameObject.SetActive(false);
                                    if (!playerModel.SetInputBan(false))
                                        Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                                })
                                .AddTo(gameObject);
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // クリア画面表示のため、ゴール到達のフラグ更新
            var datas = MainGameManager.Instance.SceneOwner.GetSaveDatas();
            var isGoalReached = new BoolReactiveProperty();
            // ゲームオーバー画面表示のため、HP0になった時ののフラグ更新
            var isPlayerDead = new BoolReactiveProperty();
            isGoalReached.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        // クリア済みデータの更新
                        if (0 < datas.sceneId &&
                            datas.sceneId < 8)
                        {
                            datas.state[datas.sceneId - 1] = 2;
                            if (datas.sceneId < datas.state.Length - 1 &&
                                datas.state[(datas.sceneId)] < 1)
                                datas.state[(datas.sceneId)] = 1;
                        }
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.me_game_clear);
                        // 初回のみ最初から拡大表示
                        if (!common.IsFinalLevel(datas))
                        {
                            // 初期処理
                            if (!rewardSelectView.SetContents(rewardSelectModel.RewardContentProps))
                                Debug.LogError("SetContents");
                            if (!clearView.SetActiveGameObject(true))
                                Debug.LogError("SetActiveGameObject");
                            gameSelectButtonView.gameObject.SetActive(false);
                            var rewardContentModel = rewardSelectModel.RewardContentModels[0];
                            if (!cursorIconView.SetSelectAndScale(rewardContentModel.transform.position, (rewardContentModel.transform as RectTransform).sizeDelta))
                                Debug.LogError("SetSelectAndScale");
                            rewardContentModel.SetSelectedGameObject();
                            gameSelectButtonView.gameObject.SetActive(true);
                            cursorIconView.gameObject.SetActive(true);
                        }
                        else
                        {
                            // 初期処理
                            if (!congratulationsView.SetActiveGameObject(true))
                                Debug.LogError("SetActiveGameObject");
                            foreach (var gameTitleButtonView in gameTitleButtonViews.Where(q => q.transform.parent.gameObject.name.Equals("Congratulations")))
                                gameTitleButtonView.gameObject.SetActive(true);
                            foreach (var gameTitleButtonModel in gameTitleButtonModels.Where(q => q.transform.parent.gameObject.name.Equals("Congratulations")))
                                gameTitleButtonModel.SetSelectedGameObject();
                            if (!playerModel.SetInputBan(true))
                                Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                            if (!MainGameManager.Instance.SceneOwner.DestroyMainSceneStagesState())
                                Debug.LogError("DestroyMainSceneStagesState");
                            if (!soulWalletModel.SetIsClearFinalStage(true))
                                Debug.LogError("SetIsClearFinalStage");
                        }
                    }
                });

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
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            // 選択された時の処理
                            if (!cursorIconView.SetSelectAndScale(gameSelectButtonModel.transform.position, (gameSelectButtonModel.transform as RectTransform).sizeDelta))
                                Debug.LogError("SetSelectAndScale");

                            break;
                        case EnumEventCommand.DeSelected:
                            if (!gameSelectButtonView.SetDefaultScale())
                                Debug.LogError("デフォルトサイズへ変更呼び出しの失敗");
                            break;
                        case EnumEventCommand.Submited:
                            // クリア状況のセーブ
                            datas.sceneId++;
                            if (!MainGameManager.Instance.SceneOwner.SetSaveDatas(datas))
                                Debug.LogError("クリア済みデータ保存呼び出しの失敗");
                            // 遷移処理
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            if (!gameSelectButtonModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameSelectButtonModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // プレイヤーの挙動によって発生するイベント無効　など
                            if (!MainGameManager.Instance.InputSystemsOwner.Exit())
                                Debug.LogError("InputSystem終了呼び出しの失敗");
                            if (!MainGameManager.Instance.LevelOwner.SetSlots())
                                Debug.LogError("SetSlots");
                            // シーン読み込み時のアニメーション
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadSelectScene())
                                .AddTo(gameObject);
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            isPlayerDead.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        var audioOwner = MainGameManager.Instance.AudioOwner;
                        audioOwner.PlaySFX(ClipToPlay.me_game_over);
                        audioOwner.StopBGM();

                        // 初期処理
                        if (!gameOverView.SetActiveGameObject(true))
                            Debug.LogError("SetActiveGameObject");
                        foreach (var gameTitleButtonView in gameTitleButtonViews.Where(q => q.transform.parent.gameObject.name.Equals("GameOver")))
                            gameTitleButtonView.gameObject.SetActive(true);
                        foreach (var gameTitleButtonModel in gameTitleButtonModels.Where(q => q.transform.parent.gameObject.name.Equals("GameOver")))
                        {
                            gameTitleButtonModel.SetSelectedGameObject();
                        }
                        if (!playerModel.SetInputBan(true))
                            Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                        if (!MainGameManager.Instance.SceneOwner.DestroyMainSceneStagesState())
                            Debug.LogError("DestroyMainSceneStagesState");
                    }
                });
            // ゲームオーバー画面 -> タイトル画面へ戻る
            foreach (var gameTitleButtonModel in gameTitleButtonModels.Select((p, i) => new { Content = p, Index = i }))
            {
                gameTitleButtonModel.Content.EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                // 処理無し
                                break;
                            case EnumEventCommand.Selected:
                                // 処理無し
                                break;
                            case EnumEventCommand.DeSelected:
                                // 処理無し
                                break;
                            case EnumEventCommand.Submited:
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                if (!gameTitleButtonModel.Content.SetButtonEnabled(false))
                                    Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                                if (!gameTitleButtonModel.Content.SetEventTriggerEnabled(false))
                                    Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                                // プレイヤーの挙動によって発生するイベント無効　など
                                if (!MainGameManager.Instance.InputSystemsOwner.Exit())
                                    Debug.LogError("InputSystem終了呼び出しの失敗");
                                // シーン読み込み時のアニメーション
                                Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                    .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadTitleScene())
                                    .AddTo(gameObject);
                                break;
                            case EnumEventCommand.Canceled:
                                // 処理無し
                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }

            // ショートカットキー
            var inputUIPushedTime = new FloatReactiveProperty();
            var inputUIActionsState = new IntReactiveProperty((int)EnumShortcuActionMode.None);
            inputUIActionsState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // 押下されるボタンが切り替わったら押下時間リセット
                    inputUIPushedTime.Value = 0f;
                });
            inputUIPushedTime.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                });
            // 遊び方を確認
            for (var i = 0; i < gameManualViewPageModels.Length; i++)
            {
                var tmpIdx = i;
                gameManualViewPageModels[tmpIdx].EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Default:
                                // 処理無し
                                break;
                            case EnumEventCommand.Selected:
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                                if (!gameManualScrollView.PlayPagingAnimation((EnumGameManualPagesIndex)tmpIdx))
                                    Debug.LogError("ページ変更アニメーション呼び出しの失敗");
                                break;
                            case EnumEventCommand.DeSelected:
                                // 処理無し
                                break;
                            case EnumEventCommand.Submited:
                                // 処理無し
                                break;
                            case EnumEventCommand.Canceled:
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                                if (!gameManualViewPageModels[tmpIdx].SetButtonEnabled(false))
                                    Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                                if (!gameManualViewPageModels[tmpIdx].SetEventTriggerEnabled(false))
                                    Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                                // 遊び方を確認クローズのアニメーション
                                Observable.FromCoroutine<bool>(observer => gameManualScrollView.PlayCloseAnimation(observer))
                                    .Subscribe(_ =>
                                    {
                                        gameManualScrollView.gameObject.SetActive(false);
                                        if (!playerModel.SetInputBan(false))
                                            Debug.LogError("操作禁止フラグをセット呼び出しの失敗");
                                    })
                                    .AddTo(gameObject);
                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }
            // チュートリアルUI -> 移動操作
            var isTriggerEnteredMoveGuide = new BoolReactiveProperty();
            isTriggerEnteredMoveGuide.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                    }
                    else
                    {
                    }
                });
            // チュートリアルUI -> ジャンプ操作
            var isTriggerEnteredJumpGuide = new BoolReactiveProperty();
            isTriggerEnteredJumpGuide.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                    }
                    else
                    {
                    }
                });
            // レベルのインスタンスに合わせてメンバー変数をセット
            var levelOwner = MainGameManager.Instance.LevelOwner;
            levelOwner.IsInstanced.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        // プレイヤーがインスタンス状態
                        playerStartPointView = GameObject.Find(ConstGameObjectNames.GAMEOBJECT_NAME_PLAYERSTARTPOINT).GetComponent<PlayerStartPointView>();
                        isStartDirectionCompleted.Value++;
                        Debug.Log($"スタート開始位置を生成完了:[{isStartDirectionCompleted.Value}]");
                        playerStartPointView.IsInstanced.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                if (x)
                                {
                                    var player = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_PLAYER);
                                    playerView = player.GetComponent<PlayerView>();
                                    playerModel = player.GetComponent<PlayerModel>();
                                    playerModel.IsInstanced.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (x)
                                                if (!pentagramTurnTableView.CalibrationToTarget(playerModel.transform))
                                                    Debug.LogError("CalibrationToTarget");
                                        });
                                    IClearCountdownTimerViewAdapter circleView = new ClearCountdownTimerGaugeViewAdapter(clearCountdownTimerGaugeView);
                                    playerModel.State.HP.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (!circleView.Set(x, playerModel.State.HPMax))
                                                Debug.LogError("Set");

                                            //HPの変化量がマイナス（ダメージ）の場合のみ、エフェクトを発生
                                            if (beforeHP - x > 0)
                                                if (!playerView.PlayHitEffect())
                                                    Debug.LogError("PlayHitEffect");
                                            beforeHP = x;

                                        });
                                    playerModel.State.IsDead.ObserveEveryValueChanged(x => x.Value)
                                        .Subscribe(x =>
                                        {
                                            if (x)
                                            {
                                                if (!pentagramTurnTableView.SetSpriteIndex(0f, playerModel.State.HPMax))
                                                    Debug.LogError("SetSpriteIndex");
                                                // ゲームオーバー画面を表示する為には以下の処理を変更する必要がある。
                                                isPlayerDead.Value = true;
                                            }
                                        });
                                }
                            });
                        clearCountdownTimerSystemModel.enabled = true;
                        IClearCountdownTimerViewAdapter circleView = new ClearCountdownTimerCircleViewAdapter(clearCountdownTimerCircleView);
                        clearCountdownTimerSystemModel.TimeSec.ObserveEveryValueChanged(x => x.Value)
                            .Pairwise()    
                            .Subscribe(x =>
                            {
                                if (!circleView.Set(x.Current, clearCountdownTimerSystemModel.LimitTimeSecMax))
                                    Debug.LogError("SetAngle");
                                if (x.Current != 0f &&
                                    x.Previous != 0f)
                                {
                                    Observable.FromCoroutine<bool>(observer => countdownLogosView.PlayCountDownDirection(observer, x.Current))
                                        .Subscribe(_ => { })
                                        .AddTo(gameObject);
                                }
                            });
                        clearCountdownTimerSystemModel.IsTimeOut.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                if (!circleView.Set(0f, clearCountdownTimerSystemModel.LimitTimeSecMax, x))
                                    Debug.LogError("SetAngle");
                                if (!clearCountdownTimerSystemModel.SetIsGoalReached(isGoalReached, x))
                                    Debug.LogError("SetIsGoalReached");
                            });
                        this.UpdateAsObservable()
                            .Select(_ => levelOwner.InstancedLevel.GetComponentInChildren<EnemiesSpawnModel>())
                            .Where(enemiesSpawnModel => enemiesSpawnModel != null)
                            .Take(1)
                            .Subscribe(enemiesSpawnModel =>
                            {
                                // enemiesSpawnModelがnullでないときの処理を設定
                                sunMoonSystemModel.OnmyoState.ObserveEveryValueChanged(x => x.Value)
                                    .Subscribe(x =>
                                    {
                                        sunMoonStateIconView.SetRotate(x);
                                        if (!enemiesSpawnModel.SetOnmyoState(x))
                                            Debug.LogError("SetOnmyoState");
                                        if (!clearCountdownTimerCircleView.SetColor(x))
                                            Debug.LogError("SetColor");
                                        Observable.FromCoroutine<bool>(observer => levelBackgroundView.SwitchLayerAndPlayFadeAnimation(observer, x))
                                            .Subscribe(_ => { })
                                            .AddTo(gameObject);
                                    });
                            });
                        enemyEventSystemModel.OnEnemyDead.Subscribe(enemyModel =>
                        {
                            for (int i = 0; i < enemyModel.EnemiesProp.soulMoneyPoint; i++)
                            {
                                if (!spawnSoulMoneyModel.InstanceCloneObjects(enemyModel.transform.position, enemyModel.EnemiesProp))
                                    Debug.LogError("InstanceCloneObjects");
                            }
                            if (!missionsSystemTutorialModel.UpdateKilledEnemyCount())
                                Debug.LogError("UpdateKilledEnemyCount");
                        });
                        spawnSoulMoneyModel.OnSoulMoneyGeted
                           .Subscribe(soulMoney =>
                           {
                               //if (soulMoney.IsGeted.Value)
                               //{
                               //    var soulMoneyPoint = soulWalletModel.AddSoulMoney(soulMoney.EnemiesProp.soulMoneyPoint);
                               //    //Debug.Log(soulMoney.EnemiesProp.soulMoneyPoint);
                               //    if (soulMoneyPoint < 0)
                               //        Debug.LogError("AddSoulMoney");
                               //}
                           })
                           .AddTo(gameObject); // UniRxのAddToを使用して、このGameObjectが破棄されたときに購読を自動的に解除
                        if (!soulWalletModel.SetIsUnLockUpdateOfSoulMoney(x))
                            Debug.LogError("SetIsUnLockUpdateOfSoulMoney");
                        levelOwner.SelectedRewardIDs.ObserveAdd()
                            .Subscribe(x =>
                            {
                                if (!gameSelectButtonModel.CheckSelectedIDAndSetUIControllEnabled(levelOwner.SelectedRewardIDs))
                                    Debug.LogError("CheckSelectedIDAndSetUIControllEnabled");
                            })
                            .AddTo(gameObject);
                        levelOwner.SelectedRewardIDs.ObserveRemove()
                            .Subscribe(x =>
                            {
                                if (!gameSelectButtonModel.CheckSelectedIDAndSetUIControllEnabled(levelOwner.SelectedRewardIDs))
                                    Debug.LogError("CheckSelectedIDAndSetUIControllEnabled");
                            })
                            .AddTo(gameObject);
                    }
                });
            BgmConfDetails bgmConfDetails = new BgmConfDetails();
            this.UpdateAsObservable()
                .Select(_ => pentagramSystemModel)
                // モデルがアクティブな状態でのみ動作すること
                .Where(x => x.isActiveAndEnabled)
                .Subscribe(x =>
                {
                    if (!pentagramSystemModel.InputSlipLoopState.IsLooping.Value)
                    {
                        bgmConfDetails.InputValue = x.InputValue.Value;
                        bgmConfDetails.PentagramSpinState = (PentagramSpinState)x.PentagramSpinState.Value;
                        if (!pentagramTurnTableView.MoveSpin(bgmConfDetails))
                            Debug.LogError("MoveSpin");
                    }
                });
            pentagramSystemModel.JockeyCommandType.ObserveEveryValueChanged(x => x.Value)
                .Pairwise()
                .Subscribe(pair =>
                {
                    if (!shikigamiSkillSystemModel.UpdateCandleResource((JockeyCommandType)pair.Current, (JockeyCommandType)pair.Previous))
                        Debug.LogError("UpdateCandleResource");
                    if (!pentagramTurnTableModel.BuffAllTurrets((JockeyCommandType)pair.Current))
                        Debug.LogError("BuffAllTurrets");
                    // バックスピンはSPゲージ回復中は無効
                    if (!shikigamiSkillSystemModel.CandleInfo.isRest.Value)
                    {
                    if (!shikigamiSkillSystemModel.ForceZeroAndRapidRecoveryCandleResource((JockeyCommandType)pair.Current))
                        Debug.LogError("ForceZeroAndRapidRecoveryCandleResource");
                    Observable.FromCoroutine<bool>(observer => pentagramTurnTableView.PlayDirectionBackSpin(observer, (JockeyCommandType)pair.Current))
                        .Subscribe(x =>
                        {
                            if (!pentagramSystemModel.ResetJockeyCommandType())
                                Debug.LogError("ResetJockeyCommandType");
                        })
                        .AddTo(gameObject);
                    }
                    else
                    {
                        // バックスピンの呼び出しが無効の場合は、即座に入力状態をリセット
                        if (!pentagramSystemModel.ResetJockeyCommandType())
                            Debug.LogError("ResetJockeyCommandType");
                    }
                    if (!pentagramSystemModel.SetIsLooping((JockeyCommandType)pair.Current))
                        Debug.LogError("SetIsLooping");
                    if (!shikigamiSkillSystemModel.SetIsStopRecovery((JockeyCommandType)pair.Current))
                        Debug.LogError("SetIsStopRecovery");
                    if (!pentagramTurnTableView.AdjustBGM((JockeyCommandType)pair.Previous, (JockeyCommandType)pair.Current))
                        Debug.LogError("AdjustBGM");
                });
            this.UpdateAsObservable()
                .Select(_ => pentagramSystemModel.InputSlipLoopState.IsLooping)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    x.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (!x)
                            {
                                if (!pentagramTurnTableModel.SetMoveDirectionsDefaultOfOnmyoWrapGraffitiTurret())
                                    Debug.LogError("SetMoveDirectionsDefaultOfOnmyoWrapGraffitiTurret");
                                if (!pentagramTurnTableView.ResetFromAngle())
                                    Debug.LogError("ResetFromAngle");
                            }
                            if (!pentagramTurnTableModel.SetActionRateNormalOfOnmyoTurret(x))
                                Debug.LogError("SetActionRateNormalOfOnmyoTurret");

                            pentagramTurnTableView.SetLoopImage(x);
                        });
                });
            this.UpdateAsObservable()
                .Select(_ => pentagramSystemModel.InputSlipLoopState.ActionTrigger)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    x.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (x)
                            {
                                if (!pentagramTurnTableModel.SetMoveDirectionsToDanceOfOnmyoWrapGraffitiTurret())
                                    Debug.LogError("SetMoveDirectionsToDanceOfOnmyoWrapGraffitiTurret");
                                Observable.FromCoroutine<bool>(observer => pentagramTurnTableView.MoveSpin(observer, pentagramSystemModel.InputSlipLoopState))
                                    .Subscribe(x =>
                                    {
                                        if (x)
                                        {
                                            if (!pentagramTurnTableModel.AttackOfOnmyoTurretLoop(pentagramSystemModel.InputSlipLoopState))
                                                Debug.LogError("AttackOfOnmyoTurret");
                                            if (!shikigamiSkillSystemModel.UpdateCandleResourceOfAttackOnmyoTurret(pentagramSystemModel.InputSlipLoopState))
                                                Debug.LogError("UpdateCandleResourceOfAttackOnmyoTurret");
                                        }
                                    })
                                    .AddTo(gameObject);
                            }
                        });
                });
            this.UpdateAsObservable()
                .Select(_ => pentagramSystemModel.InputSlipLoopState.beatLength)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    x.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            pentagramTurnTableView.SetBeatLength(x);
                        });
                });
            this.UpdateAsObservable()
                .Select(_ => shikigamiSkillSystemModel.ShikigamiInfos)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    foreach (var item in x.Select((p, i) => new { Content = p, Index = i }))
                        item.Content.state.tempoLevel.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                switch (item.Content.prop.type)
                                {
                                    case ShikigamiType.Wrap:
                                        wrapTempoLevelBeforeRest = x;
                                        break;
                                    case ShikigamiType.Dance:
                                        danceTempoLevelBeforeRest = x;
                                        break;
                                    case ShikigamiType.Graffiti:
                                        graffitiTempoLevelBeforeRest = x;
                                        break;
                                    case ShikigamiType.OnmyoTurret:
                                        //処理なし
                                        break;
                                    default:
                                        throw new System.Exception("例外エラー");
                                }

                                //SP回復中は強制的にTempoLevelをMINに設定
                                if (item.Content.state.isRest.Value)
                                    x = -1.0f;
                                    
                                foreach (var faderUniversalView in faderUniversalViews)
                                {
                                    if (!faderUniversalView.SetSliderValue(x, item.Content.prop.type))
                                        Debug.LogError("SetSliderValue");
                                }
                                if (!pentagramTurnTableModel.UpdateTempoLvValues(x, item.Content.prop.type))
                                    Debug.LogError("UpdateTempoLvValues");
                                Observable.FromCoroutine<bool>(observer => fadersGroupView.PlayMoveAnchorsBasedOnHeight(observer, EnumFadeState.Open))
                                    .Subscribe(_ => {})
                                    .AddTo(gameObject);
                            });
                    foreach (var item in x.Select((p, i) => new { Content = p, Index = i }))
                        item.Content.state.isRest.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                //SP回復完了時に、TempoLevelを設定
                                if (!x)
                                {
                                    foreach (var faderUniversalView in faderUniversalViews)
                                    {
                                        if (!faderUniversalView.SetSliderValue(wrapTempoLevelBeforeRest, ShikigamiType.Wrap))
                                            Debug.LogError("SetSliderValue");
                                        if (!faderUniversalView.SetSliderValue(danceTempoLevelBeforeRest, ShikigamiType.Dance))
                                            Debug.LogError("SetSliderValue");
                                        if (!faderUniversalView.SetSliderValue(graffitiTempoLevelBeforeRest, ShikigamiType.Graffiti))
                                            Debug.LogError("SetSliderValue");
                                    }
                                    if (!pentagramTurnTableModel.UpdateTempoLvValues(wrapTempoLevelBeforeRest, ShikigamiType.Wrap))
                                        Debug.LogError("UpdateTempoLvValues");
                                    if (!pentagramTurnTableModel.UpdateTempoLvValues(danceTempoLevelBeforeRest, ShikigamiType.Dance))
                                        Debug.LogError("UpdateTempoLvValues");
                                    if (!pentagramTurnTableModel.UpdateTempoLvValues(graffitiTempoLevelBeforeRest, ShikigamiType.Graffiti))
                                        Debug.LogError("UpdateTempoLvValues");
                                    Observable.FromCoroutine<bool>(observer => fadersGroupView.PlayMoveAnchorsBasedOnHeight(observer, EnumFadeState.Open))
                                        .Subscribe(_ => { })
                                        .AddTo(gameObject);
                                }
                            });
                });
            this.UpdateAsObservable()
                .Select(_ => shikigamiSkillSystemModel.CandleInfo.CandleResource)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    x.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (!spGaugeView.SetVertical(x, shikigamiSkillSystemModel.CandleInfo.LimitCandleResorceMax))
                                Debug.LogError("SetVertical");
                        });
                });
            this.UpdateAsObservable()
                .Select(_ => shikigamiSkillSystemModel.CandleInfo.IsOutCost)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    x.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (!pentagramSystemModel.SetIsLooping(JockeyCommandType.None))
                                Debug.LogError("SetIsLooping");
                            if (!shikigamiSkillSystemModel.SetIsStopRecovery(JockeyCommandType.None))
                                Debug.LogError("SetIsStopRecovery");
                        });
                });
            this.UpdateAsObservable()
                .Select(_ => shikigamiSkillSystemModel.CandleInfo.isRest)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    x.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            spGaugeView.SetFire(x);
                        });
                });

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (isInputUIPausedEnabled.Value &&
                        pauseView.IsControllEnabled)
                    {
                        var inputSystemsOwner = MainGameManager.Instance.InputSystemsOwner;
                        switch ((InputMode)inputSystemsOwner.CurrentInputMode.Value)
                        {
                            case InputMode.Gamepad:
                                inputUIPausedState.Value = inputSystemsOwner.InputUI.Paused;

                                break;
                            case InputMode.MidiJackDDJ200:
                                inputUIPausedState.Value = inputSystemsOwner.InputMidiJackDDJ200.PlayOrPause ||
                                    inputSystemsOwner.InputMidiJackDDJ200.Cue;

                                break;
                            default:
                                break;
                        }
                    }
                    if (isInputUIActionsEnabled.Value)
                    {
                        if (((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.None))
                        {
                            // ショートカットキーの押下が None -> Any へ変わる
                            if (MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Undoed &&
                                !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Selected)
                                inputUIActionsState.Value = (int)EnumShortcuActionMode.UndoAction;
                            else if (MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Selected)
                                inputUIActionsState.Value = (int)EnumShortcuActionMode.SelectAction;
                        }
                        else if ((((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.UndoAction) &&
                            !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Undoed) ||
                            (((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.SelectAction) &&
                            !MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Selected))
                        {
                            // ショートカットキーの押下が Any -> None へ変わる
                            inputUIActionsState.Value = (int)EnumShortcuActionMode.None;
                        }
                        if (!((EnumShortcuActionMode)inputUIActionsState.Value).Equals(EnumShortcuActionMode.None))
                            inputUIPushedTime.Value += Time.deltaTime;
                        else if (0f < inputUIPushedTime.Value)
                            // ショートカットキーの押下状態がNoneへ戻ったらリセット
                            // 既に0fなら何度も更新は行わない
                            inputUIPushedTime.Value = 0f;
                    }
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
                    if (!clearRewardTextContents.SetSoulMoney(x))
                        Debug.LogError("SetSoulMoney");
                });
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
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);

                                break;
                            case EnumEventCommand.DeSelected:
                                if (!rewardSelectView.ScaleDown(item.Index))
                                    Debug.LogError("ScaleDown");

                                break;
                            case EnumEventCommand.Submited:
                                if (!rewardSelectModel.Check(item.Index))
                                    Debug.LogError("Check");
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);

                                break;
                            case EnumEventCommand.Canceled:
                                if (!rewardSelectModel.UnCheck(item.Index))
                                    Debug.LogError("UnCheck");
                                MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);

                                break;
                            default:
                                // 処理無し
                                break;
                        }
                    });
            }
            rewardSelectModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    if (!clearView.SetActiveGameObject(false))
                        Debug.LogError("SetActiveGameObject");
                });
            // チュートリアルモードであるかのフラグ（IsTutorialMode）を監視
            tutorialModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ =>
                {
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
                                        tutorialGuideContentsStuct = utility.InitializeTutorialGuideContentsOfPentagramTurnTableModel(tutorialGuideContentsStuct, pentagramTurnTableModel);
                                        if (tutorialGuideContentsStuct.targetComponents == null ||
                                            tutorialGuideContentsStuct.targetComponents.Length < 1)
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
                                                switch (x)
                                                {
                                                    case GuideMessageID.GM0012:
                                                        // プレイヤーの挙動によって発生するイベント無効　など
                                                        if (!MainGameManager.Instance.InputSystemsOwner.Exit())
                                                            Debug.LogError("InputSystem終了呼び出しの失敗");
                                                        // シーン読み込み時のアニメーション
                                                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                                            .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadTitleScene())
                                                            .AddTo(gameObject);

                                                        break;
                                                    default:
                                                        break;
                                                }
                                            });
                                        // MissionsSystemTutorialModelコンポーネントの進行中のミッションID（CallMissionID）を監視
                                        System.IDisposable modelUpdObservable = tutorialMissionContentsStuct.missionsSystemTutorialModel.UpdateAsObservable().Subscribe(_ => { });
                                        modelUpdObservable.Dispose();
                                        missionsSystemTutorialModel.CallMissionID.ObserveEveryValueChanged(x => x.Value)
                                            .Subscribe(x =>
                                            {
                                                if (!utility.DoTutorialMissionContents(x, tutorialMissionContentsStuct, modelUpdObservable))
                                                    Debug.LogError("DoTutorialMissionContents");
                                            });
                                    });
                            }
                            else
                            {
                                // TutorialModelコンポーネントのオブジェクトを無効化
                                tutorialModel.gameObject.SetActive(false);
                            }
                        });
                });
        }
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
    }
}
