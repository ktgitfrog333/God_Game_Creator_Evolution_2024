using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Model;
using UnityEngine;
using Universal.Bean;

namespace Main.Test.Stub
{
    public class ShikigamiParameterUtilityTest4 : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public UserBean.ShikigamiInfo.MainSkill[] mainSkills;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public string throwMessage;
            public ShikigamiInfo.Prop.MainSkill[] mainSkills;
        }
    }
}
