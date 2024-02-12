using System.Collections;
using System.Collections.Generic;
using Main.Model;
using Main.Test.Common;
using Main.Utility;
using UnityEngine;
using System.Linq;
using UniRx;

namespace Main.Test.Driver
{
    public class SpawnUtilityTest1 : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
            CaseNormalSystem(0001);
            CaseNormalSystem(0002);
            CaseNormalSystem(0003);
            CaseNormalSystem(0004);
            CaseNormalSystem(0005);
            CaseNormalSystem(0006);
            CaseNormalSystem(0007);
            CaseNormalSystem(0008);
            // 異常系
            CaseAbnormalSystem(1000);
            // 境界値
            CaseBoundaryValueAnalysis(2000);
            CaseBoundaryValueAnalysis(2001);
            CaseBoundaryValueAnalysis(2002);
            CaseBoundaryValueAnalysis(2003);
            CaseBoundaryValueAnalysis(2004);
            // 複数パターン
            CaseMultiplePatterns(3000);
        }

        protected override void Case(int caseId, bool isAbnormal = false)
        {
            var utility = new SpawnUtility();
            var input = GetComponent<Stub.SpawnUtilityTest1>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!input.isNullinstanceCountRemaining)
            {
                input.enemiesSpawnTable.instanceCountRemaining = new FloatReactiveProperty(input.instanceCountRemaining);
            }
            var output = GetComponent<Stub.SpawnUtilityTest1>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
            {
                OutputResult(((ISpawnUtilityTest1)utility).GetCalcMaxCountAndAddRemaining(input.enemiesSpawnTable, input.onmyoState) == output.resultMaxCount, caseId);
            }
            else
                ExceptionResult(() => ((ISpawnUtilityTest1)utility).GetCalcMaxCountAndAddRemaining(input.enemiesSpawnTable, input.onmyoState), caseId, output.throwMessage);
        }
    }

    /// <see cref="Main.Utility.SpawnUtility"/>
    public interface ISpawnUtilityTest1
    {
        /// <see cref="Main.Utility.SpawnUtility.GetCalcMaxCountAndAddRemaining(EnemiesSpawnTable, float)"/>
        public int GetCalcMaxCountAndAddRemaining(EnemiesSpawnTable enemiesSpawnTable, float onmyoState);
    }
}
