using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 抽象ゲーム画面
    /// ビュー
    /// </summary>
    public class AbstractGameView : MonoBehaviour, IAbstractGameView
    {
        protected virtual void OnEnable()
        {
            Time.timeScale = 0f;
            Debug.LogWarning($"Time.timeScale:[{Time.timeScale}]");
        }

        protected virtual void OnDisable()
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
    /// 抽象ゲーム画面
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IAbstractGameView
    {
        /// <summary>
        /// ゲームオブジェクトの有効／無効をセット
        /// </summary>
        /// <param name="active">有効／無効状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetActiveGameObject(bool active);
    }
}
