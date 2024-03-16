using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Select.Common;
using Select.Model;
using Select.Test.Driver;
using Select.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Select.Presenter
{
    /// <summary>
    /// ステージセレクト
    /// プレゼンタデモ
    /// </summary>
    public class StageSelectPresenterDemo : MonoBehaviour
    {
        [SerializeField] private StageSelectView stageSelectView;
        [SerializeField] private StageSelectModel stageSelectModel;
        public IReactiveProperty<bool> IsCompleted { get; private set; } = new BoolReactiveProperty();

        private void Reset()
        {
            stageSelectView = GameObject.Find("StageSelect").GetComponent<StageSelectView>();
            stageSelectModel = GameObject.Find("StageSelect").GetComponent<StageSelectModel>();
        }

        private void Start()
        {
            foreach (var eventState in stageSelectModel.EventStates.Select((p, i) => new { Content = p, Index = i}))
            {
                eventState.Content.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (!stageSelectView.RenderLineStageContetsBetweenTargetPoints(eventState.Index, (EnumEventCommand)x))
                            Debug.LogError("RenderLineStageContetsBetweenTargetPoints");
                    });
            }

            // ロードの完了
            IsCompleted.Value = true;
        }
    }
}
