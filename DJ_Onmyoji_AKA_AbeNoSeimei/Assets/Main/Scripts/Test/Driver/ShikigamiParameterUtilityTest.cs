using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Test.Common;
using Main.Utility;
using UnityEngine;

namespace Main.Test.Driver
{
    public class ShikigamiParameterUtilityTest : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
            // 異常系
            CaseAbnormalSystem(1000);
            CaseAbnormalSystem(1001);
            // 境界値分析
            CaseBoundaryValueAnalysis(2000);
            CaseBoundaryValueAnalysis(2001);
            CaseBoundaryValueAnalysis(2002);
            CaseBoundaryValueAnalysis(2003);
            CaseBoundaryValueAnalysis(2004);
            CaseBoundaryValueAnalysis(2005);
            CaseBoundaryValueAnalysis(2006);
            CaseBoundaryValueAnalysis(2007);
            // 複数パターン
            CaseMultiplePatterns(3000);
        }

        protected override void Case(int caseId, bool isAbnormal=false)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var utility = new ShikigamiParameterUtility();
            var input = GetComponent<Stub.ShikigamiParameterUtilityTest>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.ShikigamiParameterUtilityTest>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
                OutputResult(((IShikigamiParameterUtilityTest)utility).GetMainSkillRank(input.shikigamiInfo, input.mainSkillType).Equals(output.skillRank), caseId);
            else
                ExceptionResult(() => ((IShikigamiParameterUtilityTest)utility).GetMainSkillRank(input.shikigamiInfo, input.mainSkillType), caseId, output.throwMessage);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="ShikigamiParameterUtility"/>
    public interface IShikigamiParameterUtilityTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shikigamiInfo"></param>
        /// <param name="mainSkillType"></param>
        /// <returns></returns>
        /// <see cref="ShikigamiParameterUtility.GetMainSkillRank(ShikigamiInfo, MainSkillType)"/>
        public SkillRank GetMainSkillRank(ShikigamiInfo shikigamiInfo, MainSkillType mainSkillType);
    }
}
