using Main.Common;
using Main.Presenter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Model;
using System.Linq;

namespace Main.Test.Stub
{
    public class MainTutorialsUtilityTest2 : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public MainPresenterDemo.TutorialComponentMap tutorialComponentMap;
            public PentagramTurnTableModel pentagramTurnTableModel;
            public int index;
            public MainPresenterDemo.TutorialComponentMap[] tutorialComponentMaps;
            public MainPresenterDemo.ComponentState componentState;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public bool isSuccessed;
            public MainPresenterDemo.TutorialComponentMap[] tutorialComponentMaps;
            public string throwMessage;
        }

        private void Reset()
        {
            /* 
             * 対象のコンポーネント
             *  PentagramSystemModel
             * 
             * パターン[0000]：
             *  デフォルトのPause
             * パターン[0001]：
             *  全て有効
             * パターン[0002]：
             *  全て無効
             */

            inputs = new Input[]
            {
                new Input()
                {
                    caseId = 0,
                    tutorialComponentMap = new MainPresenterDemo.TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0000,
                        tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                        {
                            new MainPresenterDemo.TutorialComponent()
                            {
                                component = null,
                                componentState = MainPresenterDemo.ComponentState.Pause
                            }
                        }
                    },
                    index = 0,
                    tutorialComponentMaps = new MainPresenterDemo.TutorialComponentMap[]
                    {
                        new MainPresenterDemo.TutorialComponentMap()
                        {
                            guideMessageID = GuideMessageID.GM0000,
                            tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                            {
                                new MainPresenterDemo.TutorialComponent()
                                {
                                    component = null,
                                    componentState = MainPresenterDemo.ComponentState.Pause
                                }
                            }
                        },
                    },
                    componentState = MainPresenterDemo.ComponentState.Pause
                },
                new Input()
                {
                    caseId = 1,
                    tutorialComponentMap = new MainPresenterDemo.TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0000,
                        tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                        {
                            new MainPresenterDemo.TutorialComponent()
                            {
                                component = null,
                                componentState = MainPresenterDemo.ComponentState.Pause
                            }
                        }
                    },
                    index = 0,
                    tutorialComponentMaps = new MainPresenterDemo.TutorialComponentMap[]
                    {
                        new MainPresenterDemo.TutorialComponentMap()
                        {
                            guideMessageID = GuideMessageID.GM0000,
                            tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                            {
                                new MainPresenterDemo.TutorialComponent()
                                {
                                    component = null,
                                    componentState = MainPresenterDemo.ComponentState.Pause
                                }
                            }
                        },
                    },
                    componentState = MainPresenterDemo.ComponentState.Enable
                },
                new Input()
                {
                    caseId = 2,
                    tutorialComponentMap = new MainPresenterDemo.TutorialComponentMap()
                    {
                        guideMessageID = GuideMessageID.GM0000,
                        tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                        {
                            new MainPresenterDemo.TutorialComponent()
                            {
                                component = null,
                                componentState = MainPresenterDemo.ComponentState.Pause
                            }
                        }
                    },
                    index = 0,
                    tutorialComponentMaps = new MainPresenterDemo.TutorialComponentMap[]
                    {
                        new MainPresenterDemo.TutorialComponentMap()
                        {
                            guideMessageID = GuideMessageID.GM0000,
                            tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                            {
                                new MainPresenterDemo.TutorialComponent()
                                {
                                    component = null,
                                    componentState = MainPresenterDemo.ComponentState.Pause
                                }
                            }
                        },
                    },
                    componentState = MainPresenterDemo.ComponentState.Disable
                },
            };
            foreach (var input in inputs.Select((p, i) => new { Content = p, Index = i }))
            {
                foreach (var tutorialComponentMap in input.Content.tutorialComponentMaps)
                {
                    foreach (var tutorialComponent in tutorialComponentMap.tutorialComponents.Select((p, i) => new { Content = p, Index = i }))
                    {
                        var name = $"MainTutorialsUtilityTest2_InputObject_{input.Content.caseId:D4}_{tutorialComponent.Index}";
                        GameObject obj = new GameObject(name);
                        obj.transform.parent = transform;

                        tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.AddComponent<PentagramSystemModel>();
                    }
                }
                foreach (var tutorialComponentMap in input.Content.tutorialComponentMaps)
                {
                    foreach (var tutorialComponent in tutorialComponentMap.tutorialComponents.Select((p, i) => new { Content = p, Index = i }))
                    {
                        var name = $"MainTutorialsUtilityTest2_InputObject_{input.Content.caseId:D4}_{tutorialComponent.Index}";
                        GameObject obj = GameObject.Find(name);

                        input.Content.tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<PentagramSystemModel>();
                    }
                }
                var pentagramTurnTableModelName = $"MainTutorialsUtilityTest2_InputObject_{input.Content.caseId:D4}_pentagramTurnTableModel";
                GameObject pentagramTurnTableModelObj = new GameObject(pentagramTurnTableModelName);
                pentagramTurnTableModelObj.transform.parent = transform;

                inputs[input.Index].pentagramTurnTableModel = pentagramTurnTableModelObj.AddComponent<PentagramTurnTableModel>();
            }
            outputs = new Output[]
            {
                new Output()
                {
                    caseId = 0,
                    isSuccessed = true,
                    tutorialComponentMaps = new MainPresenterDemo.TutorialComponentMap[]
                    {
                        new MainPresenterDemo.TutorialComponentMap()
                        {
                            guideMessageID = GuideMessageID.GM0000,
                            tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                            {
                                new MainPresenterDemo.TutorialComponent()
                                {
                                    component = null,
                                    componentState = MainPresenterDemo.ComponentState.Pause
                                }
                            }
                        },
                    }
                },
                new Output()
                {
                    caseId = 1,
                    isSuccessed = true,
                    tutorialComponentMaps = new MainPresenterDemo.TutorialComponentMap[]
                    {
                        new MainPresenterDemo.TutorialComponentMap()
                        {
                            guideMessageID = GuideMessageID.GM0000,
                            tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                            {
                                new MainPresenterDemo.TutorialComponent()
                                {
                                    component = null,
                                    componentState = MainPresenterDemo.ComponentState.Enable
                                }
                            }
                        },
                    }
                },
                new Output()
                {
                    caseId = 2,
                    isSuccessed = true,
                    tutorialComponentMaps = new MainPresenterDemo.TutorialComponentMap[]
                    {
                        new MainPresenterDemo.TutorialComponentMap()
                        {
                            guideMessageID = GuideMessageID.GM0000,
                            tutorialComponents = new MainPresenterDemo.TutorialComponent[]
                            {
                                new MainPresenterDemo.TutorialComponent()
                                {
                                    component = null,
                                    componentState = MainPresenterDemo.ComponentState.Disable
                                }
                            }
                        },
                    }
                },
            };
            foreach (var output in outputs)
            {
                foreach (var tutorialComponentMap in output.tutorialComponentMaps)
                {
                    foreach (var tutorialComponent in tutorialComponentMap.tutorialComponents.Select((p, i) => new { Content = p, Index = i }))
                    {
                        var name = $"MainTutorialsUtilityTest2_OutputObject_{output.caseId:D4}_{tutorialComponent.Index}";
                        GameObject obj = new GameObject(name);
                        obj.transform.parent = transform;

                        tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.AddComponent<PentagramSystemModel>();
                    }
                }
                foreach (var tutorialComponentMap in output.tutorialComponentMaps)
                {
                    foreach (var tutorialComponent in tutorialComponentMap.tutorialComponents.Select((p, i) => new { Content = p, Index = i }))
                    {
                        var name = $"MainTutorialsUtilityTest2_OutputObject_{output.caseId:D4}_{tutorialComponent.Index}";
                        GameObject obj = GameObject.Find(name);

                        tutorialComponentMap.tutorialComponents[tutorialComponent.Index].component = obj.GetComponent<PentagramSystemModel>();
                    }
                }
            }
        }
    }
}
