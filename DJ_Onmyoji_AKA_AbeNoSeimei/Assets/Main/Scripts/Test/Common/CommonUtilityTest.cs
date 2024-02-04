using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Test.Common
{
    public abstract class CommonUtilityTest : MonoBehaviour
    {
        protected virtual void Start() {}
        protected void OutputResult(bool assert, int caseId)
        {
            if (assert)
                Debug.Log($"{caseId:D4}:Success");
            else
                Debug.LogError($"{caseId:D4}:Faild");
        }

        protected void ExceptionResult<T>(System.Func<T> function, int caseId, string throwMessage)
        {
            try
            {
                function();
                Debug.LogError($"{caseId:D4}:Faild");
            }
            catch (System.Exception e)
            {
                OutputResult(0 < throwMessage.Length && (e.Message.StartsWith(throwMessage) || e.Message.EndsWith(throwMessage)), caseId);
            }
        }

        protected abstract void Case(int caseId, bool isAbnormal=false);
        public void CaseNormalSystem(int caseId)
        {
            Case(caseId);
        }

        protected void CaseAbnormalSystem(int caseId)
        {
            Case(caseId, true);
        }

        protected void CaseBoundaryValueAnalysis(int caseId)
        {
            Case(caseId);
        }

        protected void CaseMultiplePatterns(int caseId)
        {
            Case(caseId);
        }
    }
}
