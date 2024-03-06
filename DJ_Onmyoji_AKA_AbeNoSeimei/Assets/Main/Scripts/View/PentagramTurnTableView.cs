using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Audio;
using UniRx;
using UniRx.Triggers;
using Main.Common;

namespace Main.View
{
    /// <summary>
    /// ペンダグラムターンテーブル
    /// プレゼンタから伝達された入力を元に出力を行う
    /// Imageコンポーネントへ入力操作を行う
    /// ビュー
    /// </summary>
    public class PentagramTurnTableView : PentagramTurnTableCommonView, IPentagramTurnTableView
    {
        /// <summary>バックスピン回転数</summary>
        [SerializeField] private int backSpinCount = 5;
        /// <summary>演出の再生時間</summary>
        [SerializeField] private float[] durations = { 1.5f };

        public bool MoveSpin(BgmConfDetails bgmConfDetails)
        {
            try
            {
                if (!ControllAudio(bgmConfDetails))
                    throw new System.Exception("ControllAudio");
                float angle = bgmConfDetails.InputValue * -1f * angleCorrectionValue;
                image.transform.Rotate(new Vector3(0f, 0f, angle));
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool CalibrationToTarget(Transform transform)
        {
            try
            {
                var target = transform;
                var mainCamera = Camera.main;
                var parentRect = Transform.parent.GetComponent<RectTransform>();
                this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        // 2Dオブジェクトのワールド座標をスクリーン座標に変換
                        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);
                        // スクリーン座標→UIローカル座標変換
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            parentRect,
                            screenPosition,
                            mainCamera, // オーバーレイモードの場合はnull
                            out var uiLocalPos
                        );
                        // UI要素の位置を更新
                        Transform.localPosition = uiLocalPos;
                    });
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// オーディオ情報を操作
        /// 引数を元にオーディオ制御を行う
        /// ●再生速度の可変
        /// </summary>
        /// <param name="bgmConfDetails">BGM設定の詳細</param>
        /// <returns>成功／失敗</returns>
        private bool ControllAudio(BgmConfDetails bgmConfDetails)
        {
            try
            {
                if (!AudioOwner.ChangeSpeed(ClipToPlay.se_scratch_1, bgmConfDetails))
                    throw new System.Exception("ChangeSpeed");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSpriteIndex(float timeSec, float limitTimeSecMax)
        {
            return _utility.SetSpriteIndex(image, timeSec, limitTimeSecMax, sprites);
        }

        public IEnumerator PlayDirectionBackSpin(System.IObserver<bool> observer, JockeyCommandType jockeyCommandType)
        {
            switch (jockeyCommandType)
            {
                case JockeyCommandType.BackSpin:
                    IntReactiveProperty onNextCnt = new IntReactiveProperty();
                    onNextCnt.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (1 < x)
                                observer.OnNext(true);
                        });
                    Observable.FromCoroutine<bool>(observer => _utility.PlayBackSpinAnimation(observer, durations[0], backSpinCount, Transform))
                        .Subscribe(x =>
                        {
                            if (x)
                                onNextCnt.Value++;
                        })
                        .AddTo(gameObject);
                    // BGMをフェードアウト
                    Observable.FromCoroutine<bool>(observer => AudioOwner.PlayFadeOut(observer, durations[0]))
                        .Subscribe(x =>
                        {
                            if (x)
                            {
                                // 任意の時間帯まで再生時間を戻す
                                AudioOwner.PlayBack();
                                onNextCnt.Value++;
                            }
                        })
                        .AddTo(gameObject);
                    // バックスピンSEを再生
                    AudioOwner.PlaySFX(ClipToPlay.se_backspin);

                    break;
                default:
                    // それ以外のジョッキーコマンドは対象外
                    break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// ペンダグラムターンテーブル
    /// </summary>
    public interface IPentagramTurnTableView
    {
        /// <summary>
        /// UIオブジェクトを回転させる
        /// 引数を元にメンバー変数_playingSpinを更新する
        /// ●pbSpeedに渡された値を再生速度とする
        /// </summary>
        /// <param name="bgmConfDetails">BGM設定の詳細</param>
        /// <returns>成功／失敗</returns>
        public bool MoveSpin(BgmConfDetails bgmConfDetails);
        /// <summary>
        /// ターゲット位置を元に調整
        /// </summary>
        /// <param name="transform">ターゲット情報</param>
        /// <returns>成功／失敗</returns>
        public bool CalibrationToTarget(Transform transform);
        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetSpriteIndex(float timeSec, float limitTimeSecMax);
        /// <summary>
        /// バックスピン演出
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="jockeyCommandType">ジョッキーコマンドタイプ</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayDirectionBackSpin(System.IObserver<bool> observer, JockeyCommandType jockeyCommandType);
    }
}
