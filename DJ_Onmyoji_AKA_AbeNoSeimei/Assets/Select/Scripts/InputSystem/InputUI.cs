using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Select.InputSystem
{
    /// <summary>
    /// UI用のInputAction
    /// </summary>
    public class InputUI : MonoBehaviour, IInputSystemsOwner
    {
        /// <summary>ポーズ入力</summary>
        private bool _paused;
        /// <summary>ポーズ入力</summary>
        public bool Paused => _paused;
        /// <summary>
        /// Pauseのアクションに応じてフラグを更新
        /// </summary>
        /// <param name="context">コールバック</param>
        public void OnPaused(InputAction.CallbackContext context)
        {
            _paused = context.ReadValueAsButton();
        }

        public void DisableAll()
        {
            _paused = false;
        }
    }
}
