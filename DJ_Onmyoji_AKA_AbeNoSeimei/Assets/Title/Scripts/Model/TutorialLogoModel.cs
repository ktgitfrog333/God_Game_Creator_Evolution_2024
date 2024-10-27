using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Title.Model
{
    /// <summary>
    /// チュートリアルロゴ
    /// モデル
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class TutorialLogoModel : UIEventController, ITutorialLogoModel
    {
        /// <summary>ボタン</summary>
        [SerializeField] private Button button;
        /// <summary>イベントトリガー</summary>
        [SerializeField] private EventTrigger eventTrigger;

        private void Reset()
        {
            button = GetComponent<Button>();
            eventTrigger = GetComponent<EventTrigger>();
        }

        public bool SetButtonEnabled(bool enabled)
        {
            try
            {
                button.enabled = enabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            try
            {
                eventTrigger.enabled = enabled;

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
    /// チュートリアルロゴ
    /// インターフェース
    /// </summary>
    public interface ITutorialLogoModel
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
