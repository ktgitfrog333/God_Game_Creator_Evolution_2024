using System.Collections;
using System.Collections.Generic;
using Select.Common;
using UnityEngine;

namespace Select.View
{
    /// <summary>
    /// ステージセレクト
    /// ビュー
    /// </summary>
    public class StageSelectView : MonoBehaviour, IStageSelectView
    {
        /// <summary>ステージコンテンツ</summary>
        [SerializeField] private StageContetsView[] stageContetsViews;
        /// <summary>線を引く用のトランスフォーム</summary>
        [SerializeField] private LineRectTransformView lineRectTransformView;
        /// <summary>サークルカーソル</summary>
        [SerializeField] private CircleCursorView[] circleCursorViews;

        private void Reset()
        {
            stageContetsViews = GetComponentsInChildren<StageContetsView>();
            lineRectTransformView = GetComponentInChildren<LineRectTransformView>();
            circleCursorViews = GetComponentsInChildren<CircleCursorView>();
        }

        public bool RenderLineStageContetsBetweenTargetPoints(int index, EnumEventCommand enumEventCommand)
        {
            try
            {
                switch (enumEventCommand)
                {
                    case EnumEventCommand.Selected:
                        if (!lineRectTransformView.RenderLineFromPointBetweenToPoint(stageContetsViews[0].AnchoredPositionsInChild1[index].position, stageContetsViews[1].AnchoredPositionsInChild[index].position))
                            Debug.LogError("RenderLineFromPointBetweenToPoint");
                        if (!circleCursorViews[0].SetAnchorPosition(stageContetsViews[0].AnchoredPositionsInChild[index].position))
                            Debug.LogError("SetAnchorPosition");
                        if (!circleCursorViews[0].SetSizeDelta(stageContetsViews[0].AnchoredPositionsInChild[index].sizeDelta))
                            Debug.LogError("SetAnchorPosition");
                        if (!circleCursorViews[1].SetAnchorPosition(stageContetsViews[1].AnchoredPositionsInChild[index].position))
                            Debug.LogError("SetAnchorPosition");

                        break;
                    case EnumEventCommand.DeSelected:
                        break;
                    default:
                        // 処理無し
                        break;
                }

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
    /// ステージセレクト
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IStageSelectView
    {
        /// <summary>
        /// ステージコンテンツからターゲットポインツへ線を描画
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="enumEventCommand">イベントコマンド入力</param>
        /// <returns>成功／失敗</returns>
        public bool RenderLineStageContetsBetweenTargetPoints(int index, EnumEventCommand enumEventCommand);
    }
}
