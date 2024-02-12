using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Main.Common;
using UniRx;
using UniRx.Triggers;

namespace Main.InputSystem
{
    /// <summary>
    /// 入力情報の履歴
    /// </summary>
    public class InputHistroy : MonoBehaviour, IMainGameManager
    {
        /// <summary>入力コマンドタイプID</summary>
        public InputTypeID InputTypeID { get; private set; } = InputTypeID.None;
        /// <summary>入力コマンドタイプIDに紐づくパターン</summary>
        [SerializeField] private ResultPatternState[] resultPatternState;
        /// <summary>入力履歴管理期間（ミリ秒）</summary>
        [SerializeField] private float cmdAcceptanceTimeMs = .25f;

        public void OnStart()
        {
            List<InputRecord> inputHistory = new List<InputRecord>();
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    inputHistory = UpdateInputHistroy(resultPatternState, inputHistory, cmdAcceptanceTimeMs, MainGameManager.Instance.InputSystemsOwner);
                    if (inputHistory == null)
                        Debug.LogError("UpdateInputHistroy");
                });
        }

        /// <summary>
        /// 入力履歴の更新
        /// </summary>
        /// <param name="resultPatternState">入力コマンドタイプIDに紐づくパターン</param>
        /// <param name="inputHistory">入力履歴</param>
        /// <param name="cmdAcceptanceTimeMs">入力履歴管理期間（ミリ秒）</param>
        /// <param name="inputSystemsOwner">InputSystemのオーナー</param>
        /// <returns>更新後の入力履歴</returns>
        private List<InputRecord> UpdateInputHistroy(ResultPatternState[] resultPatternState, List<InputRecord> inputHistory, float cmdAcceptanceTimeMs, InputSystemsOwner inputSystemsOwner)
        {
            try
            {
                // 入力をチェックし、履歴に追加する
                InputType currentInput = GetCurrentInput(inputSystemsOwner);
                if (inputHistory.Count < 1 ||
                currentInput != inputHistory[inputHistory.Count - 1].Input)
                {
                    // Noneも許容するが同じ入力は許容しない
                    inputHistory.Add(new InputRecord { Input = currentInput, Time = Time.time });
                }

                // 古い入力を削除する
                inputHistory.RemoveAll(record => Time.time - record.Time > cmdAcceptanceTimeMs);

                // 入力パターンをチェックする
                var id = CheckInputPattern(resultPatternState, inputHistory);
                if (!id.Equals(InputTypeID.None))
                    InputTypeID = id;
                else
                    InputTypeID = InputTypeID.None;

                return inputHistory;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// 入力をチェックし、履歴に追加する
        /// </summary>
        /// <param name="inputSystemsOwner">InputSystemのオーナー</param>
        /// <returns>昼／夜チャージ入力</returns>
        private InputType GetCurrentInput(InputSystemsOwner inputSystemsOwner)
        {
            // ここで現在の入力を取得する
            var chargeSun = inputSystemsOwner.InputUI.ChargeSun;
            var chargeMoon = inputSystemsOwner.InputUI.ChargeMoon;
            InputType result = InputType.None;
            if (chargeSun ||
            chargeMoon)
            {
                if (chargeSun)
                    result = InputType.ChargeSun;
                if (chargeMoon)
                    result = InputType.ChargeMoon;
            }

            return result;
        }

        /// <summary>
        /// 入力パターンをチェックする
        /// </summary>
        /// <param name="resultPatternState">入力コマンドタイプIDに紐づくパターン</param>
        /// <param name="inputHistory">入力履歴</param>
        /// <returns>昼／夜チャージ入力ID</returns>
        private InputTypeID CheckInputPattern(ResultPatternState[] resultPatternState, List<InputRecord> inputHistory)
        {
            if ((resultPatternState.Length - 1) < resultPatternState.Where(q => inputHistory.Count < q.targetPattern.Length)
            .Select(q => q)
            .ToArray()
            .Length)
                return InputTypeID.None;

            foreach (var item in resultPatternState)
                for (int i = 0; i <= inputHistory.Count - item.targetPattern.Length; i++)
                    if (inputHistory.Skip(i).Take(item.targetPattern.Length).Select(q => q.Input).SequenceEqual(item.targetPattern))
                        return item.inputTypeID;

            return InputTypeID.None;
        }
    }

    /// <summary>
    /// 入力コマンドタイプIDに紐づくパターン
    /// </summary>
    [System.Serializable]
    public struct ResultPatternState
    {
        /// <summary>昼／夜チャージ入力ID</summary>
        public InputTypeID inputTypeID;
        /// <summary>昼／夜チャージ入力配列</summary>
        public InputType[] targetPattern;
    }

    /// <summary>
    /// 昼／夜チャージ入力
    /// </summary>
    public enum InputType
    {
        None = -1,
        /// <summary>昼チャージ入力</summary>
        ChargeSun,
        /// <summary>夜チャージ入力</summary>
        ChargeMoon,
    }

    /// <summary>
    /// 入力履歴
    /// </summary>
    [System.Serializable]
    public class InputRecord
    {
        /// <summary>昼／夜チャージ入力</summary>
        public InputType Input;
        /// <summary>タイムスタンプ</summary>
        public float Time;
    }

    /// <summary>
    /// 昼／夜チャージ入力ID
    /// </summary>
    public enum InputTypeID
    {
        None = -1,
        IT0001,
        IT0002,
        IT0003,
        IT0004,
        IT0005,
        IT0006,
        IT0007,
        IT0008,
        IT0009,
        IT0010,
        IT0011,
        IT0012,
        IT0013,
        IT0014,
        IT0015,
        IT0016,
        IT0017,
        IT0018,
        IT0019,
        IT0020,
        IT0021,
        IT0022,
        IT0023,
        IT0024,
        IT0025,
        IT0026,
        IT0027,
        IT0028,
        IT0029,
        IT0030,
    }
}
