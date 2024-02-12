using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;

namespace Main.Test.Stub
{
    public class SpawnUtilityTest : MonoBehaviour
    {
        [SerializeField] private Input[] inputs;
        public Input[] Inputs => inputs;
        [SerializeField] private Output[] outputs;
        public Output[] Outputs => outputs;

        [System.Serializable]
        public struct Input
        {
            public int caseId;
            public EnemiesID[] enemiesIDs;
        }

        [System.Serializable]
        public struct Output
        {
            public int caseId;
            public EnemiesID enemiesID;
            public string throwMessage;
        }
    }
}
