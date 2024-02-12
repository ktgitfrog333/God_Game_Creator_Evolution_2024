using System.Collections;
using System.Collections.Generic;
using Main.Model;
using Main.Test.Common;
using Main.Utility;
using UnityEngine;
using System.Linq;

namespace Main.Test.Driver
{
    public class SpawnUtilityTest : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
            // 異常系
            CaseAbnormalSystem(1000);
            // 境界値
            CaseBoundaryValueAnalysis(2000);
        }

        protected override void Case(int caseId, bool isAbnormal = false)
        {
            var utility = new SpawnUtility();
            var input = GetComponent<Stub.SpawnUtilityTest>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.SpawnUtilityTest>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
            {
                Debug.Log($"{caseId}[{((ISpawnUtilityTest)utility).GetRandomEnemiesID(input.enemiesIDs)}]");
                OutputResult(((ISpawnUtilityTest)utility).GetRandomEnemiesID(input.enemiesIDs).Equals(output.enemiesID), caseId);
            }
            else
                ExceptionResult(() => ((ISpawnUtilityTest)utility).GetRandomEnemiesID(input.enemiesIDs), caseId, output.throwMessage);
        }
    }

    /// <see cref="Main.Utility.SpawnUtility"/>
    public interface ISpawnUtilityTest
    {
        /// <see cref="Main.Utility.SpawnUtility.GetRandomEnemiesID(EnemiesID[])"/>
        public EnemiesID GetRandomEnemiesID(EnemiesID[] enemiesIDs);
    }
}
