using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// カウントダウンタイマーの情報に合わせてUIを変化させる
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ClearCountdownTimerCircleView : MonoBehaviour, IClearCountdownTimerCircleView, IClearCountdownTimerCircleBossDirectionView
    {
        /// <summary>対象の画像</summary>
        [SerializeField] private Image image;
        /// <summary>ユーティリティ</summary>
        private MainViewUtility _utility = new MainViewUtility();
        /// <summary>マスクする角度の割合（0f~1f）</summary>
        [SerializeField, Range(0f, 1f)] private float maskAngle = .2f;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        [SerializeField] private Color32[] colors = new Color32[]
        {
            new Color32()
            {
                r = 248,
                g = 167,
                b = 46,
                a = 255,
            },
            new Color32()
            {
                r = 46,
                g = 158,
                b = 248,
                a = 255,
            },
        };
        /// <summary>ボス登場演出のプロパティ</summary>
        [SerializeField] private CircleBossDirector circleBossDirector = new CircleBossDirector()
        {
            durations = new float[]
            {
                2f,
                .25f,
            },
            dangerousColor = new Color32()
            {
                r = 255,
                g = 0,
                b = 0,
                a = 255,
            },
        };

        private void Reset()
        {
            image = GetComponent<Image>();
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillOrigin = 2;
            image.fillClockwise = false;
        }

        private void Start()
        {
            var utility = new MainCommonUtility();
            maskAngle = utility.AdminDataSingleton.AdminBean.clearCountdownTimerCircleView.maskAngle;
        }

        public bool SetAngle(float timeSec, float limitTimeSecMax)
        {
            return _utility.SetFillAmountOfImage(image, timeSec, limitTimeSecMax, maskAngle, Transform);
        }

        public bool SetColor(float onmyoStateValue)
        {
            return _utility.SetColorOfImage(onmyoStateValue, image, colors);
        }

        public IEnumerator PlayRepairAngleAnimation(System.IObserver<bool> observer, int isTimeOutState)
        {
            switch ((IsTimeOutState)isTimeOutState)
            {
                case IsTimeOutState.Infinite:
                    Observable.FromCoroutine<bool>(observer => _utility.PlayFillAmountAndColorOfImage(observer, circleBossDirector.durations, circleBossDirector.dangerousColor, image, 1f, 1f, maskAngle, Transform))
                        .Subscribe(_ => observer.OnNext(true),
                        onError: exception =>
                        {
                            Debug.LogError("PlayFillAmountOfImage");
                            observer.OnError(exception);
                        })
                        .AddTo(gameObject);
                    MainGameManager.Instance.AudioOwner.PlayBGM(Audio.ClipToPlayBGM.bgm_stage_vol15);

                    break;
                default:
                    // それ以外
                    observer.OnNext(true);

                    break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// カウントダウンタイマーの情報に合わせてUIを変化させる
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IClearCountdownTimerCircleView
    {
        /// <summary>
        /// 角度をセットする
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>成功／失敗</returns>
        public bool SetAngle(float timeSec, float limitTimeSecMax);
        /// <summary>
        /// 色をセットする
        /// </summary>
        /// <param name="onmyoStateValue">陰陽（昼夜）の状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetColor(float onmyoStateValue);
    }

    /// <summary>
    /// ボス登場演出用
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IClearCountdownTimerCircleBossDirectionView
    {
        /// <summary>
        /// 回復する様に角度を描画するアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="isTimeOutState">タイムアウト状態</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayRepairAngleAnimation(System.IObserver<bool> observer, int isTimeOutState);
    }

    /// <summary>
    /// ボス登場演出のプロパティ
    /// </summary>
    [System.Serializable]
    public struct CircleBossDirector
    {
        /// <summary>アニメーション終了時間</summary>
        public float[] durations;
        /// <summary>危険カラー</summary>
        public Color32 dangerousColor;
    }
}
