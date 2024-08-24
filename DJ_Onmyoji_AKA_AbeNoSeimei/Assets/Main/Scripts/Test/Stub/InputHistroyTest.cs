using System.Collections;
using System.Collections.Generic;
using Main.InputSystem;
using UnityEngine;

namespace Main.Test.Stub
{
    public class InputHistroyTest : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int Case_;
            public ResultPatternState[] resultPatternStates;
            public InputRecord[] inputHistory;
            public bool isChargeSun;
            public bool isChargeMoon;
            public Vector2 isScratch;
            public float cmdAcceptanceTimeMs;
        }

        [System.Serializable]
        public struct Output
        {
            public int Case_;
            public InputTypeID inputTypeID;
            public InputType inputType;
            public List<InputRecord> inputHistory;
        }
    }
}
