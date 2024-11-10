using Main.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Presenter;
using Main.Model;
using System.Linq;
using Main.View;
using UniRx;

namespace Main.Utility
{
    /// <summary>
    /// チュートリアルのユーティリティ
    /// </summary>
    public class MainTutorialsUtility : IMainTutorialsUtility
    {
        public bool InitializeTutorialGuideContentsOfPentagramTurnTableModel(MainPresenterDemo.TutorialGuideContentsStuct tutorialGuideContentsStuct, PentagramTurnTableModel pentagramTurnTableModel)
        {
            try
            {
                if (pentagramTurnTableModel.WrapTurretModel == null ||
                    pentagramTurnTableModel.DanceTurretModel == null ||
                    pentagramTurnTableModel.GraffitiTurretModel == null ||
                    pentagramTurnTableModel.OnmyoTurretModels == null ||
                    pentagramTurnTableModel.OnmyoTurretModels.Length != 2)
                    throw new System.ArgumentNullException("ペンダグラムターンテーブルに含まれる式神情報がnull");

                // 対象のコンポーネントをセット
                List<Component> targetComponents = tutorialGuideContentsStuct.targetComponents.ToList();
                targetComponents.Add(pentagramTurnTableModel.WrapTurretModel);
                targetComponents.Add(pentagramTurnTableModel.DanceTurretModel);
                targetComponents.Add(pentagramTurnTableModel.GraffitiTurretModel);
                targetComponents.AddRange(pentagramTurnTableModel.OnmyoTurretModels);
                tutorialGuideContentsStuct.targetComponents = targetComponents.ToArray();
                // チュートリアルで扱うリソースの構造体をセット
                foreach (var tutorialComponentMap in tutorialGuideContentsStuct.tutorialComponentMaps.Select((p, i) => new { Content = p, Index = i }))
                {
                    switch (tutorialComponentMap.Content.guideMessageID)
                    {
                        case GuideMessageID.GM0000:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0001:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0002:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0003:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps, MainPresenterDemo.ComponentState.Enable))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0004:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0005:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps, MainPresenterDemo.ComponentState.Enable))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0006:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0007:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0008:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0009:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0010:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0011:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

                            break;
                        case GuideMessageID.GM0012:
                            if (!AddTutorialComponents(tutorialComponentMap.Content, pentagramTurnTableModel, tutorialComponentMap.Index, tutorialGuideContentsStuct.tutorialComponentMaps))
                                throw new System.ArgumentException("AddTutorialComponents");

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
        /// チュートリアルで扱うガイドIDとリソースの構造体へ
        /// ペンダグラムターンテーブルのモデルの式神情報を追加
        /// </summary>
        /// <param name="tutorialComponentMap">チュートリアルで扱うガイドIDとリソースの構造体</param>
        /// <param name="pentagramTurnTableModel">ペンダグラムターンテーブルのモデル</param>
        /// <param name="index">インデックス</param>
        /// <param name="tutorialComponentMaps">チュートリアルで扱うガイドIDとリソースの構造体</param>
        /// <param name="componentState">コンポーネント、【有効、無効、一時停止】</param>
        /// <returns>成功／失敗</returns>
        private bool AddTutorialComponents(MainPresenterDemo.TutorialComponentMap tutorialComponentMap, PentagramTurnTableModel pentagramTurnTableModel, int index, MainPresenterDemo.TutorialComponentMap[] tutorialComponentMaps, MainPresenterDemo.ComponentState componentState=MainPresenterDemo.ComponentState.Pause)
        {
            try
            {
                List<MainPresenterDemo.TutorialComponent> tutorialComponents = tutorialComponentMap.tutorialComponents.ToList();
                tutorialComponents.Add(new MainPresenterDemo.TutorialComponent()
                {
                    component = pentagramTurnTableModel.WrapTurretModel,
                    componentState = componentState
                });
                tutorialComponents.Add(new MainPresenterDemo.TutorialComponent()
                {
                    component = pentagramTurnTableModel.DanceTurretModel,
                    componentState = componentState
                });
                tutorialComponents.Add(new MainPresenterDemo.TutorialComponent()
                {
                    component = pentagramTurnTableModel.GraffitiTurretModel,
                    componentState = componentState
                });
                tutorialComponents.AddRange(new MainPresenterDemo.TutorialComponent[]
                {
                    new MainPresenterDemo.TutorialComponent()
                    {
                        component = pentagramTurnTableModel.OnmyoTurretModels[0],
                        componentState = componentState
                    },
                    new MainPresenterDemo.TutorialComponent()
                    {
                        component = pentagramTurnTableModel.OnmyoTurretModels[1],
                        componentState = componentState
                    },
                });
                tutorialComponentMaps[index].tutorialComponents = tutorialComponents.ToArray();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool DoTutorialGuideContents(GuideMessageID guideMessageID, MainPresenterDemo.TutorialGuideContentsStuct tutorialGuideContentsStuct)
        {
            try
            {
                var tutorialComponentMaps = tutorialGuideContentsStuct.tutorialComponentMaps.Where(q => q.guideMessageID.Equals(guideMessageID))
                    .Select(q => q.tutorialComponents)
                    .ToArray();
                foreach (var tutorialComponents in tutorialComponentMaps)
                {
                    foreach (var tutorialComponent in tutorialComponents)
                    {
                        // ChatGPT 4o
                        switch (tutorialComponent.component)
                        {
                            case PentagramSystemModel targetViewOrModel:
                                var target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case ShikigamiSkillSystemModel targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case TurretModel targetViewOrModel:
                                var target1 = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel))
                                    .Select(q => (TurretModel)q).ToArray();
                                if (target1.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target1.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target1))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case SunMoonSystemModel targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case ClearCountdownTimerSystemModel targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case PauseView targetViewOrModel:
                                var target2 = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel))
                                    .Select(q => (PauseView)q).ToArray();
                                if (target2.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target2.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target2))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case FadersGroupView targetViewOrModel:
                                var target3 = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel))
                                    .Select(q => (FadersGroupView)q).ToArray();
                                if (target3.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target3.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target3))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideMessageView targetViewOrModel:
                                var target4 = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel))
                                    .Select(q => (GuideMessageView)q).ToArray();
                                if (target4.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target4.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target4))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideUITheTurntableView targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideUITheEqualizerView targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideUITheEqualizerGageView targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideUITheDJEnergyView targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideUIThePlayerHPView targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideUITheClearCountdownTimerCircleView targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            case GuideUITheClearRewardTextContentsView targetViewOrModel:
                                target = tutorialGuideContentsStuct.targetComponents.Where(q => q.Equals(targetViewOrModel)).ToArray();
                                if (target.Length != 1)
                                    throw new System.ArgumentException($"対象のコンポーネントは一つのみ指定: [{tutorialComponent.component.GetType().FullName}][{target.Length}]");

                                if (!CheckComponentStateAndSetActive(tutorialComponent, target))
                                    throw new System.Exception("CheckComponentStateAndSetActive");

                                break;
                            default:
                                Debug.LogWarning($"Unhandled component type: {tutorialComponent.component.GetType().FullName}");
                                break;
                        }
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
        /// コンポーネントの指定された状態をチェックして
        /// オブジェクトを有効、無効、一時停止にする
        /// </summary>
        /// <param name="tutorialComponent">チュートリアルで扱うリソースの構造体</param>
        /// <param name="target">対象のコンポーネント</param>
        /// <returns>成功／失敗</returns>
        private bool CheckComponentStateAndSetActive(MainPresenterDemo.TutorialComponent tutorialComponent, Component[] target)
        {
            try
            {
                switch (tutorialComponent.componentState)
                {
                    case MainPresenterDemo.ComponentState.Disable:
                        foreach (var item in target.Where(q => q.gameObject.activeSelf))
                            item.gameObject.SetActive(false);

                        break;
                    case MainPresenterDemo.ComponentState.Enable:
                        foreach (var item in target.Where(q => !q.gameObject.activeSelf))
                            item.gameObject.SetActive(true);

                        break;
                    case MainPresenterDemo.ComponentState.Pause:
                        throw new System.NotImplementedException($"必要に応じて処理を追加する: [{tutorialComponent.component.GetType().FullName}]");

                    default:
                        break;
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
        /// コンポーネントの指定された状態をチェックして
        /// オブジェクトを有効、無効、一時停止にする
        /// </summary>
        /// <param name="tutorialComponent">チュートリアルで扱うリソースの構造体</param>
        /// <param name="target">対象のコンポーネント</param>
        /// <returns>成功／失敗</returns>
        private bool CheckComponentStateAndSetActive(MainPresenterDemo.TutorialComponent tutorialComponent, TurretModel[] target)
        {
            try
            {
                switch (tutorialComponent.componentState)
                {
                    case MainPresenterDemo.ComponentState.Disable:
                        foreach (var item in target.Where(q => q.gameObject.activeSelf))
                            item.gameObject.SetActive(false);

                        break;
                    case MainPresenterDemo.ComponentState.Enable:
                        foreach (var item in target.Where(q => !q.gameObject.activeSelf))
                            item.gameObject.SetActive(true);
                        foreach (var item in target)
                            if (!item.SetAutoInstanceMode(true))
                                throw new System.ArgumentException("SetAutoInstanceMode");

                        break;
                    case MainPresenterDemo.ComponentState.Pause:
                        foreach (var item in target)
                            if (!item.SetAutoInstanceMode(false))
                                throw new System.ArgumentException("SetAutoInstanceMode");

                        break;
                    default:
                        break;
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
        /// コンポーネントの指定された状態をチェックして
        /// オブジェクトを有効、無効、一時停止にする
        /// </summary>
        /// <param name="tutorialComponent">チュートリアルで扱うリソースの構造体</param>
        /// <param name="target">対象のコンポーネント</param>
        /// <returns>成功／失敗</returns>
        private bool CheckComponentStateAndSetActive(MainPresenterDemo.TutorialComponent tutorialComponent, PauseView[] target)
        {
            try
            {
                switch (tutorialComponent.componentState)
                {
                    case MainPresenterDemo.ComponentState.Disable:
                        foreach (var item in target.Where(q => q.gameObject.activeSelf))
                            item.gameObject.SetActive(false);

                        break;
                    case MainPresenterDemo.ComponentState.Enable:
                        foreach (var item in target.Where(q => !q.gameObject.activeSelf))
                            item.gameObject.SetActive(true);
                        foreach (var item in target)
                            if (!item.SetControllEnabled(true))
                                throw new System.ArgumentException("SetControllEnabled");

                        break;
                    case MainPresenterDemo.ComponentState.Pause:
                        foreach (var item in target)
                            if (!item.SetControllEnabled(false))
                                throw new System.ArgumentException("SetControllEnabled");

                        break;
                    default:
                        break;
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
        /// コンポーネントの指定された状態をチェックして
        /// オブジェクトを有効、無効、一時停止にする
        /// </summary>
        /// <param name="tutorialComponent">チュートリアルで扱うリソースの構造体</param>
        /// <param name="target">対象のコンポーネント</param>
        /// <returns>成功／失敗</returns>
        private bool CheckComponentStateAndSetActive(MainPresenterDemo.TutorialComponent tutorialComponent, FadersGroupView[] target)
        {
            try
            {
                switch (tutorialComponent.componentState)
                {
                    case MainPresenterDemo.ComponentState.Disable:
                        foreach (var item in target.Where(q => q.gameObject.activeSelf))
                            item.gameObject.SetActive(false);

                        break;
                    case MainPresenterDemo.ComponentState.Enable:
                        foreach (var item in target.Where(q => !q.gameObject.activeSelf))
                            item.gameObject.SetActive(true);
                        foreach (var item in target)
                        {
                            Observable.FromCoroutine<bool>(observer => item.PlayMoveAnchorsHeight(observer))
                                .Subscribe(_ => { })
                                .AddTo(item);
                        }

                        break;
                    case MainPresenterDemo.ComponentState.Pause:
                        foreach (var item in target)
                        {
                            Observable.FromCoroutine<bool>(observer => item.PlayMoveAnchorsBased(observer))
                                .Subscribe(_ => { })
                                .AddTo(item);
                        }

                        break;
                    default:
                        break;
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
        /// コンポーネントの指定された状態をチェックして
        /// オブジェクトを有効、無効、一時停止にする
        /// </summary>
        /// <param name="tutorialComponent">チュートリアルで扱うリソースの構造体</param>
        /// <param name="target">対象のコンポーネント</param>
        /// <returns>成功／失敗</returns>
        private bool CheckComponentStateAndSetActive(MainPresenterDemo.TutorialComponent tutorialComponent, GuideMessageView[] target)
        {
            try
            {
                switch (tutorialComponent.componentState)
                {
                    case MainPresenterDemo.ComponentState.Disable:
                        foreach (var item in target.Where(q => q.gameObject.activeSelf))
                            item.gameObject.SetActive(false);

                        break;
                    case MainPresenterDemo.ComponentState.Enable:
                        foreach (var item in target.Where(q => !q.gameObject.activeSelf))
                            item.gameObject.SetActive(true);
                        foreach (var item in target)
                            if (!item.SetButtonEnabled(true))
                                throw new System.ArgumentException("SetButtonEnabled");

                        break;
                    case MainPresenterDemo.ComponentState.Pause:
                        foreach (var item in target)
                            if (!item.SetButtonEnabled(false))
                                throw new System.ArgumentException("SetButtonEnabled");

                        break;
                    default:
                        break;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool DoTutorialMissionContents(MissionID missionID, MainPresenterDemo.TutorialMissionContentsStuct tutorialMissionContentsStuct)
        {
            try
            {
                var tutorialStuct = tutorialMissionContentsStuct;
                switch (missionID)
                {
                    case MissionID.MI0000:
                        // 監視の破棄は元モデルで実施

                        break;
                    case MissionID.MI0001:
                        tutorialStuct.missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct.killedEnemyCount.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                if (!tutorialStuct.guideMessageView.UpdateSentence(missionID, x, tutorialStuct.missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct.killedEnemyCountMax))
                                    Debug.LogError("UpdateSentence");
                            });
                        tutorialStuct.missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct.isCompleted.ObserveEveryValueChanged(x => x.Value)
                            .Where(x => x)
                            .Subscribe(x =>
                            {
                                if (!tutorialStuct.guideMessageView.SetButtonEnabled(true))
                                    Debug.LogError("SetButtonEnabled");
                                MainGameManager.Instance.InputSystemsOwner.InputMidiJackDDJ200.AutoPushCue();
                            });
                        if (!tutorialStuct.enemiesSpawnTutorialModel.InstanceTamachans())
                            Debug.LogError("InstanceTamachans");

                        break;
                    case MissionID.MI0002:
                        tutorialStuct.missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct.killedEnemyCount.ObserveEveryValueChanged(x => x.Value)
                            .Subscribe(x =>
                            {
                                if (!tutorialStuct.guideMessageView.UpdateSentence(missionID,x, tutorialStuct.missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct.killedEnemyCountMax))
                                    Debug.LogError("UpdateSentence");
                            });
                        tutorialStuct.missionsSystemTutorialModel.CurrentMissionsSystemTutorialStruct.isCompleted.ObserveEveryValueChanged(x => x.Value)
                            .Where(x => x)
                            .Subscribe(x =>
                            {
                                if (!tutorialStuct.guideMessageView.SetButtonEnabled(true))
                                    Debug.LogError("SetButtonEnabled");
                                MainGameManager.Instance.InputSystemsOwner.InputMidiJackDDJ200.AutoPushCue();
                            });
                        if (!tutorialStuct.enemiesSpawnTutorialModel.InstanceAuraTamachans())
                            Debug.LogError("InstanceAuraTamachans");

                        break;
                }

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
    /// チュートリアルのユーティリティ
    /// インターフェース
    /// </summary>
    public interface IMainTutorialsUtility
    {
        /// <summary>
        /// チュートリアルのガイドで扱うリソースの構造体の初期処理
        /// ペンダグラムターンテーブルのモデルの情報をセット
        /// </summary>
        /// <param name="tutorialGuideContentsStuct">チュートリアルのガイドで扱うリソースの構造体</param>
        /// <param name="pentagramTurnTableModel">ペンダグラムターンテーブルのモデル</param>
        /// <returns>成功／失敗</returns>
        public bool InitializeTutorialGuideContentsOfPentagramTurnTableModel(MainPresenterDemo.TutorialGuideContentsStuct tutorialGuideContentsStuct, PentagramTurnTableModel pentagramTurnTableModel);
        /// <summary>
        /// チュートリアルのガイドコンテンツを実行する
        /// </summary>
        /// <param name="guideMessageID">ガイドメッセージID</param>
        /// <param name="tutorialGuideContentsStuct">チュートリアルのガイドで扱うリソースの構造体</param>
        /// <returns>成功／失敗</returns>
        public bool DoTutorialGuideContents(GuideMessageID guideMessageID, MainPresenterDemo.TutorialGuideContentsStuct tutorialGuideContentsStuct);
        /// <summary>
        /// チュートリアルのミッションコンテンツを実行する
        /// </summary>
        /// <param name="missionID">ミッションID</param>
        /// <param name="tutorialMissionContentsStuct">チュートリアルのミッションで扱うリソースの構造体</param>
        /// <returns>成功／失敗</returns>
        public bool DoTutorialMissionContents(MissionID missionID, MainPresenterDemo.TutorialMissionContentsStuct tutorialMissionContentsStuct);
    }
}
