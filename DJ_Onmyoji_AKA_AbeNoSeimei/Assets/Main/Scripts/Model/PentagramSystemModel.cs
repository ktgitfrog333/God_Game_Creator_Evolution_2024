using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.InputSystem;
using UniRx;
using UniRx.Triggers;
using Main.Common;
using UnityEngine.InputSystem;

namespace Main.Model
{
    /// <summary>
    /// ペンダグラムシステム
    /// デバイスの入力情報を内部管理する
    /// モデル
    /// </summary>
    public class PentagramSystemModel : MonoBehaviour
    {
        /// <summary>入力角度</summary>
        public IReactiveProperty<float> InputValue { get; private set; }
        /// <summary>距離の補正乗算値</summary>
        private float _multiDistanceCorrected = 7.5f;
        public int UpdateCnt {get; private set;}

        private void Start()
        {
            InputSystemsOwner inputSystemsOwner = null;
            InputValue = new FloatReactiveProperty();

            Vector2ReactiveProperty previousInput = new Vector2ReactiveProperty(Vector2.zero); // 前回の入力を保存する変数
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (inputSystemsOwner == null)
                        inputSystemsOwner = MainGameManager.Instance != null ? MainGameManager.Instance.InputSystemsOwner : null;
                    else
                    {
                        Vector2 currentInput = inputSystemsOwner.InputUI.Scratch; // 現在の入力を取得
                        if (isPerformed(previousInput.Value.sqrMagnitude, currentInput.sqrMagnitude)) // 前回と今回の入力が十分に大きい場合
                        {
                            float angle = Vector2.SignedAngle(previousInput.Value, currentInput) * -1f; // 前回の入力から今回の入力への角度を計算
                            float distance = Mathf.PI * angle / 180; // 角度を円周の長さに変換
                            // Debug.Log($"Update");
                            UpdateCnt++;
                            InputValue.Value = Mathf.Clamp(distance * _multiDistanceCorrected, -1f, 1f);
                        }
                        previousInput.Value = currentInput; // 現在の入力を保存
                    }
                });
        }

        /// <summary>
        /// 回転の操作中か
        /// </summary>
        /// <param name="prevMagnitude">直前の入力値</param>
        /// <param name="currentMagnitude">現在の入力値</param>
        /// <returns>真／偽</returns>
        private bool isPerformed(float prevMagnitude, float currentMagnitude)
        {
            return 0f < prevMagnitude &&
                0f < currentMagnitude;
        }
    }
}
