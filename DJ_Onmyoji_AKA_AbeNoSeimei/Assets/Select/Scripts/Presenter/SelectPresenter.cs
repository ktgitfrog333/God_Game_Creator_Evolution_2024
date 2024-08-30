using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;
using Select.View;
using Select.Model;
using UniRx;
using UniRx.Triggers;
using Select.Audio;
using System.Linq;
using DG.Tweening;

namespace Select.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class SelectPresenter : MonoBehaviour, ISelectGameManager
    {
        /// <summary>FadeImageのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>Fadeimageのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;
        /// <summary>ステージセレクトのビュー</summary>
        [SerializeField] private StageSelectView stageSelectView;
        /// <summary>ステージセレクトのモデル</summary>
        [SerializeField] private StageSelectModel stageSelectModel;
        /// <summary>プレゼンタの読み込み完了</summary>
        public IReactiveProperty<bool> IsCompleted { get; private set; } = new BoolReactiveProperty();
        /// <summary>チュートリアル画面のビュー</summary>
        [SerializeField] private TutorialView tutorialView;
        /// <summary>チュートリアル画面ページのビュー</summary>
        [SerializeField] private TutorialViewPageView[] tutorialViewPageViews;
        /// <summary>遊び方確認ページのモデル</summary>
        [SerializeField] private TutorialViewPageModel[] tutorialViewPageModels;
        /// <summary>遅延処理</summary>
        private Tween _tween;

        private void Reset()
        {
            // フェードのビューとモデルをセット
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();
            stageSelectView = GameObject.Find("StageSelect").GetComponent<StageSelectView>();
            stageSelectModel = GameObject.Find("StageSelect").GetComponent<StageSelectModel>();
            tutorialView = GameObject.Find("Tutorial").GetComponent<TutorialView>();
            var gmvPageVIdx = 0;
            var gmvPageMIdx = 0;
            tutorialViewPageViews = new TutorialViewPageView[5];
            tutorialViewPageModels = new TutorialViewPageModel[5];
            tutorialViewPageViews[gmvPageVIdx++] = GameObject.Find("TutorialViewPage_1").GetComponent<TutorialViewPageView>();
            tutorialViewPageModels[gmvPageMIdx++] = GameObject.Find("TutorialViewPage_1").GetComponent<TutorialViewPageModel>();
            tutorialViewPageViews[gmvPageVIdx++] = GameObject.Find("TutorialViewPage_2").GetComponent<TutorialViewPageView>();
            tutorialViewPageModels[gmvPageMIdx++] = GameObject.Find("TutorialViewPage_2").GetComponent<TutorialViewPageModel>();
            tutorialViewPageViews[gmvPageVIdx++] = GameObject.Find("TutorialViewPage_3").GetComponent<TutorialViewPageView>();
            tutorialViewPageModels[gmvPageMIdx++] = GameObject.Find("TutorialViewPage_4").GetComponent<TutorialViewPageModel>();
            tutorialViewPageViews[gmvPageVIdx++] = GameObject.Find("TutorialViewPage_4").GetComponent<TutorialViewPageView>();
            tutorialViewPageModels[gmvPageMIdx++] = GameObject.Find("TutorialViewPage_4").GetComponent<TutorialViewPageModel>();
            tutorialViewPageViews[gmvPageVIdx++] = GameObject.Find("TutorialViewPage_5").GetComponent<TutorialViewPageView>();
            tutorialViewPageModels[gmvPageMIdx++] = GameObject.Find("TutorialViewPage_5").GetComponent<TutorialViewPageModel>();
        }

        public void OnStart()
        {
            // ステージ番号を取得する処理を追加する
            tutorialView.gameObject.SetActive(false);
            SelectGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_select);
            // ステージ番号を取得する処理を追加する
            var saveDatas = SelectGameManager.Instance.SceneOwner.GetSaveDatas();
            var stageIndex = new IntReactiveProperty(saveDatas.sceneId);
            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // UI操作を許可
                    if (!stageSelectModel.SetButtonAndEventTriggerEnabled(true))
                        Debug.LogError("SetButtonAndEventTriggerEnabled");
                    switch (stageIndex.Value)
                    {
                        case 1:
                            _tween = DOVirtual.DelayedCall(0.5f, () =>
                            {
                                if (!tutorialView.gameObject.activeSelf)
                                {
                                    // チュートリアル画面の表示
                                    tutorialView.gameObject.SetActive(true);
                                    if (!tutorialView.SetPage(EnumTutorialPagesIndex.Page_1))
                                        Debug.LogError("ページ変更呼び出しの失敗");
                                    tutorialViewPageModels[(int)EnumTutorialPagesIndex.Page_1].SetSelectedGameObject();
                                }
                            });

                            break;
                        default:
                            _tween = DOVirtual.DelayedCall(5.0f, () =>
                            {
                                if (!tutorialView.gameObject.activeSelf)
                                {
                                    // チュートリアル画面の表示
                                    tutorialView.gameObject.SetActive(true);
                                    if (!tutorialView.SetPage(EnumTutorialPagesIndex.Page_1))
                                        Debug.LogError("ページ変更呼び出しの失敗");
                                    tutorialViewPageModels[(int)EnumTutorialPagesIndex.Page_1].SetSelectedGameObject();
                                }
                            });

                            break;
                    }
                })
                .AddTo(gameObject);
            foreach (var eventState in stageSelectModel.EventStates.Select((p, i) => new { Content = p, Index = i }))
            {
                eventState.Content.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (!stageSelectView.RenderLineStageContetsBetweenTargetPoints(eventState.Index, (EnumEventCommand)x, fadeImageView, _tween))
                            Debug.LogError("RenderLineStageContetsBetweenTargetPoints");
                    });
            }
            // ロードの完了
            IsCompleted.Value = true;
            // チュートリアル画面の操作
            for (var i = 0; i < tutorialViewPageModels.Length; i++)
            {
                var tmpIdx = i;
                tutorialViewPageModels[tmpIdx].EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理なし
                            break;
                        case EnumEventCommand.Selected:
                            SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            if (!tutorialView.PlayPagingAnimation((EnumTutorialPagesIndex)tmpIdx))
                                Debug.LogError("ページ変更アニメーション呼び出しの失敗");
                            break;
                        case EnumEventCommand.DeSelected:
                            // 処理なし
                            break;
                        case EnumEventCommand.Submited:
                            // 処理なし
                            break;
                        case EnumEventCommand.Canceled:
                            SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            if (!tutorialViewPageModels[tmpIdx].SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!tutorialViewPageModels[tmpIdx].SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // チュートリアル画面クローズのアニメーション
                            Observable.FromCoroutine<bool>(observer => tutorialView.PlayCloseAnimation(observer))
                                .Subscribe(_ =>
                                {
                                    tutorialView.gameObject.SetActive(false);
                                    if (!stageSelectModel.SetSelectedGameObjectOfAreaContentModels(stageIndex.Value))
                                        Debug.LogError("SetSelectedGameObjectOfAreaContentModels");
                                })
                                .AddTo(gameObject);
                            break;
                    }
                });
            }

            // uGUI制御（有効）
            this.UpdateAsObservable()
                .Select(_ => SelectGameManager.Instance)
                .Where(x => x != null)
                .Select(x => x.InputSystemsOwner)
                .Subscribe(x =>
                {
                    if (x.InputUI.Paused)
                    {
                        if (!tutorialView.gameObject.activeSelf)
                        {
                            // チュートリアル画面の表示
                            SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            tutorialView.gameObject.SetActive(true);
                            if (!tutorialView.SetPage(EnumTutorialPagesIndex.Page_1))
                                Debug.LogError("ページ変更呼び出しの失敗");
                            tutorialViewPageModels[(int)EnumTutorialPagesIndex.Page_1].SetSelectedGameObject();
                        }
                    }
                });
        }
    }
}
