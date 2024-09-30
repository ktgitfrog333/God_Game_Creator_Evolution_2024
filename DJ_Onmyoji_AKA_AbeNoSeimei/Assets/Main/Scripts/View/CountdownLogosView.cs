using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// カウントダウンロゴ（親）
    /// ビュー
    /// </summary>
    public class CountdownLogosView : MonoBehaviour, ICountdownLogosView
    {
        /// <summary>カウントダウンロゴの構造体</summary>
        [SerializeField] private CountdownLogosConf[] countdownLogosConfs;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>スプライト表示用</summary>
        private SpriteRenderer _countdownSpriteRenderer;

        private void Reset()
        {
            List<CountdownLogosConf> countdownLogosConfs = new List<CountdownLogosConf>();
            int index = 1;
            foreach (Transform child in transform)
            {
                countdownLogosConfs.Add(new CountdownLogosConf()
                {
                    number = index++,
                    fadeImage = child.GetComponent<FadeImageView>(),
                    countdownLogoView = child.GetComponent<CountdownLogoView>(),
                });
            }
            this.countdownLogosConfs = countdownLogosConfs.ToArray();
        }

        private void Start()
        {
            foreach (var tmpCountdownLogosConf in countdownLogosConfs.Select((p, i) => new { Content = p, Index = i }))
            {
                tmpCountdownLogosConf.Content.fadeImage.SetFade(Common.EnumFadeState.Open);
                countdownLogosConfs[tmpCountdownLogosConf.Index].isOverLimit = new BoolReactiveProperty();
            }
        }

        public IEnumerator PlayCountDownDirection(System.IObserver<bool> observer, float timeSec)
        {
            var tmpCountdownLogosConfs = countdownLogosConfs.Where(q => timeSec < q.number &&
                q.isOverLimit != null &&
                !q.isOverLimit.Value)
                .OrderByDescending(q => q.number)
                .ToArray();
            if (tmpCountdownLogosConfs.Length < 1)
            {
                observer.OnNext(true);
                yield return null;
            }
            else
            {
                countdownLogosConfs.Where(q => q.number == tmpCountdownLogosConfs[0].number)
                    .ToArray()[0].isOverLimit.Value = true;
                IntReactiveProperty compleateCount = new IntReactiveProperty();
                compleateCount.ObserveEveryValueChanged(x => x.Value)
                    .Where(x => 1 < x)
                    .Subscribe(_ => observer.OnNext(true));
                var tmpCountdownLogosStructs = countdownLogosConfs.Where(q => q.number == tmpCountdownLogosConfs[0].number)
                    .ToArray();
                if (tmpCountdownLogosStructs.Length < 1 ||
                    2 < tmpCountdownLogosStructs.Length)
                    throw new System.ArgumentOutOfRangeException($"重複設定ありまたはデータ無し: number[{string.Join(",", tmpCountdownLogosStructs.Select(q => q.number))}]");

                foreach (var countdownLogosStruct in tmpCountdownLogosStructs)
                {
                    Observable.FromCoroutine<bool>(observer => countdownLogosStruct.fadeImage.PlayFadeLoopsYoyoAnimation(observer, Common.EnumFadeState.Close))
                        .Where(x => x)
                        .Subscribe(_ => compleateCount.Value++)
                        .AddTo(gameObject);
                    Observable.FromCoroutine<bool>(observer => countdownLogosStruct.countdownLogoView.PlayCountDownDirection(observer))
                        .Where(x => x)
                        .Subscribe(_ => compleateCount.Value++)
                        .AddTo(gameObject);
                }

                yield return null;
            }
        }

    }

    /// <summary>
    /// カウントダウンロゴ（親）の設定
    /// </summary>
    [System.Serializable]
    public struct CountdownLogosConf
    {
        /// <summary>番号</summary>
        public int number;
        /// <summary>フェードイメージビュー</summary>
        public FadeImageView fadeImage;
        /// <summary>カウントダウンロゴビュー</summary>
        public CountdownLogoView countdownLogoView;
        /// <summary>指定時間を過ぎているか</summary>
        public BoolReactiveProperty isOverLimit;
    }

    /// <summary>
    /// カウントダウンロゴ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ICountdownLogosView
    {
        /// <summary>
        /// カウントダウン演出を再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="timeSec">タイマー</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayCountDownDirection(System.IObserver<bool> observer, float timeSec);
    }
}
