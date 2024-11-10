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
    public class MainTutorialsUtilityTest2 : CommonUtilityTest
    {
        protected override void Start()
        {
            // 正常系
            CaseNormalSystem(0000);
        }

        protected override void Case(int caseId, bool isAbnormal = false)
        {
            var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var utility = new MainTutorialsUtility();
            MethodInfo methodInfo = typeof(MainTutorialsUtility).GetMethod(
                "AddTutorialComponents",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var input = GetComponent<Stub.MainTutorialsUtilityTest2>().Inputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            var output = GetComponent<Stub.MainTutorialsUtilityTest2>().Outputs
                .Where(q => q.caseId == caseId)
                .Select(q => q)
                .ToArray()[0];
            if (!isAbnormal)
            {
                // メソッドを呼び出し
                bool isSuccessed = (bool)methodInfo.Invoke(utility, new object[] { input.tutorialComponentMap, input.pentagramTurnTableModel, input.index, input.tutorialComponentMaps, input.componentState });
                OutputResult(isSuccessed.Equals(output.isSuccessed), caseId);

                bool isSuccessed1 = false;
                foreach (var tutorialComponentMap in input.tutorialComponentMaps.Select((p, i) => new { Content = p, Index = i }))
                {
                    isSuccessed1 = tutorialComponentMap.Content.guideMessageID.Equals(output.tutorialComponentMaps[tutorialComponentMap.Index].guideMessageID);
                    foreach (var tutorialComponent in tutorialComponentMap.Content.tutorialComponents.Select((p, i) => new { Content = p, Index = i }))
                    {
                        isSuccessed1 = tutorialComponent.Content.component.GetType().Name.Equals(output.tutorialComponentMaps[tutorialComponentMap.Index].tutorialComponents[tutorialComponent.Index].component.GetType().Name);
                        isSuccessed1 = tutorialComponent.Content.componentState.Equals(output.tutorialComponentMaps[tutorialComponentMap.Index].tutorialComponents[tutorialComponent.Index].componentState);
                    }
                }
                OutputResult(isSuccessed1, caseId);
            }
            else
            {
                ExceptionResult(() => methodInfo.Invoke(utility, new object[] { input.tutorialComponentMap, input.pentagramTurnTableModel, input.index, input.tutorialComponentMaps, input.componentState }), caseId, output.throwMessage);
            }
        }
    }
}
