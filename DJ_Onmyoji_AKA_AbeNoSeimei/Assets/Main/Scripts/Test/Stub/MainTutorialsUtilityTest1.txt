using Main.Common;
using Main.Presenter;
using Main.Test.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Main.Model;
using Main.View;

namespace Main.Test.Stub
{
    public class MainTutorialsUtilityTest1 : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public GuideMessageID guideMessageID;
            public MainPresenterDemo.TutorialGuideContentsStuct tutorialGuideContentsStuct;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public bool isSuccessed;
            public Component[] target;
            public string throwMessage;
        }

        private void Reset()
        {
            /*
             * 対象のコンポーネント
             *  PentagramSystemModel
             *  ShikigamiSkillSystemModel
             *  DanceTurretModel
             *  GraffitiTurretModel
             *  OnmyoTurretModel
             *  WrapTurretModel
             *  SunMoonSystemModel
             *  ClearCountdownTimerSystemModel
             *  PauseView
             *  FadersGroupView
             *  GuideMessageView
             *  GuideUITheTurntableView
             *  GuideUITheEqualizerView
             *  GuideUITheEqualizerGageView
             *  GuideUITheDJEnergyView
             *  GuideUIThePlayerHPView
             *  GuideUITheClearCountdownTimerCircleView
             *  GuideUITheClearRewardTextContentsView
             *  MissionsSystemTutorialModel（※ダミー用）
             *  
             * ガイドID：GM0000
             *  MissionsSystemTutorialModel以外
             *      false
             * ガイドID：GM0001
             *  PentagramSystemModel
             *      true
             *  PentagramSystemModelとMissionsSystemTutorialModel以外
             *      false
             * 
             * 対象のコンポーネント
             *  PentagramSystemModel
             *  PentagramSystemModel
             * 
             * パターン[0000]：
             *  対象のガイドIDに基づいて対象となる全てのコンポーネントが無効になる
             * パターン[0001]：
             *  対象のガイドIDに基づいて対象となる全てのコンポーネントの内、1つのコンポーネントが有効になり、他は無効になる
             * パターン[0002]：
             *  対象のコンポーネントが複数存在する
             */
            inputs = new Input[]
            {
                new Input()
                {
                    caseId = 0,
                    guideMessageID = GuideMessageID.GM0000,
                    tutorialGuideContentsStuct = new MainPresenterDemo.TutorialGuideContentsStuct()
                    {
                        targetComponents = new Component[19],
                        tutorialComponentMaps = SetMainPresenterDemoTutorialComponentMaps(),
                    }
                },
                new Input()
                {
                    caseId = 1,
                    guideMessageID = GuideMessageID.GM0001,
                    tutorialGuideContentsStuct = new MainPresenterDemo.TutorialGuideContentsStuct()
                    {
                        targetComponents = new Component[19],
                        tutorialComponentMaps = SetMainPresenterDemoTutorialComponentMaps(),
                    }
                },
                new Input()
                {
                    caseId = 2,
                    guideMessageID = GuideMessageID.GM0000,
                    tutorialGuideContentsStuct = new MainPresenterDemo.TutorialGuideContentsStuct()
                    {
                        targetComponents = new Component[19],
                        tutorialComponentMaps = SetMainPresenterDemoTutorialComponentMaps(),
                    }
                },
            };
            foreach (var input in inputs)
            {
                foreach (var targetComponent in input.tutorialGuideContentsStuct.targetComponents.Select((p, i) => new { Content = p, Index = i }))
                {
                    var name = $"MainTutorialsUtilityTest1_InputObject_{input.caseId:D4}_{targetComponent.Index}";
                    GameObject obj = new GameObject(name);
                    obj.transform.parent = transform;

                    switch (targetComponent.Index)
                    {
                        case 0:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<PentagramSystemModel>();

                            break;
                        case 1:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<ShikigamiSkillSystemModel>();

                            break;
                        case 2:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<DanceTurretModel>();

                            break;
                        case 3:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GraffitiTurretModel>();

                            break;
                        case 4:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<OnmyoTurretModel>();

                            break;
                        case 5:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<WrapTurretModel>();

                            break;
                        case 6:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<SunMoonSystemModel>();

                            break;
                        case 7:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<ClearCountdownTimerSystemModel>();

                            break;
                        case 8:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<PauseView>();

                            break;
                        case 9:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<FadersGroupView>();

                            break;
                        case 10:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideMessageView>();

                            break;
                        case 11:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideUITheTurntableView>();

                            break;
                        case 12:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideUITheEqualizerView>();

                            break;
                        case 13:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideUITheEqualizerGageView>();

                            break;
                        case 14:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideUITheDJEnergyView>();

                            break;
                        case 15:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideUIThePlayerHPView>();

                            break;
                        case 16:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideUITheClearCountdownTimerCircleView>();

                            break;
                        case 17:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<GuideUITheClearRewardTextContentsView>();

                            break;
                        case 18:
                            input.tutorialGuideContentsStuct.targetComponents[targetComponent.Index] = obj.AddComponent<MissionsSystemTutorialModel>();

                            break;
                    }
                }
                foreach (var tutorialComponentMap in input.tutorialGuideContentsStuct.tutorialComponentMaps)
                {
                    foreach (var tutorialComponent in tutorialComponentMap.tutorialComponents.Select((p, i) => new { Content = p, Index = i }))
                    {
                        var name = $"MainTutorialsUtilityTest1_InputObject_{input.caseId:D4}_{tutorialComponent.Index}";
                        GameObject obj = GameObject.Find(name);
                        switch (tutorialComponent.Index)
                        {
                            case 0:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<PentagramSystemModel>();

                                break;
                            case 1:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<ShikigamiSkillSystemModel>();

                                break;
                            case 2:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<DanceTurretModel>();

                                break;
                            case 3:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GraffitiTurretModel>();

                                break;
                            case 4:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<OnmyoTurretModel>();

                                break;
                            case 5:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<WrapTurretModel>();

                                break;
                            case 6:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<SunMoonSystemModel>();

                                break;
                            case 7:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<ClearCountdownTimerSystemModel>();

                                break;
                            case 8:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<PauseView>();

                                break;
                            case 9:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<FadersGroupView>();

                                break;
                            case 10:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideMessageView>();

                                break;
                            case 11:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideUITheTurntableView>();

                                break;
                            case 12:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideUITheEqualizerView>();

                                break;
                            case 13:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideUITheEqualizerGageView>();

                                break;
                            case 14:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideUITheDJEnergyView>();

                                break;
                            case 15:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideUIThePlayerHPView>();

                                break;
                            case 16:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideUITheClearCountdownTimerCircleView>();

                                break;
                            case 17:
                                tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<GuideUITheClearRewardTextContentsView>();

                                break;
                        }
                    }
                }
            }
            foreach (var input in inputs.Where(p => p.caseId == 1))
            {
                foreach (var tutorialComponentMap in input.tutorialGuideContentsStuct.tutorialComponentMaps.Where(q => q.guideMessageID.Equals(GuideMessageID.GM0001)))
                {
                    tutorialComponentMap.tutorialComponents[0].componentState = MainPresenterDemo.ComponentState.Enable;
                }
            }
            foreach (var input in inputs.Where(p => p.caseId == 2))
            {
                foreach (var tutorialComponentMap in input.tutorialGuideContentsStuct.tutorialComponentMaps.Where(q => q.guideMessageID.Equals(GuideMessageID.GM0000)))
                {
                    var obj = tutorialComponentMap.tutorialComponents[1].component.gameObject;
                    DestroyImmediate(obj.GetComponent<ShikigamiSkillSystemModel>());
                    tutorialComponentMap.tutorialComponents[1].component = obj.AddComponent<PentagramSystemModel>();
                }
            }
            outputs = new Output[]
            {
                new Output()
                {
                    caseId = 0,
                    isSuccessed = true,
                    target = new Component[19],
                    throwMessage = "",
                },
                new Output()
                {
                    caseId = 1,
                    isSuccessed = true,
                    target = new Component[19],
                    throwMessage = "",
                },
                new Output()
                {
                    caseId = 2,
                    isSuccessed = false,
                    target = new Component[19],
                    throwMessage = "",
                },
            };
            foreach (var output in outputs)
            {
                foreach (var target in output.target.Select((p, i) => new { Content = p, Index = i }))
                {
                    var name = $"MainTutorialsUtilityTest1_OutputObject_{output.caseId:D4}_{target.Index}";
                    GameObject obj = new GameObject(name);
                    obj.transform.parent = transform;

                    switch (target.Index)
                    {
                        case 0:
                            output.target[target.Index] = obj.AddComponent<PentagramSystemModel>();

                            break;
                        case 1:
                            output.target[target.Index] = obj.AddComponent<ShikigamiSkillSystemModel>();

                            break;
                        case 2:
                            output.target[target.Index] = obj.AddComponent<DanceTurretModel>();

                            break;
                        case 3:
                            output.target[target.Index] = obj.AddComponent<GraffitiTurretModel>();

                            break;
                        case 4:
                            output.target[target.Index] = obj.AddComponent<OnmyoTurretModel>();

                            break;
                        case 5:
                            output.target[target.Index] = obj.AddComponent<WrapTurretModel>();

                            break;
                        case 6:
                            output.target[target.Index] = obj.AddComponent<SunMoonSystemModel>();

                            break;
                        case 7:
                            output.target[target.Index] = obj.AddComponent<ClearCountdownTimerSystemModel>();

                            break;
                        case 8:
                            output.target[target.Index] = obj.AddComponent<PauseView>();

                            break;
                        case 9:
                            output.target[target.Index] = obj.AddComponent<FadersGroupView>();

                            break;
                        case 10:
                            output.target[target.Index] = obj.AddComponent<GuideMessageView>();

                            break;
                        case 11:
                            output.target[target.Index] = obj.AddComponent<GuideUITheTurntableView>();

                            break;
                        case 12:
                            output.target[target.Index] = obj.AddComponent<GuideUITheEqualizerView>();

                            break;
                        case 13:
                            output.target[target.Index] = obj.AddComponent<GuideUITheEqualizerGageView>();

                            break;
                        case 14:
                            output.target[target.Index] = obj.AddComponent<GuideUITheDJEnergyView>();

                            break;
                        case 15:
                            output.target[target.Index] = obj.AddComponent<GuideUIThePlayerHPView>();

                            break;
                        case 16:
                            output.target[target.Index] = obj.AddComponent<GuideUITheClearCountdownTimerCircleView>();

                            break;
                        case 17:
                            output.target[target.Index] = obj.AddComponent<GuideUITheClearRewardTextContentsView>();

                            break;
                        case 18:
                            output.target[target.Index] = obj.AddComponent<MissionsSystemTutorialModel>();

                            break;
                    }

                }
                switch (output.caseId)
                {
                    case 0:
                        foreach (var target in output.target)
                            target.gameObject.SetActive(false);
                        output.target[18].gameObject.SetActive(true);

                        break;
                    case 1:
                        foreach (var target in output.target)
                            target.gameObject.SetActive(false);
                        output.target[0].gameObject.SetActive(true);
                        output.target[18].gameObject.SetActive(true);

                        break;
                    case 2:
                        break;
                }
            }
        }

        private MainPresenterDemo.TutorialComponentMap[] SetMainPresenterDemoTutorialComponentMaps()
        {
            return new MainPresenterDemo.TutorialComponentMap[]
            {
                new MainPresenterDemo.TutorialComponentMap()
                {
                    guideMessageID = GuideMessageID.GM0000,
                    tutorialComponents = new MainPresenterDemo.TutorialComponent[18],
                },
                new MainPresenterDemo.TutorialComponentMap()
                {
                    guideMessageID = GuideMessageID.GM0001,
                    tutorialComponents = new MainPresenterDemo.TutorialComponent[18],
                },
            };
        }
    }
}
