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
    public class ShikigamiParameterUtilityTest4 : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
            // 異常系
            CaseAbnormalSystem(1000);
            // 境界値分析
            // 複数パターン
            CaseMultiplePatterns(3000);
        }

        protected override void Case(int caseId, bool isAbnormal=false)
        {
            var utility = new ShikigamiParameterUtility();
            var input = GetComponent<Stub.ShikigamiParameterUtilityTest4>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.ShikigamiParameterUtilityTest4>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
                OutputResult(((IShikigamiParameterUtilityTest4)utility).ConvertMainSkills(input.mainSkills).SequenceEqual(output.mainSkills), caseId);
            else
                ExceptionResult(() => ((IShikigamiParameterUtilityTest4)utility).ConvertMainSkills(input.mainSkills), caseId, output.throwMessage);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="Main.Utility.ShikigamiParameterUtility"/>
    public interface IShikigamiParameterUtilityTest4
    {
        /// <see cref="Main.Utility.ShikigamiParameterUtility.ConvertMainSkills(UserBean.ShikigamiInfo.MainSkill[])"/>
        public ShikigamiInfo.Prop.MainSkill[] ConvertMainSkills(UserBean.ShikigamiInfo.MainSkill[] mainSkills);
    }
}
