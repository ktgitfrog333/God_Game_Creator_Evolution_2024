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
        /// <summary>遅延処理</summary>
        private Tween _tween;

        private void Reset()
        {
            // フェードのビューとモデルをセット
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();
            stageSelectView = GameObject.Find("StageSelect").GetComponent<StageSelectView>();
            stageSelectModel = GameObject.Find("StageSelect").GetComponent<StageSelectModel>();
        }

        public void OnStart()
        {
            // ステージ番号を取得する処理を追加する
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
        }
    }
}
