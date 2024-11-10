using Main.Model;
using Main.Presenter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Test.Stub
{
    public class MainTutorialsUtilityTest : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public MainPresenterDemo.TutorialComponent tutorialComponent;
            public Component[] target;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public bool result;
            public Component[] target;
            public string throwMessage;
        }

        private void Reset()
        {
            // PentagramSystemModel
            /*
             * パターン[0000]：
             *  PentagramSystemModel：有効⇒無効
             * パターン[0001]：
             *  PentagramSystemModel：無効⇒有効
             * パターン[0002]：
             *  PentagramSystemModel：有効⇒停止（例外）
             */
            int[] patterns = new int[]
            {
                0,
                1,
                2,
            };
            inputs = new Input[patterns.Length];
            foreach (var pattern in patterns.Select((p, i) => new { Content = p, Index = i }))
            {
                switch (pattern.Content)
                {
                    case 0:
                        Initialize(pattern.Index, pattern.Content, inputs);
                        inputs[0].target[0].gameObject.SetActive(true);
                        inputs[0].tutorialComponent.component = inputs[0].target[0].GetComponent<PentagramSystemModel>();
                        inputs[0].tutorialComponent.componentState = MainPresenterDemo.ComponentState.Disable;

                        break;
                    case 1:
                        Initialize(pattern.Index, pattern.Content, inputs);
                        inputs[1].target[0].gameObject.SetActive(false);
                        inputs[1].tutorialComponent.component = inputs[0].target[0].GetComponent<PentagramSystemModel>();
                        inputs[1].tutorialComponent.componentState = MainPresenterDemo.ComponentState.Enable;

                        break;
                    case 2:
                        Initialize(pattern.Index, pattern.Content, inputs);
                        inputs[2].target[0].gameObject.SetActive(true);
                        inputs[2].tutorialComponent.component = inputs[0].target[0].GetComponent<PentagramSystemModel>();
                        inputs[2].tutorialComponent.componentState = MainPresenterDemo.ComponentState.Pause;

                        break;
                }
            }
            outputs = new Output[patterns.Length];
            foreach (var pattern in patterns.Select((p, i) => new { Content = p, Index = i }))
            {
                switch (pattern.Content)
                {
                    case 0:
                        Initialize(pattern.Index, pattern.Content, outputs);
                        outputs[0].target[0].gameObject.SetActive(false);
                        outputs[0].result = true;

                        break;
                    case 1:
                        Initialize(pattern.Index, pattern.Content, outputs);
                        outputs[1].target[0].gameObject.SetActive(true);
                        outputs[1].result = true;

                        break;
                    case 2:
                        Initialize(pattern.Index, pattern.Content, outputs);
                        outputs[2].throwMessage = "必要に応じて処理を追加する: [Main.Model.PentagramSystemModel]";
                        outputs[2].result = false;

                        break;
                }
            }
        }

        private void Initialize(int index, int content, Input[] inputs)
        {
            List<GameObject> objs = new List<GameObject>();
            for (var i = 0; i < 1; i++)
            {
                var name = $"MainTutorialsUtilityTest_InputObject_{content}_{i}";
                GameObject obj = new GameObject(name);
                obj.transform.parent = transform;
                Component component = null;
                switch (i)
                {
                    case 0:
                        component = obj.AddComponent<PentagramSystemModel>();

                        break;
                }
                objs.Add(obj);
            }
            inputs[index] = new Input
            {
                caseId = content,
                tutorialComponent = new MainPresenterDemo.TutorialComponent()
                {
                    component = objs[0].GetComponent<PentagramSystemModel>(),
                    componentState = MainPresenterDemo.ComponentState.Disable,
                },
                target = new Component[]
                {
                    objs[0].GetComponent<PentagramSystemModel>(),
                }
            };
        }

        private void Initialize(int index, int content, Output[] outputs)
        {
            List<GameObject> objs = new List<GameObject>();
            for (var i = 0; i < 1; i++)
            {
                var name = $"MainTutorialsUtilityTest_OutputObject_{content}_{i}";
                GameObject obj = new GameObject(name);
                obj.transform.parent = transform;
                Component component = null;
                switch (i)
                {
                    case 0:
                        component = obj.AddComponent<PentagramSystemModel>();

                        break;
                }
                objs.Add(obj);
            }
            outputs[index] = new Output
            {
                caseId = content,
                target = new Component[]
                {
                    objs[0].GetComponent<PentagramSystemModel>(),
                }
            };
        }
    }
}
