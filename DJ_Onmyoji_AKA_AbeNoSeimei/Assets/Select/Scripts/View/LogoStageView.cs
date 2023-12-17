using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;

namespace Select.View
{
    /// <summary>
    /// ビュー
    /// ロゴステージ
    /// </summary>
    public class LogoStageView : MonoBehaviour, ILogoStageView
    {
        /// <summary>ステージ選択のフレーム</summary>
        [SerializeField] private GameObject selectStageFrame;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        private void Reset()
        {
            selectStageFrame = GameObject.Find("SelectStageFrame");
        }

        public bool MoveSelectStageFrame()
        {
            try
            {
                if (_transform == null)
                    _transform = transform;
                selectStageFrame.transform.position = _transform.position;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RenderDisableMark()
        {
            try
            {
                Debug.Log("選択不可");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool RenderClearMark()
        {
            try
            {
                Debug.Log("クリア済み");
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
    /// ロゴステージ
    /// インターフェース
    /// </summary>
    public interface ILogoStageView
    {
        /// <summary>
        /// ステージ選択のフレームを移動して選択させる
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool MoveSelectStageFrame();
        /// <summary>
        /// T.B.D 選択不可マークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderDisableMark();
        /// <summary>
        /// T.B.D クリア済みマークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderClearMark();
    }
}
