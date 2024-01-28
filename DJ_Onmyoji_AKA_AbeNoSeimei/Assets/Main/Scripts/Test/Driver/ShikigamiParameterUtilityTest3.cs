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
    public class ShikigamiParameterUtilityTest3 : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
            // 異常系
            // 境界値分析
            CaseBoundaryValueAnalysis(2000);
            // 複数パターン
            CaseMultiplePatterns(3000);
        }

        protected override void Case(int caseId, bool isAbnormal=false)
        {
            var utility = new ShikigamiParameterUtility();
            var input = GetComponent<Stub.ShikigamiParameterUtilityTest3>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.ShikigamiParameterUtilityTest3>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
                OutputResult(((IShikigamiParameterUtilityTest3)utility).ConvertSubSkills(input.subSkills).SequenceEqual(output.subSkills), caseId);
            else
                ExceptionResult(() => ((IShikigamiParameterUtilityTest3)utility).ConvertSubSkills(input.subSkills), caseId, output.throwMessage);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="Main.Utility.ShikigamiParameterUtility"/>
    public interface IShikigamiParameterUtilityTest3
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subSkills"></param>
        /// <returns></returns>
        /// <see cref="Main.Utility.ShikigamiParameterUtility.ConvertSubSkills(UserBean.ShikigamiInfo.SubSkill[])"/>
        public ShikigamiInfo.Prop.SubSkill[] ConvertSubSkills(UserBean.ShikigamiInfo.SubSkill[] subSkills);
    }
}
