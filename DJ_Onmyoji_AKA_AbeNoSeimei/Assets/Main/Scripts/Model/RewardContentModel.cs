using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Main.Common;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// クリア報酬のコンテンツ
    /// モデル
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class RewardContentModel : UIEventController, IButtonEventTriggerModel, IRewardContentModel
    {
        /// <summary>ボタン</summary>
        [SerializeField] private Button button;
        /// <summary>ボタン</summary>
        public Button Button => button;
        /// <summary>イベントトリガー</summary>
        [SerializeField] private EventTrigger eventTrigger;
        /// <summary>クリア報酬のコンテンツのプロパティ</summary>
        private RewardContentProp _rewardContentProp;
        /// <summary>クリア報酬のコンテンツのプロパティ</summary>
        public RewardContentProp RewardContentProp => _rewardContentProp;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>現在のサイズ</summary>
        public Vector2 CurrentSizeDelta => (Transform as RectTransform).sizeDelta * new Vector2(Transform.localScale.x, Transform.localScale.y);
        /// <summary>チェック状態か</summary>
        public IReactiveProperty<int> CheckState { get; private set; } = new IntReactiveProperty();

        public bool SetButtonEnabled(bool enabled)
        {
            return _mainUGUIsModelUtility.SetButtonEnabledOfButton(enabled, button);
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            return _mainUGUIsModelUtility.SetEventTriggerEnabledOfEventTrigger(enabled, eventTrigger);
        }

        public bool SetNavigationOfButton(Button prevButton, Button nextButton)
        {
            try
            {
                Navigation navigation = button.navigation;
                if (prevButton != null)
                    navigation.selectOnLeft = prevButton;
                if (nextButton != null)
                    navigation.selectOnRight = nextButton;
                button.navigation = navigation;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetRewardContentProp(RewardContentProp rewardContentProp)
        {
            try
            {
                _rewardContentProp = rewardContentProp;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool Check(CheckState checkState, bool absolute=false)
        {
            try
            {
                if (absolute)
                {
                    CheckState.Value = (int)checkState;
                    return true;
                }

                if (CheckState.Value != (int)checkState &&
                    CheckState.Value != (int)Common.CheckState.Disabled)
                    CheckState.Value = (int)checkState;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool Disable()
        {
            try
            {
                CheckState.Value = (int)Common.CheckState.Disabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            button = GetComponent<Button>();
            eventTrigger = GetComponent<EventTrigger>();
        }
    }

    /// <summary>
    /// クリア報酬のコンテンツ
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IRewardContentModel
    {
        /// <summary>
        /// ボタンのナビゲーションをセット
        /// </summary>
        /// <param name="prevButton">前のボタン</param>
        /// <param name="nextButton">次のボタン</param>
        /// <returns>成功／失敗</returns>
        public bool SetNavigationOfButton(Button prevButton, Button nextButton);
        /// <summary>
        /// クリア報酬のコンテンツのプロパティをセット
        /// </summary>
        /// <param name="rewardContentProp">クリア報酬のコンテンツのプロパティ</param>
        /// <returns>成功／失敗</returns>
        public bool SetRewardContentProp(RewardContentProp rewardContentProp);
        /// <summary>
        /// チェック状態の可否
        /// </summary>
        /// <param name="checkState">チェック状態</param>
        /// <param name="absolute">強制更新モード</param>
        /// <returns>成功／失敗</returns>
        public bool Check(CheckState checkState, bool absolute=false);
        /// <summary>
        /// チェック状態の無効
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool Disable();
    }

    /// <summary>
    /// ボタンとイベントトリガー
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IButtonEventTriggerModel
    {
        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonEnabled(bool enabled);
        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventTriggerEnabled(bool enabled);
    }
}
