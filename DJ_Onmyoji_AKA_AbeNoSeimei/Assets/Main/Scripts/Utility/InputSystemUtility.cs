using System.Collections;
using System.Collections.Generic;
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
                            inputValue.Value = inputValue.Value != autoSpinSpeed ? autoSpinSpeed : 0f;
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
                                    .Subscribe(_ => onmyoState.Value = System.Math.Min(onmyoState.Value + Time.deltaTime / durations[0], SunMoonSystemModel.MAX));

                                break;
                            case OnmyoStatePriority.ChargeMoon:
                                modelUpdObservable = model.UpdateAsObservable()
                                    .Subscribe(_ => onmyoState.Value = System.Math.Max(onmyoState.Value - Time.deltaTime / durations[0], SunMoonSystemModel.MIN));

                                break;
                            case OnmyoStatePriority.CompleteSun:
                                onmyoState.Value = SunMoonSystemModel.MAX;

                                break;
                            case OnmyoStatePriority.CompleteMoon:
                                onmyoState.Value = SunMoonSystemModel.MIN;

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
        /// モデルコンポーネントを監視して第1引数へセットされた値を更新
        /// 入力された内容に基づいて昼／夜の状態を変更
        /// </summary>
        /// <param name="onmyoState">陰陽（昼夜）の状態</param>
        /// <param name="durations">ボタン押下の時間管理</param>
        /// <param name="model">陰陽（昼夜）の切り替えモデル</param>
        /// <returns>成功／失敗</returns>
        public bool SetOnmyoStateInModel(IReactiveProperty<float> onmyoState, float[] durations, SunMoonSystemModel model);
    }
}
