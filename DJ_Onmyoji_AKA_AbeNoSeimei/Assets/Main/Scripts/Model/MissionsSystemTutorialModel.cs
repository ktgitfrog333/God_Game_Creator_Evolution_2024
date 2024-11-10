using Main.Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// チュートリアルの中のミッションを管理する
    /// モデル
    /// </summary>
    public class MissionsSystemTutorialModel : MonoBehaviour, IMissionsSystemTutorialModel
    {
        /// <summary>進行中のミッションID</summary>
        public IReactiveProperty<MissionID> CallMissionID => throw new System.NotImplementedException();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool SetCallMissionID(GuideMessageID guideMessageID)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateKilledEnemyCount()
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// チュートリアルの中のミッションを管理する
    /// インターフェース
    /// </summary>
    public interface IMissionsSystemTutorialModel
    {
        /// <summary>
        /// 進行中のミッションID（CallMissionID）を更新する
        /// </summary>
        /// <param name="guideMessageID">ガイドメッセージID</param>
        /// <returns>成功／失敗</returns>
        /// <remarks>
        /// ガイドメッセージIDとミッションIDを紐づけて管理
        /// 管理されたテーブルから紐づけた結果を返す
        /// </remarks>
        public bool SetCallMissionID(GuideMessageID guideMessageID);
        /// <summary>
        /// 敵の撃破数（子）を更新する
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool UpdateKilledEnemyCount();
    }
}
