using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Main.Common;
using UniRx;
using System.Linq;
using Main.Utility;
using Main.View;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// ステージ選択へ戻るボタン
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(GameContentsConfig))]
    public class GameSelectButtonModel : UIEventController, IButtonEventTriggerModel, IGameSelectButtonModel
    {
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;
        /// <summary>設定ファイル</summary>
        [SerializeField] private GameContentsConfig gameContentsConfig;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>TextMeshProの表示制御のビュー</summary>
        [SerializeField] private TextTMPView textTMPView;
        /// <summary>シーンID</summary>
        private int _sceneId = -1;

        private void Reset()
        {
            gameContentsConfig = GetComponent<GameContentsConfig>();
            textTMPView = GetComponentInChildren<TextTMPView>();
        }

        private void Start()
        {
            var utility = new MainCommonUtility();
            _sceneId = utility.UserDataSingleton.UserBean.sceneId;
            BoolReactiveProperty isInteractable = new BoolReactiveProperty();
            isInteractable.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        if (!textTMPView.SetColorToEnabled())
                            Debug.LogError("SetColorToEnabled");
                    }
                    else
                    {
                        if (!textTMPView.SetColorToDisabled())
                            Debug.LogError("SetColorToDisabled");
                    }
                });
            if (!_mainUGUIsModelUtility.ObserveButtonInteractable(_button, isInteractable, Transform))
                Debug.LogError("SetButtonColorToDisabledOfButton");
            if (!CheckSelectedIDAndSetUIControllEnabled(new ReactiveCollection<RewardID>()))
                Debug.LogError("CheckSelectedIDAndSetUIControllEnabled");
        }

        public override void Submited()
        {
            if (_mainUGUIsModelUtility.IsInteractableOfButton(_button, Transform))
                base.Submited();
        }

        public bool SetButtonEnabled(bool enabled)
        {
            return _mainUGUIsModelUtility.SetButtonEnabledOfButton(enabled, _button, Transform);
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            return _mainUGUIsModelUtility.SetEventTriggerEnabledOfEventTrigger(enabled, _eventTrigger, Transform);
        }

        public bool SetNavigation(Button prevButton, Button nextButton)
        {
            return _mainUGUIsModelUtility.SetNavigationOfButton(prevButton, nextButton, _button, Transform);
        }

        public bool CheckSelectedIDAndSetUIControllEnabled(ReactiveCollection<RewardID> selectedRewardIDs)
        {
            if (0 < _sceneId)
                // ステージ0の場合のみ
                return true;

            var length = selectedRewardIDs.Where(q => q.Equals(RewardID.RE0000) ||
                q.Equals(RewardID.RE0001) ||
                q.Equals(RewardID.RE0002))
                .ToArray()
                .Length;

            return _mainUGUIsModelUtility.SetInteractableOfButton(2 < length, _button, Transform);
        }
    }

    /// <summary>
    /// モデル
    /// ステージ選択へ戻るボタン
    /// インターフェース
    /// </summary>
    public interface IGameSelectButtonModel : IButtonCommon
    {
        /// <summary>
        /// リワードIDが0、1、2が選択されているかを判定
        /// 確定ボタンのボタンコンポーネントを有効／無効へ切り替える
        /// </summary>
        /// <param name="SelectedRewardIDs">選択したリワードID</param>
        /// <returns>成功／失敗</returns>
        public bool CheckSelectedIDAndSetUIControllEnabled(ReactiveCollection<RewardID> selectedRewardIDs);
    }

    public interface IButtonCommon
    {
        /// <summary>
        /// ボタンのナビゲーションをセット
        /// </summary>
        /// <param name="prevButton">前のボタン</param>
        /// <param name="nextButton">次のボタン</param>
        /// <returns>成功／失敗</returns>
        public bool SetNavigation(Button prevButton, Button nextButton);
    }
}
