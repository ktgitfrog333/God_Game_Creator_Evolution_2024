using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Select.Utility
{
    /// <summary>
    /// uGUIオブジェクト制御
    /// ユーティリティ
    /// </summary>
    public class SelectUGUIsModelUtility : ISelectUGUIsModelUtility
    {
        public bool SetButtonEnabledOfButton(bool enabled, Button button, Transform transform=null)
        {
            try
            {
                if (button == null)
                {
                    if (transform == null)
                        throw new System.ArgumentNullException("transformがnull");

                    button = transform.GetComponent<Button>();
                }
                button.enabled = enabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetEventTriggerEnabledOfEventTrigger(bool enabled, EventTrigger eventTrigger, Transform transform=null)
        {
            try
            {
                if (eventTrigger == null)
                {
                    if (transform == null)
                        throw new System.ArgumentNullException("transformがnull");

                    eventTrigger = transform.GetComponent<EventTrigger>();
                }
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
    /// uGUIオブジェクト制御
    /// インターフェース
    /// </summary>
    public interface ISelectUGUIsModelUtility
    {
        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <param name="button">ボタン</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonEnabledOfButton(bool enabled, Button button, Transform transform=null);
        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <param name="eventTrigger">イベントトリガー</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventTriggerEnabledOfEventTrigger(bool enabled, EventTrigger eventTrigger, Transform transform=null);
    }
}
