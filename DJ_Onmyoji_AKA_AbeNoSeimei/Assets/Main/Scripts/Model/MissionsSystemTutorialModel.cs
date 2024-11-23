using Main.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IReactiveProperty<MissionID> _callMissionID = new ReactiveProperty<MissionID>();
        /// <summary>進行中のミッションID</summary>
        public IReactiveProperty<MissionID> CallMissionID => _callMissionID;
        /// <summary>チュートリアルの中のミッションを管理する構造体</summary>
        [SerializeField]
        private MissionsSystemTutorialStruct[] missionsSystemTutorialStructs = new MissionsSystemTutorialStruct[]
        {
            new MissionsSystemTutorialStruct()
            {
                missionID = MissionID.MI0001,
                killedEnemyCountMax = 3,
                killedEnemyCount = new IntReactiveProperty(),
                isCompleted = new BoolReactiveProperty(),
            },
            new MissionsSystemTutorialStruct()
            {
                missionID = MissionID.MI0002,
                killedEnemyCountMax = 9,
                killedEnemyCount = new IntReactiveProperty(),
                isCompleted = new BoolReactiveProperty(),
            },
        };
        /// <summary>現在実行中のミッション情報</summary>
        private MissionsSystemTutorialStruct _currentMissionsSystemTutorialStruct;
        /// <summary>現在実行中のミッション情報</summary>
        public MissionsSystemTutorialStruct CurrentMissionsSystemTutorialStruct => _currentMissionsSystemTutorialStruct;
        /// <summary>ガイドメッセージIDからミッションIDへ置換するマップ</summary>
        [SerializeField] private ConvertMapMissionIDToGuideMessageID[] convertMapMissionIDToGuideMessageIDs = new ConvertMapMissionIDToGuideMessageID[]
        {
            new ConvertMapMissionIDToGuideMessageID()
            {
                guideMessageID = GuideMessageID.GM0003,
                missionID = MissionID.MI0001
            },
            new ConvertMapMissionIDToGuideMessageID()
            {
                guideMessageID = GuideMessageID.GM0005,
                missionID = MissionID.MI0002
            },
        };

        private void Start()
        {
            _callMissionID.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    var currentMissionsSystemTutorialStruct = missionsSystemTutorialStructs.FirstOrDefault(q => q.missionID.Equals(x));
                    // 目標の撃破数 <= 実際の撃破数 となった場合にミッション完了フラグを有効にする
                    currentMissionsSystemTutorialStruct.killedEnemyCount.ObserveEveryValueChanged(x => x.Value)
                        .Where(q => currentMissionsSystemTutorialStruct.killedEnemyCountMax <= q)
                        .Subscribe(_ => currentMissionsSystemTutorialStruct.isCompleted.Value = true);
                    _currentMissionsSystemTutorialStruct = currentMissionsSystemTutorialStruct;
                });
        }

        public bool SetCallMissionID(GuideMessageID guideMessageID)
        {
            try
            {
                var missionIDs = convertMapMissionIDToGuideMessageIDs.Where(q => q.guideMessageID.Equals(guideMessageID))
                    .ToArray();
                if (0 < missionIDs.Length)
                    _callMissionID.Value = missionIDs[0].missionID;
                else
                    _callMissionID.Value = MissionID.MI0000;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool UpdateKilledEnemyCount()
        {
            try
            {
                if (_currentMissionsSystemTutorialStruct.killedEnemyCount != null)
                    _currentMissionsSystemTutorialStruct.killedEnemyCount.Value++;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }


    /// <summary>
    /// チュートリアルの中のミッションを管理する
    /// 構造体
    /// </summary>
    [System.Serializable]
    public struct MissionsSystemTutorialStruct
    {
        /// <summary>ミッションID</summary>
        public MissionID missionID;
        /// <summary>敵の撃破数</summary>
        public IntReactiveProperty killedEnemyCount;
        /// <summary>敵の撃破数（最大）</summary>
        public int killedEnemyCountMax;
        /// <summary>ミッション完了フラグ</summary>
        public BoolReactiveProperty isCompleted;
    }

    /// <summary>
    /// ガイドメッセージIDからミッションIDへ置換するマップ
    /// </summary>
    [System.Serializable]
    public struct ConvertMapMissionIDToGuideMessageID
    {
        /// <summary>ガイドメッセージID</summary>
        public GuideMessageID guideMessageID;
        /// <summary>ミッションID</summary>
        public MissionID missionID;
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
