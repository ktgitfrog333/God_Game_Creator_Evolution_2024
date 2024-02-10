using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.InputSystem;
using Main.Model;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Main.Utility
{
    /// <summary>
    /// InputSystemのユーティリティ
    /// </summary>
    public class InputSystemUtility : IInputSystemUtility
    {
        /// <summary>最小値</summary>
        public const float MIN = -1f;
        /// <summary>最大値</summary>
        public const float MAX = 1f;
        /// <summary>リソース更新のUpdateオブサーバー</summary>
        private System.IDisposable _modelUpdObservable;

        public bool SetInputValueInModel(IReactiveProperty<float> inputValue, float multiDistanceCorrected, Vector2ReactiveProperty previousInput, float autoSpinSpeed, PentagramSystemModel model)
        {
            try
            {
                Observable.FromCoroutine<InputSystemsOwner>(observer => UpdateAsObservableOfInputSystemsOwner(observer, model))
                    .Where(x => x != null)
                    .Subscribe(x =>
                    {
                        Vector2 currentInput = x.InputUI.Scratch; // 現在の入力を取得
                        if (IsPerformed(previousInput.Value.sqrMagnitude, currentInput.sqrMagnitude)) // 前回と今回の入力が十分に大きい場合
                        {
                            float angle = Vector2.SignedAngle(previousInput.Value, currentInput) * -1f; // 前回の入力から今回の入力への角度を計算
                            float distance = Mathf.PI * angle / 180; // 角度を円周の長さに変換
                            inputValue.Value = Mathf.Clamp(distance * multiDistanceCorrected, -1f, 1f);
                        }
                        else
                            inputValue.Value = autoSpinSpeed;
                        previousInput.Value = currentInput; // 現在の入力を保存
                    })
                    .AddTo(model);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 回転の操作中か
        /// </summary>
        /// <param name="prevMagnitude">直前の入力値</param>
        /// <param name="currentMagnitude">現在の入力値</param>
        /// <returns>真／偽</returns>
        private bool IsPerformed(float prevMagnitude, float currentMagnitude)
        {
            return 0f < prevMagnitude &&
                0f < currentMagnitude;
        }

        public bool UpdateJockeyCommandType(IReactiveProperty<float> inputValue, IReactiveProperty<int> jockeyCommandType, float autoSpinSpeed, int inputHistoriesLimit)
        {
            try
            {
                // 入力履歴を保持するリスト
                List<float> inputHistory = new List<float>();

                // inputValueの変更を購読
                inputValue.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(value => 
                    {
                        // 入力履歴が10を超えた場合、最初の要素を削除
                        if (inputHistory.Count >= inputHistoriesLimit)
                            inputHistory.RemoveAt(0);

                        // 最後の入力と異なる場合のみ履歴に追加
                        if (inputHistory.Count == 0 ||
                            inputHistory[inputHistory.Count - 1] != value)
                            inputHistory.Add(value);

                        // 最後の入力が0の場合、jockeyCommandTypeをHoldに設定
                        if (value == 0 ||
                            isScratch(inputHistory, autoSpinSpeed))
                        {
                            if (value == 0)
                                jockeyCommandType.Value = (int)JockeyCommandType.Hold;
                            if (isScratch(inputHistory, autoSpinSpeed))
                            {
                                jockeyCommandType.Value = (int)JockeyCommandType.Scratch;
                                if (0 < inputHistory.Count)
                                    inputHistory.Clear();
                            }
                        }
                        else if (value == autoSpinSpeed)
                        {
                            jockeyCommandType.Value = (int)JockeyCommandType.None;
                            if (0 < inputHistory.Count)
                                inputHistory.Clear();
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

        private bool isScratch(List<float> inputHistory, float autoSpinSpeed)
        {
            if (inputHistory.Count < 3)
                return false;

            var ignoreZeroGroup = inputHistory.Where(q => (0f < q || q < 0f) &&
                q != autoSpinSpeed)
                .Select((p, i) => new { Content = 0f < p ? 1f : (p < 0f ? -1f : 0f), Index = i })
                .OrderBy(q => q.Content).ThenByDescending(q => q.Index)
                .GroupBy(x => x.Content);
            var ignoreZero = ignoreZeroGroup.Select(g => g.First())
                .Union(ignoreZeroGroup.Select(g => g.Last()))
                .Select(q => q.Content)
                .ToArray();

            if (2 < ignoreZero.Length)
                return (ignoreZero[ignoreZero.Length - 3] > 0 && ignoreZero[ignoreZero.Length - 2] < 0 && ignoreZero[ignoreZero.Length - 1] > 0) ||
                    (ignoreZero[ignoreZero.Length - 3] < 0 && ignoreZero[ignoreZero.Length - 2] > 0 && ignoreZero[ignoreZero.Length - 1] < 0);
            else
                return false;
        }

        public bool SetOnmyoStateInModel(IReactiveProperty<float> onmyoState, float[] durations, SunMoonSystemModel model)
        {
            try
            {
                IntReactiveProperty priority = new IntReactiveProperty((int)OnmyoStatePriority.None);
                var modelUpdObservable = model.UpdateAsObservable().Subscribe(_ => {});
                priority.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        modelUpdObservable.Dispose();
                        switch ((OnmyoStatePriority)x)
                        {
                            case OnmyoStatePriority.ChargeSun:
                                modelUpdObservable = model.UpdateAsObservable()
                                    .Subscribe(_ =>
                                    {
                                        if (!UpdateLevelUp(onmyoState, durations[0]))
                                            Debug.LogError("UpdateLevelUp");
                                    });

                                break;
                            case OnmyoStatePriority.ChargeMoon:
                                modelUpdObservable = model.UpdateAsObservable()
                                    .Subscribe(_ =>
                                    {
                                        if (!UpdateLevelDown(onmyoState, durations[0]))
                                            Debug.LogError("UpdateLevelDown");
                                    });

                                break;
                            case OnmyoStatePriority.CompleteSun:
                                onmyoState.Value = MAX;

                                break;
                            case OnmyoStatePriority.CompleteMoon:
                                onmyoState.Value = MIN;

                                break;
                            case OnmyoStatePriority.None:
                                break;
                            default:
                                throw new System.Exception("例外エラー");
                        }
                    });
                Observable.FromCoroutine<InputSystemsOwner>(observer => UpdateAsObservableOfInputSystemsOwner(observer, model))
                    .Where(x => x != null)
                    .Subscribe(x =>
                    {
                        switch (x.InputHistroy.InputTypeID)
                        {
                            case InputTypeID.IT0001:
                                priority.Value = (int)OnmyoStatePriority.CompleteSun;

                                break;
                            case InputTypeID.IT0002:
                                priority.Value = (int)OnmyoStatePriority.CompleteMoon;

                                break;
                            default:
                                var chargeSun = x.InputUI.ChargeSun;
                                var chargeMoon = x.InputUI.ChargeMoon;
                                if (chargeSun ||
                                chargeMoon)
                                {
                                    if (chargeSun &&
                                    !priority.Value.Equals((int)OnmyoStatePriority.CompleteSun))
                                        priority.Value = (int)OnmyoStatePriority.ChargeSun;
                                    if (chargeMoon &&
                                    !priority.Value.Equals((int)OnmyoStatePriority.CompleteMoon))
                                        priority.Value = (int)OnmyoStatePriority.ChargeMoon;
                                }
                                else
                                    priority.Value = (int)OnmyoStatePriority.None;

                                break;
                        }
                    })
                    .AddTo(model);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetCandleResourceAndTempoLevelsInModel(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, float updateCorrected, ShikigamiSkillSystemModel model)
        {
            try
            {
                if (!SetCandleResource(candleInfo, shikigamiInfos, model))
                    throw new System.Exception("SetCandleResource");
                if (!SetTempoLevels(shikigamiInfos, updateCorrected, candleInfo.IsOutCost, model))
                    throw new System.Exception("SetTempoLevels");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// リソースを変更
        /// </summary>
        /// <param name="candleInfo">蠟燭の情報</param>
        /// <param name="shikigamiInfos">式神の情報</param>
        /// <param name="model">式神スキル管理システムモデル</param>
        /// <returns>成功／失敗</returns>
        private bool SetCandleResource(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, ShikigamiSkillSystemModel model)
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
                var utility = new ShikigamiParameterUtility();
                model.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        float costSum = 0f;
                        foreach (var item in shikigamiInfos)
                        {
                            var tempoLv = item.state.tempoLevel.Value;
                            var shikigamiLv = item.prop.level;
                            float actionRate = utility.GetMainSkillValue(item, MainSkillType.ActionRate);
                            costSum += tempoLv * shikigamiLv * actionRate;
                        }
                        if (!UpdateCandleResource(candleInfo, costSum * Time.deltaTime))
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

        public bool UpdateCandleResourceByPentagram(JockeyCommandType jkeyCmdTypeCurrent, JockeyCommandType jkeyCmdTypePrevious, CandleInfo candleInfo, float downValue, ShikigamiSkillSystemModel model)
        {
            try
            {
                if (_modelUpdObservable == null)
                    _modelUpdObservable = model.UpdateAsObservable().Subscribe(_ => {});
                switch (jkeyCmdTypeCurrent)
                {
                    case JockeyCommandType.None:
                        _modelUpdObservable.Dispose();

                        break;
                    case JockeyCommandType.Hold:
                        if (jkeyCmdTypePrevious.Equals(JockeyCommandType.Scratch))
                            // Hold⇔Scratchの場合は何もしない
                            return true;
                        else
                        {
                            if (!CallUpdateCandleResource(ref _modelUpdObservable, candleInfo, downValue, model))
                                throw new System.Exception("CallUpdateCandleResource");
                        }

                        break;
                    case JockeyCommandType.Scratch:
                        if (jkeyCmdTypePrevious.Equals(JockeyCommandType.Hold))
                            // Hold⇔Scratchの場合は何もしない
                            return true;
                        else
                        {
                            if (!CallUpdateCandleResource(ref _modelUpdObservable, candleInfo, downValue, model))
                                throw new System.Exception("CallUpdateCandleResource");
                        }

                        break;
                    case JockeyCommandType.BackSpin:
                        _modelUpdObservable.Dispose();
                        // 何もしない
                        break;
                    case JockeyCommandType.SlipLoop:
                        _modelUpdObservable.Dispose();
                        // 何もしない
                        break;
                    default:
                        throw new System.Exception("未定義のジョッキーコマンドタイプ");
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
        /// リソースを更新メソッドを呼び出す
        /// </summary>
        /// <param name="modelUpdObservable">リソース更新のUpdateオブサーバー</param>
        /// <param name="candleInfo">蝋燭の情報</param>
        /// <param name="downValue">更新の補正値</param>
        /// <param name="model">式神スキル管理システムモデル</param>
        /// <returns>成功／失敗</returns>
        private bool CallUpdateCandleResource(ref System.IDisposable modelUpdObservable, CandleInfo candleInfo, float downValue, ShikigamiSkillSystemModel model)
        {
            try
            {
                modelUpdObservable.Dispose();
                modelUpdObservable = model.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if (!UpdateCandleResource(candleInfo, downValue * Time.deltaTime))
                            throw new System.Exception("UpdateCandleResource");
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
        /// リソースを更新
        /// </summary>
        /// <param name="candleInfo">蠟燭の情報</param>
        /// <param name="costSum">コスト</param>
        /// <returns>成功／失敗</returns>
        private bool UpdateCandleResource(CandleInfo candleInfo, float costSum)
        {
            try
            {
                var calcResult = candleInfo.CandleResource.Value - costSum;
                candleInfo.CandleResource.Value = System.Math.Clamp(calcResult, 0f, candleInfo.LimitCandleResorceMax);
                candleInfo.IsOutCost.Value = calcResult <= 0f;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// レベルを変更
        /// </summary>
        /// <param name="shikigamiInfos">式神の情報</param>
        /// <param name="updateCorrected">更新の補正値</param>
        /// <param name="isOutCost">リソース切れか</param>
        /// <param name="model">式神スキル管理システムモデル</param>
        /// <returns>成功／失敗</returns>
        private bool SetTempoLevels(ShikigamiInfo[] shikigamiInfos, float updateCorrected, IReactiveProperty<bool> isOutCost, ShikigamiSkillSystemModel model)
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
                        const float REDEFAULT = -1f;
                        if (!x)
                        {
                            // 残リソースが有り
                            const int L = 0, R = 1;
                            IntReactiveProperty[] priority = new IntReactiveProperty[]
                            {
                                new IntReactiveProperty((int)TempLevelPriority.L.None),
                                new IntReactiveProperty((int)TempLevelPriority.R.None),
                            };
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
                                                            .Subscribe(_ => ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Wrap, UpdateLevelUp));

                                                        break;
                                                    case TempLevelPriority.L.ReleaseLFader:
                                                        modelUpdObservable[item.Index] = model.UpdateAsObservable()
                                                            .Subscribe(_ => ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Wrap, UpdateLevelDown));

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
                                                            .Subscribe(_ => ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Graffiti, UpdateLevelUp));

                                                        break;
                                                    case TempLevelPriority.R.ReleaseRFader:
                                                        modelUpdObservable[item.Index] = model.UpdateAsObservable()
                                                            .Subscribe(_ => ProcessShikigamiInfos(shikigamiInfos, updateCorrected, ShikigamiType.Graffiti, UpdateLevelDown));

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
                            Observable.FromCoroutine<InputSystemsOwner>(observer => UpdateAsObservableOfInputSystemsOwner(observer, model))
                                .Where(x => x != null)
                                .Subscribe(x =>
                                {
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
                                item.state.tempoLevel.Value = REDEFAULT;
                        }
                    });
                model.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        // ダンスはラップとグラフィティの中間
                        foreach (var item in shikigamiInfos.Where(q => q.prop.type.Equals(ShikigamiType.Dance)))
                            item.state.tempoLevel.Value = (CalcShikigamiType(shikigamiInfos, ShikigamiType.Wrap)
                                + CalcShikigamiType(shikigamiInfos, ShikigamiType.Graffiti))
                                * .5f;
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

        /// <summary>
        /// モデルコンポーネントを監視する
        /// InputSystemsOwnerの情報を発行
        /// </summary>
        /// <typeparam name="T">コンポーネント型</typeparam>
        /// <param name="observer">バインド</param>
        /// <param name="model">モデル</param>
        /// <returns>コルーチン</returns>
        private IEnumerator UpdateAsObservableOfInputSystemsOwner<T>(System.IObserver<InputSystemsOwner> observer, T model) where T : MonoBehaviour
        {
            InputSystemsOwner inputSystemsOwner = null;
            model.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (inputSystemsOwner == null)
                        inputSystemsOwner = MainGameManager.Instance != null ? MainGameManager.Instance.InputSystemsOwner : null;
                    observer.OnNext(inputSystemsOwner);
                });

            yield return null;
        }

        /// <summary>
        /// 対象のパラメータをレベルアップ／ダウンして更新するデリゲート
        /// </summary>
        /// <param name="level">レベル</param>
        /// <param name="updateValue">更新値</param>
        /// <returns>成功／失敗</returns>
        private delegate bool UpdateLevelDelegate(IReactiveProperty<float> level, float updateValue);

        /// <summary>
        /// デリゲートを介して各更新メソッドを呼び出す
        /// </summary>
        /// <param name="shikigamiInfos">式神の情報</param>
        /// <param name="updateCorrected">更新の補正値</param>
        /// <param name="targetType">対象式神タイプ</param>
        /// <param name="updateLevelMethod">レベル更新メソッド</param>
        /// <exception cref="System.Exception">エラー発生メソッド名</exception>
        private void ProcessShikigamiInfos(ShikigamiInfo[] shikigamiInfos, float updateCorrected, ShikigamiType targetType, UpdateLevelDelegate updateLevelMethod)
        {
            foreach (var item in shikigamiInfos.Where(q => q.prop.type.Equals(targetType)))
                if (!updateLevelMethod(item.state.tempoLevel, updateCorrected))
                    throw new System.Exception(updateLevelMethod.Method.Name);
        }

        /// <summary>
        /// 対象のパラメータをレベルアップして更新
        /// </summary>
        /// <param name="targetLevel">対象レベル</param>
        /// <param name="corrected">加算減算の補正値</param>
        /// <returns>成功／失敗</returns>
        private bool UpdateLevelUp(IReactiveProperty<float> targetLevel, float corrected)
        {
            try
            {
                targetLevel.Value = System.Math.Min(targetLevel.Value + Time.deltaTime / corrected, MAX);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 対象のパラメータをレベルダウンして更新
        /// </summary>
        /// <param name="targetLevel">対象レベル</param>
        /// <param name="corrected">加算減算の補正値</param>
        /// <returns>成功／失敗</returns>
        private bool UpdateLevelDown(IReactiveProperty<float> targetLevel, float corrected)
        {
            try
            {
                targetLevel.Value = System.Math.Max(targetLevel.Value - Time.deltaTime / corrected, MIN);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool ResetCandleResourceAndBuffAllTempoLevelsByPentagram(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, ShikigamiSkillSystemModel model)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 昼／夜の状態変更の優先度
        /// </summary>
        private enum OnmyoStatePriority
        {
            /// <summary>入力無し</summary>
            None = -1,
            /// <summary>昼チャージ</summary>
            ChargeSun,
            /// <summary>夜チャージ</summary>
            ChargeMoon,
            /// <summary>昼完了</summary>
            CompleteSun,
            /// <summary>夜完了</summary>
            CompleteMoon,
        }

        /// <summary>
        /// テンポレベルの優先度
        /// </summary>
        private class TempLevelPriority
        {
            /// <summary>
            /// 左側フェーダー
            /// </summary>
            public enum L
            {
                /// <summary>入力無し</summary>
                None = -1,
                /// <summary>フェーダー（左）チャージ</summary>
                ChargeLFader,
                /// <summary>フェーダー（左）解放</summary>
                ReleaseLFader,
            }

            /// <summary>
            /// 右側フェーダー
            /// </summary>
            public enum R
            {
                /// <summary>入力無し</summary>
                None = -1,
                /// <summary>フェーダー（右）チャージ</summary>
                ChargeRFader,
                /// <summary>フェーダー（右）解放</summary>
                ReleaseRFader,
            }
        }
    }

    /// <summary>
    /// InputSystemのユーティリティ
    /// インタフェース
    /// </summary>
    public interface IInputSystemUtility
    {
        /// <summary>
        /// モデルコンポーネントを監視して第1引数へセットされた値を更新
        /// 現在の入力と過去の入力を元に回転量を計算
        /// </summary>
        /// <param name="inputValue">入力角度</param>
        /// <param name="multiDistanceCorrected">距離の補正乗算値</param>
        /// <param name="previousInput">過去の入力</param>
        /// <param name="autoSpinSpeed">自動回転の速度</param>
        /// <param name="model">ペンダグラムシステムモデル</param>
        /// <returns>成功／失敗</returns>
        public bool SetInputValueInModel(IReactiveProperty<float> inputValue, float multiDistanceCorrected, Vector2ReactiveProperty previousInput, float autoSpinSpeed, PentagramSystemModel model);
        /// <summary>
        /// ジョッキーコマンドタイプを更新
        /// </summary>
        /// <param name="inputValue">入力角度</param>
        /// <param name="jockeyCommandType">ジョッキーコマンドタイプ</param>
        /// <param name="autoSpinSpeed">自動回転の速度</param>
        /// <param name="inputHistoriesLimit">コマンドの入力数</param>
        /// <returns>成功／失敗</returns>
        public bool UpdateJockeyCommandType(IReactiveProperty<float> inputValue, IReactiveProperty<int> jockeyCommandType, float autoSpinSpeed, int inputHistoriesLimit);
        /// <summary>
        /// モデルコンポーネントを監視して第1引数へセットされた値を更新
        /// 入力された内容に基づいて昼／夜の状態を変更
        /// </summary>
        /// <param name="onmyoState">陰陽（昼夜）の状態</param>
        /// <param name="durations">ボタン押下の時間管理</param>
        /// <param name="model">陰陽（昼夜）の切り替えモデル</param>
        /// <returns>成功／失敗</returns>
        public bool SetOnmyoStateInModel(IReactiveProperty<float> onmyoState, float[] durations, SunMoonSystemModel model);
        /// <summary>
        /// モデルコンポーネントを監視して第1引数へセットされた値を更新
        /// 入力された内容に基づいてリソースとレベルを変更
        /// </summary>
        /// <param name="candleInfo">蠟燭の情報</param>
        /// <param name="shikigamiInfos">式神の情報</param>
        /// <param name="updateCorrected">更新の補正値</param>
        /// <param name="model">式神スキル管理システムモデル</param>
        /// <returns>成功／失敗</returns>
        public bool SetCandleResourceAndTempoLevelsInModel(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, float updateCorrected, ShikigamiSkillSystemModel model);
        /// <summary>
        /// リソースを更新
        /// 引数の+-は考慮せずリソースは消費される
        /// </summary>
        /// <param name="jkeyCmdTypeCurrent">ジョッキーコマンドタイプ</param>
        /// <param name="jkeyCmdTypePrevious">1つ前のジョッキーコマンドタイプ</param>
        /// <param name="candleInfo">蠟燭の情報</param>
        /// <param name="downValue">更新の補正値</param>
        /// <param name="model">式神スキル管理システムモデル</param>
        /// <returns>成功／失敗</returns>
        public bool UpdateCandleResourceByPentagram(JockeyCommandType jkeyCmdTypeCurrent, JockeyCommandType jkeyCmdTypePrevious, CandleInfo candleInfo, float downValue, ShikigamiSkillSystemModel model);
        /// <summary>
        /// リソースをリセットさせる
        /// また、一時的にテンポスライダーへ自動回復効果を付与
        /// </summary>
        /// <param name="candleInfo">蠟燭の情報</param>
        /// <param name="shikigamiInfos">式神の情報</param>
        /// <param name="model">式神スキル管理システムモデル</param>
        /// <returns>成功／失敗</returns>
        public bool ResetCandleResourceAndBuffAllTempoLevelsByPentagram(CandleInfo candleInfo, ShikigamiInfo[] shikigamiInfos, ShikigamiSkillSystemModel model);
    }
}
