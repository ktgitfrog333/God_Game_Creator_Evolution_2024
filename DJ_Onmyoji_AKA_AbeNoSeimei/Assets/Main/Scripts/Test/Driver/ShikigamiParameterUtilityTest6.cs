using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Test.Common;
using Main.Utility;
using UniRx;
using UnityEditor.Build.Content;
using UnityEngine;
using Universal.Bean;
using Universal.Common;

namespace Main.Test.Driver
{
    public class ShikigamiParameterUtilityTest6 : CommonUtilityTest
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
            var utility = new ShikigamiParameterUtility();
            var input = GetComponent<Stub.ShikigamiParameterUtilityTest6>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.ShikigamiParameterUtilityTest6>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            adminDataSingleton.AdminBean.levelDesign = input.levelDesign;
            input.shikigamiInfo.state.tempoLevel = new FloatReactiveProperty(input.tempoLevelValue);
            if (!isAbnormal)
            {
                Debug.Log($"[{((IShikigamiParameterUtilityTest6)utility).GetMainSkillValueBuffMax(input.shikigamiInfo, input.mainSkillType)}]_[{output.value}]");
                OutputResult(((IShikigamiParameterUtilityTest6)utility).GetMainSkillValueBuffMax(input.shikigamiInfo, input.mainSkillType).Equals(output.value), caseId);
            }
            else
                ExceptionResult(() => ((IShikigamiParameterUtilityTest6)utility).GetMainSkillValueBuffMax(input.shikigamiInfo, input.mainSkillType), caseId, output.throwMessage);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="Main.Utility.ShikigamiParameterUtility"/>
    public interface IShikigamiParameterUtilityTest6
    {
        /// <see cref="Main.Utility.ShikigamiParameterUtility.GetMainSkillValueBuffMax(ShikigamiInfo shikigamiInfo, MainSkillType mainSkillType)"/>
        public float GetMainSkillValueBuffMax(ShikigamiInfo shikigamiInfo, MainSkillType mainSkillType);
    }
}
