using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;

namespace Main.Test.Stub
{
    public class SpawnUtilityTest1 : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public EnemiesSpawnTable enemiesSpawnTable;
            public float instanceCountRemaining;
            public bool isNullinstanceCountRemaining;
            public float onmyoState;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public int resultMaxCount;
            public float instanceCountRemaining;
            public string throwMessage;
        }
    }
}
