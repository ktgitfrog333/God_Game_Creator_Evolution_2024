using Main.Presenter;
using Main.Test.Common;
using Main.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Main.Test.Driver
{
    public class MainTutorialsUtilityTest : CommonUtilityTest
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
            // MainTutorialsUtilityクラスの非公開メソッドCheckComponentStateAndSetActiveを取得
            MethodInfo methodInfo = typeof(MainTutorialsUtility).GetMethod(
                "CheckComponentStateAndSetActive",
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new System.Type[] { typeof(MainPresenterDemo.TutorialComponent), typeof(Component[]) }, // Replace with actual parameter types
                null);
            var input = GetComponent<Stub.MainTutorialsUtilityTest>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.MainTutorialsUtilityTest>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
            {
                // メソッドを呼び出し
                bool result = (bool)methodInfo.Invoke(utility, new object[] { input.tutorialComponent, input.target });
                OutputResult(result.Equals(output.result), caseId);
                
                bool result1 = false;
                foreach (var target in input.target.Select((p, i) => new { Content = p, Index = i }))
                {
                    result1 = target.Content.GetType().Name.Equals(output.target[target.Index].GetType().Name);
                }
                OutputResult(result1, caseId);

                bool result2 = false;
                foreach (var target in input.target.Select((p, i) => new { Content = p, Index = i }))
                {
                    result2 = target.Content.gameObject.activeSelf.Equals(output.target[target.Index].gameObject.activeSelf);
                }
                OutputResult(result2, caseId);
            }
            else
            {
                ExceptionResult(() => methodInfo.Invoke(utility, new object[] { input.tutorialComponent, input.target }), caseId, output.throwMessage);
            }
        }
    }
}
