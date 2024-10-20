using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Model
{
    /// <summary>
    /// イベントコントローラー
    /// </summary>
    public class UIEventController : MonoBehaviour
    {
        /// <summary>
        /// 実行イベントの監視
        /// -1 : Default
        /// 0 : Selected
        /// 1 : DeSelected
        /// 2 : Submited
        /// 3 : Canceled
        /// 4 : AnyKeysPushed
        /// </summary>
        protected readonly IntReactiveProperty _eventState = new IntReactiveProperty((int)EnumEventCommand.Default);
        /// <summary>
        /// 実行イベントの監視
        /// -1 : Default
        /// 0 : Selected
        /// 1 : DeSelected
        /// 2 : Submited
        /// 3 : Canceled
        /// 4 : AnyKeysPushed
        /// </summary>
        public IReactiveProperty<int> EventState => _eventState;
        /// <summary>イベントシステム</summary>
        private EventSystem _eventSystem;
        /// <summary>uGUIオブジェクト制御ユーティリティ</summary>
        protected MainUGUIsModelUtility _mainUGUIsModelUtility = new MainUGUIsModelUtility();

        private void Awake()
        {
            _eventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            break;
                        case EnumEventCommand.Selected:
                            if (!OnUpdateEventState(_eventState.Value))
                                Debug.LogError("OnUpdateEventState");
                            break;
                        case EnumEventCommand.DeSelected:
                            break;
                        case EnumEventCommand.Submited:
                            if (!OnUpdateEventState(_eventState.Value))
                                Debug.LogError("OnUpdateEventState");
                            break;
                        case EnumEventCommand.Canceled:
                            if (!OnUpdateEventState(_eventState.Value))
                                Debug.LogError("OnUpdateEventState");
                            break;
                        default:
                            break;
                    }
                });
        }

        protected virtual void OnEnable()
        {
            if (!EnumEventCommand.Default.Equals((EnumEventCommand)_eventState.Value))
                _eventState.Value = (int)EnumEventCommand.Default;
        }

        /// <summary>
        /// デフォルト選択切り替え
        /// </summary>
        public void SetSelectedGameObject()
        {
            if (_eventSystem == null)
                _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            _eventSystem.SetSelectedGameObject(gameObject);
        }

        /// <summary>
        /// 選択された時に発火するイベント
        /// </summary>
        public void Selected()
        {
            _eventState.Value = (int)EnumEventCommand.Selected;
        }

        /// <summary>
        /// 選択されなかった時に発火するイベント
        /// </summary>
        public void DeSelected()
        {
            _eventState.Value = (int)EnumEventCommand.DeSelected;
        }

        /// <summary>
        /// 確定時に発火するイベント
        /// </summary>
        public virtual void Submited()
        {
            _eventState.Value = (int)EnumEventCommand.Submited;
        }

        /// <summary>
        /// キャンセル時に発火するイベント
        /// </summary>
        public void Canceled()
        {
            _eventState.Value = (int)EnumEventCommand.Canceled;
        }

        /// <summary>
        /// UIイベントが更新された際に発火。イベント状態を取得。
        /// </summary>
        /// <param name="eventState">イベント状態</param>
        /// <returns>成功／失敗</returns>
        private bool OnUpdateEventState(int eventState)
        {
            try
            {
                var analyticsOwner = MainGameManager.Instance.AnalyticsOwner;
                analyticsOwner.InitializeAsyncSuccessed.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (x)
                            if (!analyticsOwner.OnUpdateEventState(eventState, name))
                                Debug.LogError("OnUpdateEventState");
                    });
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
