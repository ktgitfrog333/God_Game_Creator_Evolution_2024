using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Audio;
using UniRx;
using UniRx.Triggers;
using Main.Common;
using DG.Tweening;
using UnityEngine.UI;

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
        /// <summary>スリップループ回転角度</summary>
        [SerializeField] private float slipLoopAngle = 30f;
        /// <summary>スリップループ用の変更前角度</summary>
        private Vector3? _fromAngle;
        /// <summary>ループエリア</summary>
        [SerializeField] protected Image[] loopImage;
        /// <summary>ループエリアのTransform</summary>
        private RectTransform[] loopImageRectTransform;
        /// <summary>ループする際にアタッチするオブジェクト</summary>
        public Transform parentObjectLoopOn;
        /// <summary>ループしない際にアタッチするオブジェクト</summary>
        public Transform parentObjectLoopOff; // GUIにアタッチしないと表示されない
        /// <summary>今の拍数を保存</summary>
        private int nowBeatLength;
        /// <summary>初期状態のPositionを保存</summary>
        private Vector3[] loopImageStartPosition;
        /// <summary>初期状態のRotationを保存</summary>
        private Quaternion[] loopImageStartRotation;
        /// <summary>ループイメージのカラー</summary>
        private Color color;

        protected virtual void Start()
        {
            loopImageRectTransform = new RectTransform[loopImage.Length];
            loopImageStartPosition = new Vector3[loopImage.Length];
            loopImageStartRotation = new Quaternion[loopImage.Length];
            for (int index = 0; index < loopImage.Length; index++)
            {
                loopImageRectTransform[index] = loopImage[index].GetComponent<RectTransform>();
                loopImage[index].enabled = false;
                loopImageStartPosition[index] = loopImageRectTransform[index].localPosition;
                loopImageStartRotation[index] = loopImageRectTransform[index].localRotation;

                if (index == 0)
                    color = loopImage[index].color;
                else
                    loopImage[index].color = color;
            }
            base.Start();
        }

        public bool MoveSpin(BgmConfDetails bgmConfDetails)
        {
            try
            {
                if (0f < Time.timeScale)
                {
                    if (!ControllAudio(bgmConfDetails))
                        throw new System.Exception("ControllAudio");
                    float angle = bgmConfDetails.InputValue * angleCorrectionValue;
                    image.transform.Rotate(new Vector3(0f, 0f, angle));
                }
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
                                if (!AudioOwner.SetVolumeOn())
                                    Debug.LogError("SetVolumeOn");
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

        public IEnumerator MoveSpin(System.IObserver<bool> observer, InputSlipLoopState inputSlipLoopState)
        {
            var angle = BeatLengthApp.GetTotalReverse(inputSlipLoopState, slipLoopAngle);
            var beat = AudioOwner.GetBeatBGM();
            if (_fromAngle == null)
                _fromAngle = Transform.localEulerAngles;
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(Transform.DOLocalRotate(_fromAngle.Value + new Vector3(0f, 0f, angle), beat * .3f)
                    .SetEase(Ease.OutBack)
                    .From(_fromAngle.Value))
                .AppendCallback(() => observer.OnNext(true))
                .Append(Transform.DOLocalRotate(_fromAngle.Value, beat * .7f)
                    .SetEase(Ease.Linear));
            AudioOwner.PlayBack(inputSlipLoopState);

            yield return null;
        }

        public bool ResetFromAngle()
        {
            try
            {
                _fromAngle = null;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool AdjustBGM(JockeyCommandType jockeyCommandTypePrevious, JockeyCommandType jockeyCommandTypeCurrent)
        {
            switch (jockeyCommandTypePrevious)
            {
                case JockeyCommandType.SlipLoop:
                    switch (jockeyCommandTypeCurrent)
                    {
                        case JockeyCommandType.SlipLoop:
                            return true;
                        default:
                            return AudioOwner.AdjustBGM();
                    }
                default:
                    return true;
            }
        }

        public void SetLoopImage(bool isLooping)
        {
            for (int index = 0; index < loopImage.Length; index++)
            {
                if (isLooping)
                {
                    loopImageRectTransform[index].SetParent(parentObjectLoopOff);
                    SetLoopImageEnabled();
                }
                else
                {
                    loopImageRectTransform[index].SetParent(parentObjectLoopOn);
                    loopImageRectTransform[index].localPosition = loopImageStartPosition[index];
                    loopImageRectTransform[index].localRotation = loopImageStartRotation[index];
                    loopImage[index].enabled = false;
                }
            }


        }

        public void SetBeatLength(float beatLength)
        {
            nowBeatLength = (int)beatLength;
            SetLoopImageEnabled();
        }

        public void SetLoopImageEnabled()
        {
            for (int index = 0; index < loopImage.Length; index++)
                loopImage[index].enabled = false;

            if (nowBeatLength != 0)
            {
                loopImage[0].enabled = true;
                loopImage[nowBeatLength].enabled = true;
            }

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
        /// UIオブジェクトを回転させる
        /// 引数を元に角度を変更する
        /// BGMの拍を戻す処理を呼び出す
        /// </summary>
        /// <param name="inputSlipLoopState">スリップループの入力情報</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator MoveSpin(System.IObserver<bool> observer, InputSlipLoopState inputSlipLoopState);
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
        /// スリップループ用の変更前角度をリセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool ResetFromAngle();
        /// <summary>
        /// バックスピン演出
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="jockeyCommandType">ジョッキーコマンドタイプ</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayDirectionBackSpin(System.IObserver<bool> observer, JockeyCommandType jockeyCommandType);
        /// <summary>
        /// BGMをアジャストする
        /// 直前がループだったが元へ戻った場合はBGMのタイムラインをアジャストする
        /// </summary>
        /// <param name="jockeyCommandTypePrevious">（直前の入力）ジョッキーコマンドタイプ</param>
        /// <param name="jockeyCommandTypeCurrent">（現在の入力）ジョッキーコマンドタイプ</param>
        /// <returns>成功／失敗</returns>
        public bool AdjustBGM(JockeyCommandType jockeyCommandTypePrevious, JockeyCommandType jockeyCommandTypeCurrent);
    }
}
