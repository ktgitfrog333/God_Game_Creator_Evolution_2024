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
    public class ShikigamiParameterUtilityTest5 : CommonUtilityTest
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
            // 複数パターン
            CaseMultiplePatterns(3000);
        }

        protected override void Case(int caseId, bool isAbnormal=false)
        {
            var utility = new ShikigamiParameterUtility();
            var input = GetComponent<Stub.ShikigamiParameterUtilityTest5>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.ShikigamiParameterUtilityTest5>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var utilityCommon = new MainCommonUtility();
            utilityCommon.UserDataSingleton.UserBean.pentagramTurnTableInfo.slots = input.slots;
            if (!isAbnormal)
                OutputResult(SequenceEqualAddOption(utility.GetPentagramTurnTableInfo().slots, output.pentagramTurnTableInfo.slots), caseId);
            else
                ExceptionResult(() => utility.GetPentagramTurnTableInfo(), caseId, output.throwMessage);
        }

        private bool SequenceEqualAddOption(PentagramTurnTableInfo.Slot[] slots, PentagramTurnTableInfo.Slot[] slots_2)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (!slots[i].slotId.Equals(slots_2[i].slotId) ||
                    !slots[i].shikigamiInfo.prop.type.Equals(slots_2[i].shikigamiInfo.prop.type) ||
                    !slots[i].shikigamiInfo.prop.level.Equals(slots_2[i].shikigamiInfo.prop.level) ||
                    !slots[i].shikigamiInfo.prop.mainSkills.SequenceEqual(slots_2[i].shikigamiInfo.prop.mainSkills) ||
                    !slots[i].shikigamiInfo.prop.subSkills.SequenceEqual(slots_2[i].shikigamiInfo.prop.subSkills) ||
                    slots[i].instanceId != slots_2[i].instanceId)
                    return false;
            }

            return true;
        }
    }
}
