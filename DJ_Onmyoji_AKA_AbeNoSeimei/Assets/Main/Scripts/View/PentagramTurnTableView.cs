using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Main.Audio;
using Main.Common;

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

        [SerializeField, Range(.5f, 10f)] private float angleCorrectionValue = 5f;
        [SerializeField] private float[] durations = { .01f };
        public float[] Durations
        {
            get { return durations; }
            set { durations = value; }
        }
        public float AngleCorrectionValue
        {
            get { return angleCorrectionValue; }
            set { angleCorrectionValue = value; }
        }
        private bool _isPlaying;
        public bool MoveSpin(BgmConfDetails bgmConfDetails)
        {
            try
            {
                if (!ControllAudio(bgmConfDetails))
                    throw new System.Exception("ControllAudio");
                // if (!_isPlaying)
                // {
                //     _isPlaying = true;
                //     // imageを回転させる
                //     float angle = bgmConfDetails.InputValue * -1f * angleCorrectionValue;
                //     image.transform.DOBlendableRotateBy(new Vector3(0f, 0f, angle), durations[0])
                //         .OnComplete(() => _isPlaying = false);
                // }
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
    }
}
