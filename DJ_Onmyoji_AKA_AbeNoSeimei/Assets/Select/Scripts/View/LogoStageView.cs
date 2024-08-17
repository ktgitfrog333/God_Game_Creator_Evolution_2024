using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;
using UnityEngine.UI;

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
        /// <summary>ステージロゴ背景</summary>
        [SerializeField] private Image stageImage; // Imageコンポーネントの参照
        /// <summary>選択可能カラー</summary>
        [SerializeField] private Color targetColor;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        private void Reset()
        {
            //selectStageFrame = GameObject.Find("SelectStageFrame");
            stageImage = GetComponent<Image>(); // Imageコンポーネントの取得
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

        public bool RenderTargetMark()
        {
            try
            {
                stageImage.color = targetColor;
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
        /// <summary>
        /// 選択済みマークを表示
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool RenderTargetMark();
    }
}
