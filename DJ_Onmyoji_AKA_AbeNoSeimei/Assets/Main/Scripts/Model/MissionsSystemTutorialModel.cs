using Main.Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor.XR;
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
        /// <summary>チュートリアルの中のミッションを管理する構造体</summary>
        [SerializeField] private MissionsSystemTutorialStruct[] missionsSystemTutorialStructs;
        /// <summary>現在実行中のミッション情報</summary>
        public MissionsSystemTutorialStruct CurrentMissionsSystemTutorialStruct => throw new System.NotImplementedException();

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

        //public bool DisposeKilledEnemyCount()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public bool DisposeIsCompleted()
        //{
        //    throw new System.NotImplementedException();
        //}
    }


    /// <summary>
    /// チュートリアルの中のミッションを管理する
    /// 構造体
    /// </summary>
    public struct MissionsSystemTutorialStruct
    {
        public MissionID missionID;
        public IntReactiveProperty killedEnemyCount;
        public int killedEnemyCountMax;
        public BoolReactiveProperty isCompleted;
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
        ///// <summary>
        ///// 子の数を破棄
        ///// </summary>
        ///// <returns>成功／失敗</returns>
        //public bool DisposeKilledEnemyCount();
        ///// <summary>
        ///// ミッション完了フラグを破棄
        ///// </summary>
        ///// <returns>成功／失敗</returns>
        //public bool DisposeIsCompleted();
    }
}
