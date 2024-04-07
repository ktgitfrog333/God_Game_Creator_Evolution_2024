using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// クリア画面
    /// </summary>
    public class ClearView : MonoBehaviour, IClearView
    {
        /// <summary>クリア結果のコンテンツ</summary>
        [SerializeField] private ClearResultContents[] clearResultContents;

        private void Reset()
        {
            clearResultContents = GetComponentsInChildren<ClearResultContents>();
        }

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

        public bool SetContents(ClearResultContentsState clearResultContentsState)
        {
            try
            {
                foreach (var item in clearResultContents.Select((p, i) => new { Content = p, Index = i}))
                {
                    switch (item.Index)
                    {
                        case 0:
                            if (!item.Content.SetTimeSec(clearResultContentsState.timeSec))
                                throw new System.Exception("SetTimeSec");

                            break;
                        case 1:
                            // TODO:プレイヤーレベルを表示
                            break;
                        case 2:
                            // TODO:残り体力を表示
                            break;
                        case 3:
                            // TODO:倒した敵の数
                            break;
                        case 4:
                            if (!item.Content.SetSoulMoney(clearResultContentsState.soulMoney))
                                throw new System.Exception("SetSoulMoney");

                            break;
                        default:
                            break;
                    }
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
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
    /// クリア画面
    /// インターフェース
    /// </summary>
    public interface IClearView
    {
        /// <summary>
        /// コンテンツをセット
        /// </summary>
        /// <param name="clearResultContentsState">クリア結果のコンテンツのステート</param>
        /// <returns>成功／失敗</returns>
        public bool SetContents(ClearResultContentsState clearResultContentsState);
        /// <summary>
        /// ゲームオブジェクトの有効／無効をセット
        /// </summary>
        /// <param name="active">有効／無効状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetActiveGameObject(bool active);
    }
}
