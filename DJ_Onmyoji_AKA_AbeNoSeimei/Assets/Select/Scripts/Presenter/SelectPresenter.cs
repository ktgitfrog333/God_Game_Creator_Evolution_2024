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
using Unity.VisualScripting;

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
        /// <summary>チュートリアル画面のビュー</summary>
        [SerializeField] private TutorialView tutorialView;
        /// <summary>チュートリアル画面ページのビュー</summary>
        [SerializeField] private TutorialViewPageView[] tutorialViewPageViews;
        /// <summary>遊び方確認ページのモデル</summary>
        [SerializeField] private TutorialViewPageModel[] tutorialViewPageModels;
        /// <summary>エリアコンテンツのモデル</summary>
        [SerializeField] private AreaContentModel[] areaContentModels;
        /// <summary>実行イベント</summary>
        public IReactiveProperty<int>[] EventStates => areaContentModels.Select(q => q.EventState).ToArray();
        /// <summary>プレゼンタの読み込み完了</summary>
        public IReactiveProperty<bool> IsCompleted { get; private set; } = new BoolReactiveProperty();

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
            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // UI操作を許可
                    if (!stageSelectModel.SetButtonAndEventTriggerEnabled(true))
                        Debug.LogError("SetButtonAndEventTriggerEnabled");
                })
                .AddTo(gameObject);
            // ステージ番号を取得する処理を追加する
            var saveDatas = SelectGameManager.Instance.SceneOwner.GetSaveDatas();
            var stageIndex = new IntReactiveProperty(saveDatas.sceneId);
            foreach (var eventState in stageSelectModel.EventStates.Select((p, i) => new { Content = p, Index = i }))
            {
                eventState.Content.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (!stageSelectView.RenderLineStageContetsBetweenTargetPoints(eventState.Index, (EnumEventCommand)x, fadeImageView))
                            Debug.LogError("RenderLineStageContetsBetweenTargetPoints");
                    });
            }
            foreach (var eventState in EventStates.Select((p, i) => new { Content = p, Index = i }))
            {
                eventState.Content.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Selected:
                                stageIndex.Value = areaContentModels[eventState.Index].Index;

                                break;
                            case EnumEventCommand.Canceled:
                                if (!areaContentModels[eventState.Index].SetButtonEnabled(false))
                                    Debug.LogError("SetButtonEnabled");
                                if (!areaContentModels[eventState.Index].SetEventTriggerEnabled(false))
                                    Debug.LogError("SetEventTriggerEnabled");

                                break;
                            // case EnumEventCommand.Submited:
                            case EnumEventCommand.AnyKeysPushed:
                                // メインシーンへの遷移
                                saveDatas.sceneId = areaContentModels[eventState.Index].Index;
                                if (!SelectGameManager.Instance.SceneOwner.SetSaveDatas(saveDatas))
                                    Debug.LogError("シーンID更新処理呼び出しの失敗");
                                if (!areaContentModels[eventState.Index].SetButtonEnabled(false))
                                    Debug.LogError("SetButtonEnabled");
                                if (!areaContentModels[eventState.Index].SetEventTriggerEnabled(false))
                                    Debug.LogError("SetEventTriggerEnabled");

                                break;
                            // case EnumEventCommand.AnyKeysPushed:
                            case EnumEventCommand.Submited:
                                // チュートリアル画面の表示
                                SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                                //
                                tutorialView.gameObject.SetActive(true);
                                if (!tutorialView.SetPage(EnumTutorialPagesIndex.Page_1))
                                    Debug.LogError("ページ変更呼び出しの失敗");
                                tutorialViewPageModels[(int)EnumTutorialPagesIndex.Page_1].SetSelectedGameObject();
                                break;
                        }
                    });
            }
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
                                    // 閉じた時に次行の地方が選択される為、-1にして遷移前と同じにする
                                    areaContentModels[stageIndex.Value - 1].SetSelectedGameObject();
                                })
                                .AddTo(gameObject);
                            break;
                    }
                });
            }
            // ロードの完了
            IsCompleted.Value = true;
        }
    }
}
