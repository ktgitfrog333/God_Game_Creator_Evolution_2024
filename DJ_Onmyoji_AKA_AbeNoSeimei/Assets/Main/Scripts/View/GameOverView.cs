using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ゲームオーバー画面
    /// </summary>
    public class GameOverView : MonoBehaviour, IGameOverView
    {
        private void OnEnable()
        {
            Time.timeScale = 0f;
            Debug.LogWarning($"Time.timeScale:[{Time.timeScale}]");
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            Debug.LogWarning($"Time.timeScale:[{Time.timeScale}]");
        }

        public bool SetActiveGameObject(bool active)
        {
            try
            {
                gameObject.SetActive(active);

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
    /// ビュー
    /// ゲームオーバー画面
    /// インターフェース
    /// </summary>
    public interface IGameOverView
    {
        /// <summary>
        /// ゲームオブジェクトの有効／無効をセット
        /// </summary>
        /// <param name="active">有効／無効状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetActiveGameObject(bool active);
    }

}
