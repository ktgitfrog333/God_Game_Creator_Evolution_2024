namespace Select.Common
{
    public class AnalyticsOwner : Title.Common.AnalyticsOwner, IAnalyticsOwner { }

    /// <summary>
    /// アナリティクス
    /// インタフェース
    /// </summary>
    public interface IAnalyticsOwner
    {
        /// <summary>
        /// UIイベントが更新された際に発火。イベント状態を取得。
        /// </summary>
        /// <param name="eventState">イベント状態</param>
        /// <param name="gameObjectName">ゲームオブジェクト名</param>
        /// <returns>成功／失敗</returns>
        public bool OnUpdateEventState(int eventState, string gameObjectName);
    }
}
