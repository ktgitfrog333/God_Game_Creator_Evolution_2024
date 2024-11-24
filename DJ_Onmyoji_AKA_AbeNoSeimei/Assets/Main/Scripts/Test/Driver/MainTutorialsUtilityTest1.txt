using Main.Test.Common;
using Main.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Test.Driver
{
    public class MainTutorialsUtilityTest1 : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
            CaseNormalSystem(0001);
            CaseNormalSystem(0002);
        }

        protected override void Case(int caseId, bool isAbnormal = false)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var utility = new MainTutorialsUtility();
            // MainTutorialsUtilityクラスの非公開メソッドCheckComponentStateAndSetActiveCommonを取得
            var input = GetComponent<Stub.MainTutorialsUtilityTest1>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.MainTutorialsUtilityTest1>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
            {
                // メソッドを呼び出し
                bool isSuccessed = utility.DoTutorialGuideContents(input.guideMessageID, input.tutorialGuideContentsStuct);
                OutputResult(isSuccessed.Equals(output.isSuccessed), caseId);

                bool isSuccessed1 = false;
                foreach (var target in input.tutorialGuideContentsStuct.targetComponents.Select((p, i) => new { Content = p, Index = i }))
                {
                    isSuccessed1 = target.Content.GetType().Name.Equals(output.target[target.Index].GetType().Name);
                }
                OutputResult(isSuccessed1, caseId);

                bool isSuccessed2 = false;
                foreach (var target in input.tutorialGuideContentsStuct.targetComponents.Select((p, i) => new { Content = p, Index = i }))
                {
                    isSuccessed2 = target.Content.gameObject.activeSelf.Equals(output.target[target.Index].gameObject.activeSelf);
                }
                OutputResult(isSuccessed2, caseId);
            }
            else
            {
                ExceptionResult(() => utility.DoTutorialGuideContents(input.guideMessageID, input.tutorialGuideContentsStuct), caseId, output.throwMessage);
            }
        }
    }
}
