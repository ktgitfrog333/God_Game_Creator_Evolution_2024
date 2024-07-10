using Main.Common;
using Main.Model;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Linq;
using Main.InputSystem;

namespace Main.Utility
{
    /// <summary>
    /// InputSystemのユーティリティ
    /// </summary>
    public partial class InputSystemUtility
    {
        /// <summary>
        /// InputSystemのサブユーティリティ
        /// 蝋燭ゲージとテンポレベル
        /// </summary>
        public class CandleResourceAndTempoLevelsSubUtility : ICandleResourceAndTempoLevelsSubUtility
        {
            /// <summary>InputSystemのユーティリティ</summary>
            private readonly InputSystemUtility _inputSystemUtility;
            /// <summary>式神タイプ別パラメータ管理</summary>
            private ShikigamiParameterUtility _shikigamiParameterUtility = new ShikigamiParameterUtility();
            /// <summary>L</summary>
            private const int L = 0;
            /// <summary>R</summary>
            private const int R = 1;

            public CandleResourceAndTempoLevelsSubUtility(InputSystemUtility inputSystemUtility)
            {
                _inputSystemUtility = inputSystemUtility;
            }

            public bool SetCandleResource(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, ShikigamiSkillSystemModel model)
            {
                try
                {
                    // 1.コストの計算:
                    //  ●テンポスライダーレベル
                    //  ●式神レベル
                    //  ●攻撃間隔
                    //  ●計算式（テンポスライダーレベル*式神レベル*攻撃間隔）
                    // 2.計算結果:
                    //  ●0以下の場合は下記の処理を実行
                    //      ○後続の処理内でテンポスライダーのレベルを0にする
                    // 3.蝋燭の残リソースを更新
                    model.UpdateAsObservable()
                        .Where(_ => candleInfo.rapidRecoveryState.Value == (int)RapidRecoveryType.None ||
                        candleInfo.rapidRecoveryState.Value == (int)RapidRecoveryType.Done)
                        .Subscribe(_ =>
                        {
                            float costSum = 0f;
                            if (candleInfo.isRest.Value)
                            {
                               if (candleInfo.CandleResource.Value >= 10.0f)
                                {
                                    foreach (var item in shikigamiInfos)
                                    {
                                        item.state.isRest.Value = false;
                                    }
                                    candleInfo.isRest.Value = false;
                                }
                                costSum = -2.5f;
                            }
                            else
                            {
                                costSum = _inputSystemUtility.GetCalcCostSum(costSum, shikigamiInfos, candleInfo);
                            }
                            if (!_inputSystemUtility.UpdateCandleResource(candleInfo, costSum * Time.deltaTime))
                                Debug.LogError("UpdateCandleResource");
                        });

                    return true;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }

            public bool SetTempoLevels(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, float updateCorrected, IReactiveProperty<bool> isOutCost, ShikigamiSkillSystemModel model)
            {
                try
                {
                    System.IDisposable[] modelUpdObservable = new System.IDisposable[]
                    {
                        model.UpdateAsObservable().Subscribe(_ => {}),
                        model.UpdateAsObservable().Subscribe(_ => {}),
                    };
                    isOutCost.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            if (!x)
                            {
                                IntReactiveProperty[] priority = new IntReactiveProperty[]
                                {
                                    new IntReactiveProperty((int)TempLevelPriority.L.None),
                                    new IntReactiveProperty((int)TempLevelPriority.R.None),
                                };
                                if (!DoProcessShikigamiInfos(shikigamiInfos, updateCorrected, model, modelUpdObservable, priority))
                                    Debug.LogError("DoProcessShikigamiInfos");
                                Observable.FromCoroutine<InputSystemsOwner>(observer => _inputSystemUtility.UpdateAsObservableOfInputSystemsOwner(observer, model))
                                    .Where(x => x != null &&
                                        x.CurrentInputMode != null)
                                    .Subscribe(x =>
                                    {
                                        switch ((InputMode)x.CurrentInputMode.Value)
                                        {
                                            case InputMode.Gamepad:
                                                if (x.InputUI.ChargeLFader &&
                                                !x.InputUI.ReleaseLFader)
                                                    priority[L].Value = (int)TempLevelPriority.L.ChargeLFader;
                                                else if (x.InputUI.ReleaseLFader)
                                                    priority[L].Value = (int)TempLevelPriority.L.ReleaseLFader;
                                                else
                                                    priority[L].Value = (int)TempLevelPriority.L.None;
                                                if (x.InputUI.ChargeRFader &&
                                                !x.InputUI.ReleaseRFader)
                                                    priority[R].Value = (int)TempLevelPriority.R.ChargeRFader;
                                                else if (x.InputUI.ReleaseRFader)
                                                    priority[R].Value = (int)TempLevelPriority.R.ReleaseRFader;
                                                else
                                                    priority[R].Value = (int)TempLevelPriority.R.None;

                                                break;
                                            case InputMode.MidiJackDDJ200:
                                                if (!DoProcessShikigamiInfos(shikigamiInfos, x))
                                                    Debug.LogError("DoProcessShikigamiInfos");

                                                break;
                                        }

                                    })
                                    .AddTo(model);
                            }
                            else
                            {
                                // SPゲージが尽きた場合にフェーダーのチャージをDisposeできないのでここで行う
                                for (var i = 0; i < modelUpdObservable.Length; i++)
                                    modelUpdObservable[i].Dispose();
                                // 残リソースが無し
                                foreach (var item in shikigamiInfos)
                                {
                                    if(item.prop.type != ShikigamiType.OnmyoTurret)
                                        item.state.tempoLevel.Value = MIN;
                                    item.state.isRest.Value = true;
                                }
                                candleInfo.isRest.Value = true;
                            }
                        });
                    Observable.FromCoroutine<InputSystemsOwner>(observer => _inputSystemUtility.UpdateAsObservableOfInputSystemsOwner(observer, model))
                        // レベルリバートは別ロジックで行い、ここでは可変をロックする
                        .Where(x => shikigamiInfos.Where(q => q.prop.type.Equals(ShikigamiType.Dance) &&
                            x != null &&
                            x.CurrentInputMode != null)
                        .Select(q => q)
                        .ToArray()[0].state.tempoLevelRevertState.Value == (int)RapidRecoveryType.None)
                        .Subscribe(x =>
                        {
                            switch ((InputMode)x.CurrentInputMode.Value)
                            {
                                case InputMode.Gamepad:
                                    // ダンスはラップとグラフィティの中間
                                    foreach (var item in shikigamiInfos.Where(q => q.prop.type.Equals(ShikigamiType.Dance)))
                                        item.state.tempoLevel.Value = (CalcShikigamiType(shikigamiInfos, ShikigamiType.Wrap)
                                            + CalcShikigamiType(shikigamiInfos, ShikigamiType.Graffiti))
                                            * .5f;

                                    break;
                            }
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
            /// デリゲートを介して各更新メソッドを呼び出す
            /// </summary>
            /// <param name="shikigamiInfos">式神の情報</param>
            /// <param name="updateCorrected">更新の補正値</param>
            /// <param name="model">式神スキル管理システムモデル</param>
            /// <param name="modelUpdObservable">モデル更新の監視</param>
            /// <param name="priority">優先度</param>
            /// <returns>成功／失敗</returns>
            private bool DoProcessShikigamiInfos(ShikigamiInfo[] shikigamiInfos, float updateCorrected, ShikigamiSkillSystemModel model, System.IDisposable[] modelUpdObservable, IntReactiveProperty[] priority)
            {
                try
                {
                    // 残リソースが有り
                    foreach (var item in priority.Select((p, i) => new { Content = p, Index = i }))
                    {
                        item.Content.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                modelUpdObservable[item.Index].Dispose();
                                switch (item.Index)
                                {
                                    case L:
                                        // 左
                                        switch ((TempLevelPriority.L)x)
                                        {
                                            case TempLevelPriority.L.ChargeLFader:
                                                modelUpdObservable[item.Index] = model.UpdateAsObservable()
                                                    // レベルリバートは別ロジックで行い、ここでは可変をロックする
                                                    .Where(_ => shikigamiInfos.Where(q => q.prop.type.Equals(ShikigamiType.Wrap))
                                                        .Select(q => q)
                                                        .ToArray()[0].state.tempoLevelRevertState.Value == (int)RapidRecoveryType.None)
                                                    .Subscribe(_ => _inputSystemUtility.ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Wrap, _inputSystemUtility.UpdateLevelUp));

                                                break;
                                            case TempLevelPriority.L.ReleaseLFader:
                                                modelUpdObservable[item.Index] = model.UpdateAsObservable()
                                                    // レベルリバートは別ロジックで行い、ここでは可変をロックする
                                                    .Where(_ => shikigamiInfos.Where(q => q.prop.type.Equals(ShikigamiType.Wrap))
                                                        .Select(q => q)
                                                        .ToArray()[0].state.tempoLevelRevertState.Value == (int)RapidRecoveryType.None)
                                                    .Subscribe(_ => _inputSystemUtility.ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Wrap, _inputSystemUtility.UpdateLevelDown));

                                                break;
                                            case TempLevelPriority.L.None:
                                                break;
                                            default:
                                                throw new System.Exception("例外エラー");
                                        }

                                        break;
                                    case R:
                                        // 右
                                        switch ((TempLevelPriority.R)x)
                                        {
                                            case TempLevelPriority.R.ChargeRFader:
                                                modelUpdObservable[item.Index] = model.UpdateAsObservable()
                                                    // レベルリバートは別ロジックで行い、ここでは可変をロックする
                                                    .Where(_ => shikigamiInfos.Where(q => q.prop.type.Equals(ShikigamiType.Graffiti))
                                                        .Select(q => q)
                                                        .ToArray()[0].state.tempoLevelRevertState.Value == (int)RapidRecoveryType.None)
                                                    .Subscribe(_ => _inputSystemUtility.ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Graffiti, _inputSystemUtility.UpdateLevelUp));

                                                break;
                                            case TempLevelPriority.R.ReleaseRFader:
                                                modelUpdObservable[item.Index] = model.UpdateAsObservable()
                                                    // レベルリバートは別ロジックで行い、ここでは可変をロックする
                                                    .Where(_ => shikigamiInfos.Where(q => q.prop.type.Equals(ShikigamiType.Graffiti))
                                                        .Select(q => q)
                                                        .ToArray()[0].state.tempoLevelRevertState.Value == (int)RapidRecoveryType.None)
                                                    .Subscribe(_ => _inputSystemUtility.ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Graffiti, _inputSystemUtility.UpdateLevelDown));

                                                break;
                                            case TempLevelPriority.R.None:
                                                break;
                                            default:
                                                throw new System.Exception("例外エラー");
                                        }

                                        break;
                                    default:
                                        throw new System.Exception("例外エラー");
                                }
                            });
                    }

                    return true;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }

            /// <summary>
            /// デリゲートを介して各更新メソッドを呼び出す
            /// </summary>
            /// <param name="shikigamiInfos">式神の情報</param>
            /// <param name="inputSystemsOwner">InputSystemのオーナー</param>
            /// <returns>成功／失敗</returns>
            private bool DoProcessShikigamiInfos(ShikigamiInfo[] shikigamiInfos, InputSystemsOwner inputSystemsOwner)
            {
                try
                {
                    foreach (var shikigamiInfo in shikigamiInfos)
                    {
                        switch (shikigamiInfo.prop.type)
                        {
                            case ShikigamiType.Wrap:
                                shikigamiInfo.state.tempoLevel.Value = (inputSystemsOwner.InputMidiJackDDJ200.Mixer1 * 2f) - 1f;

                                break;
                            case ShikigamiType.Dance:
                                shikigamiInfo.state.tempoLevel.Value = (inputSystemsOwner.InputMidiJackDDJ200.Mixer2 * 2f) - 1f;

                                break;
                            case ShikigamiType.Graffiti:
                                shikigamiInfo.state.tempoLevel.Value = (inputSystemsOwner.InputMidiJackDDJ200.Mixer3 * 2f) - 1f;

                                break;
                            default:
                                break;
                        }
                    }

                    return true;
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }

            /// <summary>
            /// 同一の式神タイプのレベル合計値を計算
            /// </summary>
            /// <param name="shikigamiInfos">式神の情報</param>
            /// <param name="shikigamiType">式神タイプ</param>
            /// <returns>計算後の合計値</returns>
            private float CalcShikigamiType(ShikigamiInfo[] shikigamiInfos, ShikigamiType shikigamiType)
            {
                float wrapCalc = 0f;
                foreach (var item in shikigamiInfos.Where(q => q.prop.type.Equals(shikigamiType))
                        .Select(q => q.state.tempoLevel))
                    wrapCalc += item.Value;
                return wrapCalc;
            }
        }

        /// <summary>
        /// InputSystemのサブユーティリティ
        /// 蝋燭ゲージとテンポレベル
        /// インターフェース
        /// </summary>
        public interface ICandleResourceAndTempoLevelsSubUtility
        {
            /// <summary>
            /// リソースを変更
            /// </summary>
            /// <param name="candleInfo">蠟燭の情報</param>
            /// <param name="shikigamiInfos">式神の情報</param>
            /// <param name="model">式神スキル管理システムモデル</param>
            /// <returns>成功／失敗</returns>
            public bool SetCandleResource(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, ShikigamiSkillSystemModel model);
            /// <summary>
            /// レベルを変更
            /// </summary>
            /// <param name="candleInfo">蠟燭の情報</param>
            /// <param name="shikigamiInfos">式神の情報</param>
            /// <param name="updateCorrected">更新の補正値</param>
            /// <param name="isOutCost">リソース切れか</param>
            /// <param name="model">式神スキル管理システムモデル</param>
            /// <returns>成功／失敗</returns>
            public bool SetTempoLevels(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, float updateCorrected, IReactiveProperty<bool> isOutCost, ShikigamiSkillSystemModel model);
        }
    }
}
