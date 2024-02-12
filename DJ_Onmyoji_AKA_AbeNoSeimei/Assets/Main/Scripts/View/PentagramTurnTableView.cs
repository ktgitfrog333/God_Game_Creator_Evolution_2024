using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Main.Audio;
using Main.Common;
using UniRx;
using UniRx.Triggers;
using Universal.Common;

namespace Main.View
{
    /// <summary>
    /// ペンダグラムターンテーブル
    /// プレゼンタから伝達された入力を元に出力を行う
    /// Imageコンポーネントへ入力操作を行う
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class PentagramTurnTableView : MonoBehaviour, IPentagramTurnTableView
    {
        /// <summary>ターンテーブル</summary>
        [SerializeField] private Image image;
        /// <summary>オーディオオーナー</summary>
        private AudioOwner _audioOwner;
        /// <summary>オーディオオーナー</summary>
        private AudioOwner AudioOwner
        {
            get
            {
                if (_audioOwner != null)
                    return _audioOwner;
                else
                {
                    _audioOwner = MainGameManager.Instance.AudioOwner;
                    return _audioOwner;
                }
            }
        }

        /// <summary>回転制御においてスティック入力感度の補正値小さいほど鈍く回転して、大きいほど素早く回転する。</summary>
        [SerializeField, Range(.5f, 10f)] private float angleCorrectionValue;
        /// <summary>回転制御においてスティック入力感度の補正値小さいほど鈍く回転して、大きいほど素早く回転する。</summary>
        public float AngleCorrectionValue
        {
            get { return angleCorrectionValue; }
            set { angleCorrectionValue = value; }
        }
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;

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

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        private void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            angleCorrectionValue = adminDataSingleton.AdminBean.pentagramTurnTableView.angleCorrectionValue;
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
    }
}
