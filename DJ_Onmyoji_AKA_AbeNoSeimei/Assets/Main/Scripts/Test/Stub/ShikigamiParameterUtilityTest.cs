using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;

namespace Main.Test.Stub
{
    public class ShikigamiParameterUtilityTest : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public ShikigamiInfo shikigamiInfo;
            public MainSkillType mainSkillType;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public SkillRank skillRank;
            public string throwMessage;
        }
    }
}
