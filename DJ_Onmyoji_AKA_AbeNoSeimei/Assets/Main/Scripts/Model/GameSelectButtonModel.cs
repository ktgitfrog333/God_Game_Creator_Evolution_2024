using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Main.Common;

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

        private void Reset()
        {
            gameContentsConfig = GetComponent<GameContentsConfig>();
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
    }

    /// <summary>
    /// モデル
    /// ステージ選択へ戻るボタン
    /// インターフェース
    /// </summary>
    public interface IGameSelectButtonModel : IButtonCommon
    {

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
