using Select.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Select.Model
{
    /// <summary>
    /// ステージセレクト
    /// モデル
    /// </summary>
    public class StageSelectModel : MonoBehaviour, IStageSelectModel
    {
        /// <summary>エリアコンテンツのモデル</summary>
        [SerializeField] private AreaContentModel[] areaContentModels;
        /// <summary>実行イベント</summary>
        public IReactiveProperty<int>[] EventStates => areaContentModels.Select(q => q.EventState).ToArray();
        /// <summary>ボタン</summary>
        public Button[] Buttons => areaContentModels.Select(q => q.GetComponent<Button>()).ToArray();

        private void Reset()
        {
            areaContentModels = GetComponentsInChildren<AreaContentModel>();
        }

        private void Start()
        {
            // ステージ番号を取得する処理を追加する
            var saveDatas = SelectGameManager.Instance.SceneOwner.GetSaveDatas();
            var stageIndex = new IntReactiveProperty(saveDatas.sceneId);
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
                        }
                    });
            }
        }

        public bool SetButtonAndEventTriggerEnabled(bool enabled)
        {
            try
            {
                foreach (var areaContentModel in areaContentModels)
                {
                    if (!areaContentModel.SetButtonEnabled(enabled))
                        throw new System.Exception("SetButtonEnabled");
                    if (!areaContentModel.SetEventTriggerEnabled(enabled))
                        throw new System.Exception("SetEventTriggerEnabled");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// ステージセレクト
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IStageSelectModel
    {
        /// <summary>
        /// ボタンとイベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonAndEventTriggerEnabled(bool enabled);
    }
}
