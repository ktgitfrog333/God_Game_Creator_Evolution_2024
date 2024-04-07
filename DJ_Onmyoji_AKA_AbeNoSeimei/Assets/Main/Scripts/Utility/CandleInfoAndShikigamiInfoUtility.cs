using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Main.Common;
using Main.Model;
using UniRx;

namespace Main.Utility
{
    /// <summary>
    /// 蝋燭リソースと式神情報
    /// </summary>
    public class CandleInfoAndShikigamiInfoUtility : ICandleInfoAndShikigamiInfoUtility
    {
        public IEnumerator ResetContentsAndRevert<T>(System.IObserver<bool> observer, T[] contents, float[] durations, float resetValue, ShikigamiSkillSystemModel model, ShikigamiInfo[] prevContents)
        {
            Dictionary<RapidRecoveryType, IntReactiveProperty> allFixedCnts = new Dictionary<RapidRecoveryType, IntReactiveProperty>()
            {
                { RapidRecoveryType.Done, new IntReactiveProperty()},
                { RapidRecoveryType.Doing, new IntReactiveProperty()},
                { RapidRecoveryType.Reserve, new IntReactiveProperty()},
            };
            if (contents is ShikigamiInfo[] shikigamiInfos)
            {
                allFixedCnts[RapidRecoveryType.Doing].ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (shikigamiInfos.Length <= x)
                        {
                            // レベルリバートを終了
                            foreach (var item in shikigamiInfos)
                                // レベルリバートはここで完了となるNoneとする
                                item.state.tempoLevelRevertState.Value = (int)RapidRecoveryType.None;
                            observer.OnNext(true);
                        }
                    });
                allFixedCnts[RapidRecoveryType.Reserve].ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (shikigamiInfos.Length <= x)
                        {
                            // レベルリバート実行状態へ移行
                            foreach (var item in shikigamiInfos.Select((p, i) => new { Content = p, Index = i}))
                            {
                                item.Content.state.tempoLevelRevertState.Value = (int)RapidRecoveryType.Doing;
                                if (prevContents is ShikigamiInfo[] prevShikigamiInfos)
                                    // レベルリバート実行中
                                    DOTween.To(() => item.Content.state.tempoLevel.Value,
                                        x => item.Content.state.tempoLevel.Value = x,
                                        prevShikigamiInfos[item.Index].state.tempoLevel.Value,
                                        durations[1])
                                        .OnComplete(() => allFixedCnts[RapidRecoveryType.Doing].Value++);
                            }
                        }
                    });
                // レベルリバート準備に入る
                foreach (var item in shikigamiInfos)
                {
                    item.state.tempoLevelRevertState.Value = (int)RapidRecoveryType.Reserve;
                    DOTween.To(() => item.state.tempoLevel.Value,
                        x => item.state.tempoLevel.Value = x,
                        resetValue,
                        durations[0])
                        .SetEase(Ease.OutCirc)
                        .OnComplete(() => allFixedCnts[RapidRecoveryType.Reserve].Value++);
                }
            }
            else if (contents is CandleInfo[] candleInfos)
            {
                allFixedCnts[RapidRecoveryType.Done].ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        foreach (var item in candleInfos)
                            // 急速回復はここで完全に終了
                            item.rapidRecoveryState.Value = (int)RapidRecoveryType.None;
                        observer.OnNext(true);
                    });
                allFixedCnts[RapidRecoveryType.Doing].ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (candleInfos.Length <= x)
                        {
                            // 急速回復を終了（最大値になるまで）
                            foreach (var item in candleInfos)
                            {
                                item.rapidRecoveryState.Value = (int)RapidRecoveryType.Done;
                                // 効果時間に応じて持続させる
                                DOVirtual.DelayedCall(item.rapidRecoveryTimeSec, () => allFixedCnts[RapidRecoveryType.Done].Value++);
                            }
                        }
                    });
                allFixedCnts[RapidRecoveryType.Reserve].ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (candleInfos.Length <= x)
                        {
                            // 急速回復実行状態へ移行
                            foreach (var item in candleInfos)
                            {
                                item.rapidRecoveryState.Value = (int)RapidRecoveryType.Doing;
                                // 急速回復を実行中
                                item.IsOutCost.Value = false;
                                DOTween.To(() => item.CandleResource.Value,
                                    x => item.CandleResource.Value = x,
                                    item.LimitCandleResorceMax,
                                    durations[1])
                                    .OnComplete(() => allFixedCnts[RapidRecoveryType.Doing].Value++);
                            }
                        }
                    });
                // 急速回復準備に入る
                foreach (var item in candleInfos)
                {
                    item.rapidRecoveryState.Value = (int)RapidRecoveryType.Reserve;
                    DOTween.To(() => item.CandleResource.Value,
                        x => item.CandleResource.Value = x,
                        0f,
                        durations[0])
                        .SetEase(Ease.OutCirc)
                        .OnComplete(() =>
                        {
                            allFixedCnts[RapidRecoveryType.Reserve].Value++;
                            item.IsOutCost.Value = true;
                        });
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// 蝋燭リソースと式神情報
    /// インターフェース
    /// </summary>
    public interface ICandleInfoAndShikigamiInfoUtility
    {
        /// <summary>
        /// コンテンツをリセットした後に元に戻す
        /// </summary>
        /// <typeparam name="T">コンテンツタイプ</typeparam>
        /// <param name="observer">バインド</param>
        /// <param name="contents">コンテンツ</param>
        /// <param name="durations">終了時間</param>
        /// <param name="resetValue">リセット値</param>
        /// <param name="model">式神スキル管理システムモデル</param>
        /// <param name="prevContents">変更前コンテンツ</param>
        /// <returns>コルーチン</returns>
        public IEnumerator ResetContentsAndRevert<T>(System.IObserver<bool> observer, T[] contents, float[] durations, float resetValue, ShikigamiSkillSystemModel model, ShikigamiInfo[] prevContents);
    }
}
