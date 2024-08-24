using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using Universal.Bean;

namespace Main.Test.Stub
{
    public class ShikigamiParameterUtilityTest1 : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public LevelDesign levelDesign;
            public ShikigamiInfo shikigamiInfo;
            public MainSkillType mainSkillType;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public float value;
            public string throwMessage;
        }
    }
}
