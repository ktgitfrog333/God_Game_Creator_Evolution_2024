using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// カウントダウンロゴ
    /// ビュー
    /// </summary>
    public class CountdownLogoView : MonoBehaviour, ICountdownLogoView
    {
        /// <summary>カウントダウンロゴの設定</summary>
        [SerializeField] private CountdownLogoConf countdownLogoConf;
        /// <summary>カウントダウンロゴの設定</summary>
        public CountdownLogoConf CountdownLogoConf => countdownLogoConf;
        /// <summary>Rectトランスフォーム</summary>
        private RectTransform _rectTransform;
        /// <summary>Rectトランスフォーム</summary>
        public RectTransform RectTransform => _rectTransform != null ? _rectTransform : _rectTransform = transform as RectTransform;

        private void Reset()
        {
            CountdownLogoConf countdownLogoConf = new CountdownLogoConf()
            {
                duration = .25f,
                scale = Vector2.one * 1.25f,
            };
            int index = 1;
            foreach (Transform child in transform.parent)
            {
                if (child.Equals(transform))
                {
                    countdownLogoConf.number = index;
                    break;
                }
                index++;
            }
            this.countdownLogoConf = countdownLogoConf;
        }

        public IEnumerator PlayCountDownDirection(System.IObserver<bool> observer)
        {
            RectTransform.DOScale(countdownLogoConf.scale, countdownLogoConf.duration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }
    }

    /// <summary>
    /// カウントダウンロゴの設定
    /// </summary>
    [System.Serializable]
    public struct CountdownLogoConf
    {
        /// <summary>番号</summary>
        public int number;
        /// <summary>スケール</summary>
        public Vector2 scale;
        /// <summary>終了時間</summary>
        public float duration;
    }

    /// <summary>
    /// カウントダウンロゴ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ICountdownLogoView
    {
        /// <summary>
        /// カウントダウン演出を再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayCountDownDirection(System.IObserver<bool> observer);
    }
}
