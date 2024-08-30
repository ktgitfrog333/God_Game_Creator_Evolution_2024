using System.Collections;
using System.Collections.Generic;
using Select.Audio;
using Select.Common;
using UnityEngine;
using UniRx;

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
        /// <summary>日本地図のビュー</summary>
        [SerializeField] private MapOfJapanView mapOfJapanView;

        private void Reset()
        {
            stageContetsViews = GetComponentsInChildren<StageContetsView>();
            lineRectTransformView = GetComponentInChildren<LineRectTransformView>();
            circleCursorViews = GetComponentsInChildren<CircleCursorView>();
            mapOfJapanView = GetComponentInChildren<MapOfJapanView>();
        }

        public bool RenderLineStageContetsBetweenTargetPoints(int index, EnumEventCommand enumEventCommand, FadeImageView fadeImageView)
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
                        if (!stageContetsViews[0].RenderTargetMark(index))
                            Debug.LogError("RenderTargetMark");
                        if (!mapOfJapanView.RenderTargetMark(index))
                            Debug.LogError("RenderTargetMark");
                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);

                        break;
                    case EnumEventCommand.Canceled:
                        // キャンセルSEを再生
                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                        // シーン読み込み時のアニメーション
                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                            .Subscribe(_ =>
                            {
                                SelectGameManager.Instance.SceneOwner.LoadTitleScene();
                            })
                            .AddTo(gameObject);

                        break;
                    case EnumEventCommand.Submited:
                        // 決定SEを再生
                        SelectGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                        // シーン読み込み時のアニメーション
                        Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                            .Subscribe(_ =>
                            {
                                // メインシーンを実装
                                SelectGameManager.Instance.SceneOwner.LoadMainScene();
                            })
                            .AddTo(gameObject);

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
        /// <param name="fadeImageView">フェードイメージのビュー</param>
        /// <returns>成功／失敗</returns>
        public bool RenderLineStageContetsBetweenTargetPoints(int index, EnumEventCommand enumEventCommand, FadeImageView fadeImageView);
    }
}
