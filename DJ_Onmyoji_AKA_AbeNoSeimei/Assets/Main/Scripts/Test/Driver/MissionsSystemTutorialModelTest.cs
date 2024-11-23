// ChatGPT 4o
using System.Linq;
using UnityEngine;
using UniRx;
using Main.Common;
using Main.Model;
using UniRx.Triggers;

namespace Main.Test.Driver
{
    public class MissionsSystemTutorialModelTest : MonoBehaviour
    {
        [SerializeField] private MissionsSystemTutorialModel missionsSystemTutorialModel;
        [SerializeField] private GuideMessageID testGuideMessageID;

        private void Reset()
        {
            missionsSystemTutorialModel = GameObject.FindObjectOfType<MissionsSystemTutorialModel>();
        }

        private void Start()
        {
            if (missionsSystemTutorialModel == null)
            {
                Debug.LogError("MissionsSystemTutorialModelが割り当てられていません！");
                return;
            }

            System.IDisposable modelUpdObservable = this.UpdateAsObservable().Subscribe(_ => { });
            modelUpdObservable.Dispose();
            // 進行中のミッションIDの更新を監視してデバッグ出力
            missionsSystemTutorialModel.CallMissionID
                .ObserveEveryValueChanged(x => x.Value)
                .Subscribe(missionID =>
                {
                    modelUpdObservable.Dispose();

                    Debug.Log($"進行中のミッションIDが更新されました: {missionID}");

                    switch (missionID)
                    {
                        case MissionID.MI0000:
                            break;
                        case MissionID.MI0001:
                            modelUpdObservable = this.UpdateAsObservable()
                                .Select(x => missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct)
                                .Where(currentStruct => currentStruct.killedEnemyCount != null &&
                                    currentStruct.isCompleted != null)
                                .Subscribe(currentStruct =>
                                {
                                    // 現在のミッション情報をデバッグ出力
                                    var currentMission = currentStruct;
                                    Debug.Log($"現在実行中のミッション情報: ID={currentMission.missionID}, 撃破数={currentMission.killedEnemyCount.Value}/{currentMission.killedEnemyCountMax}, 完了={currentMission.isCompleted.Value}");
                                })
                                .AddTo(missionsSystemTutorialModel);

                            break;
                        case MissionID.MI0002:
                            modelUpdObservable = this.UpdateAsObservable()
                                .Select(x => missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct)
                                .Where(currentStruct => currentStruct.killedEnemyCount != null &&
                                    currentStruct.isCompleted != null)
                                .Subscribe(currentStruct =>
                                {
                                    // 現在のミッション情報をデバッグ出力
                                    var currentMission = missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct;
                                    Debug.Log($"現在実行中のミッション情報: ID={currentMission.missionID}, 撃破数={currentMission.killedEnemyCount.Value}/{currentMission.killedEnemyCountMax}, 完了={currentMission.isCompleted.Value}");
                                })
                                .AddTo(missionsSystemTutorialModel);

                            break;
                        default:
                            break;
                    }
                })
                .AddTo(this);
        }

        private void OnGUI()
        {
            // SetCallMissionIDボタン
            if (GUI.Button(new Rect(10, 10, 200, 50), "Set Call Mission ID"))
            {
                if (missionsSystemTutorialModel.SetCallMissionID(testGuideMessageID))
                {
                    Debug.Log($"SetCallMissionIDが成功しました: {testGuideMessageID}");
                }
                else
                {
                    Debug.LogError("SetCallMissionIDが失敗しました");
                }
            }

            // UpdateKilledEnemyCountボタン
            if (GUI.Button(new Rect(10, 70, 200, 50), "Update Killed Enemy Count"))
            {
                if (missionsSystemTutorialModel.UpdateKilledEnemyCount())
                {
                    Debug.Log("UpdateKilledEnemyCountが成功しました");
                }
                else
                {
                    Debug.LogError("UpdateKilledEnemyCountが失敗しました");
                }
            }
        }
    }
}
