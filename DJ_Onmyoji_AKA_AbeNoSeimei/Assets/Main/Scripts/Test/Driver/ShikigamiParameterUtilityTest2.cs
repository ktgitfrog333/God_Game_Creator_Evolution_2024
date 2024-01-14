using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Test.Common;
using Main.Utility;
using UnityEngine;
using Universal.Bean;
using Universal.Common;

namespace Main.Test.Driver
{
    public class ShikigamiParameterUtilityTest2 : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
            // 異常系
            CaseAbnormalSystem(1000);
            CaseAbnormalSystem(1001);
            // 境界値分析
            // 複数パターン
            CaseMultiplePatterns(3000);
        }

        protected override void Case(int caseId, bool isAbnormal=false)
        {
            var utility = new ShikigamiParameterUtility();
            var input = GetComponent<Stub.ShikigamiParameterUtilityTest2>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.ShikigamiParameterUtilityTest2>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
                OutputResult(utility.GetShikigamiInfo(input.pentagramTurnTableInfo, input.instanceId).prop.type.Equals(output.shikigamiInfo.prop.type), caseId);
            else
                ExceptionResult(() => utility.GetShikigamiInfo(input.pentagramTurnTableInfo, input.instanceId), caseId, output.throwMessage);
        }
    }
}
