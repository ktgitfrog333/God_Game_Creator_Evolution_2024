using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.InputSystem;
using UnityEngine;

namespace Main.Test.Driver
{
    public class InputHistroyTest : MonoBehaviour
    {
        // [SerializeField] private InputHistroy inputHistroy;
        // private void Reset()
        // {
        //     inputHistroy = GetComponent<InputHistroy>();
        // }

        // private void Start()
        // {
        //     // 正常系
        //     Case_0000();
        //     Case_0001();
        //     Case_0002();
        //     Case_0003();
        //     // 異常系
        //     Case_1000();
        //     Case_1001();
        //     Case_1002();
        //     Case_1003();
        //     Case_1004();
        //     Case_1005();
        //     // 境界値分析
        //     Case_2000();
        //     Case_2001();
        //     Case_2002();
        //     Case_2003();
        //     Case_2004();
        //     Case_2005();
        //     Case_2006();
        //     Case_2007();
        //     Case_2008();
        //     Case_2009();
        //     Case_2010();
        //     Case_2011();
        //     Case_2012();
        //     // 複数パターン
        //     Case_3000();
        //     Case_3001();
        //     Case_3002();
        //     Case_3003();
        //     Case_3004();
        //     Case_3005();
        //     Case_3006();
        //     Case_3007();
        //     Case_3008();
        //     Case_3009();
        //     Case_3010();
        //     Case_3011();
        //     Case_3012();
        // }

        // private void OutputResult(bool assert, string name)
        // {
        //     if (assert)
        //         Debug.Log($"{name}:Success");
        //     else
        //         Debug.LogError($"{name}:Faild");
        // }

        // private void Case_0000()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.None,
        //         targetPattern = new InputType[]{ InputType.None },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.None,
        //         Time = 0f,
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.None), name);
        // }

        // private void Case_0001()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0001,
        //         targetPattern = new InputType[]{
        //             InputType.ChargeSun,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.ChargeSun,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.IT0001), name);
        // }

        // private void Case_0002()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 2).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 2).Select(q => q).ToArray()[0];
        //     MainGameManager.Instance.InputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.GetCurrentInputDriver(MainGameManager.Instance.InputSystemsOwner).Equals(output.inputType), name);
        // }

        // private void Case_0003()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3).Select(q => q).ToArray()[0];
        //     InputSystemsOwner inputSystemsOwner = GetComponent<InputSystemsOwner>();
        //     inputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.UpdateInputHistroyDriver(input.resultPatternStates, input.inputHistory.ToList(), input.cmdAcceptanceTimeMs, inputSystemsOwner)
        //         .Select(q => q.Input)
        //         .ToArray()
        //         .SequenceEqual(output.inputHistory
        //             .Select(q => q.Input)
        //             .ToArray()) &&
        //     inputHistroy.InputTypeID.Equals(output.inputTypeID), name);
        //     Debug.Log($"{name}:{string.Join(",", output.inputHistory.Select(q => q.Time).ToArray())}");
        // }

        // private void Case_1000()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     try
        //     {
        //         ResultPatternState resultPatternState = new ResultPatternState()
        //         {

        //         };
        //         ResultPatternState[] resultPatternStates = null;
        //         InputRecord inputRecord = new InputRecord
        //         {
        //             Input = InputType.None,
        //             Time = 0f,
        //         };
        //         List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //         if (inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.None))
        //             Debug.Log($"{name}:Faild");
        //         else
        //             Debug.Log($"{name}:Faild");
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"{name}:Success");
        //     }
        // }

        // private void Case_1001()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     try
        //     {
        //         ResultPatternState resultPatternState = new ResultPatternState()
        //         {
        //             inputTypeID = InputTypeID.None,
        //             targetPattern = new InputType[]{ InputType.None },
        //         };
        //         ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //         InputRecord inputRecord = new InputRecord
        //         {

        //         };
        //         List<InputRecord> inputHistory = null;
        //         if (inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.None))
        //             Debug.Log($"{name}:Faild");
        //         else
        //             Debug.Log($"{name}:Faild");
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"{name}:Success");
        //     }
        // }
        // private void Case_1002()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     try
        //     {
        //         inputHistroy.GetCurrentInputDriver(null);
        //         Debug.LogError($"{name}:Faild");
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"{name}:Success");
        //     }
        // }

        // private void Case_1003()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     try
        //     {
        //         Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //         var input = inputHistroyTest.Inputs[0];
        //         var output = inputHistroyTest.Outputs[0];
        //         InputSystemsOwner inputSystemsOwner = GetComponent<InputSystemsOwner>();
        //         inputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //         input.resultPatternStates = null;
        //         var inputHistory = inputHistroy.UpdateInputHistroyDriver(input.resultPatternStates, input.inputHistory.ToList(), input.cmdAcceptanceTimeMs, inputSystemsOwner);
        //         if (inputHistory == null)
        //             throw new System.Exception("UpdateInputHistroy");

        //         Debug.LogError($"{name}:Faild");
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"{name}:Success");
        //     }
        // }

        // private void Case_1004()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     try
        //     {
        //         Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //         var input = inputHistroyTest.Inputs[0];
        //         var output = inputHistroyTest.Outputs[0];
        //         InputSystemsOwner inputSystemsOwner = GetComponent<InputSystemsOwner>();
        //         inputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //         var inputHistory = inputHistroy.UpdateInputHistroyDriver(input.resultPatternStates, input.inputHistory.ToList(), input.cmdAcceptanceTimeMs, null);
        //         if (inputHistory == null)
        //             throw new System.Exception("UpdateInputHistroy");

        //         Debug.LogError($"{name}:Faild");
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"{name}:Success");
        //     }
        // }

        // private void Case_1005()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     try
        //     {
        //         Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //         var input = inputHistroyTest.Inputs[0];
        //         var output = inputHistroyTest.Outputs[0];
        //         InputSystemsOwner inputSystemsOwner = GetComponent<InputSystemsOwner>();
        //         inputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //         var inputHistory = inputHistroy.UpdateInputHistroyDriver(input.resultPatternStates, null, input.cmdAcceptanceTimeMs, inputSystemsOwner);
        //         if (inputHistory == null)
        //             throw new System.Exception("UpdateInputHistroy");

        //         Debug.LogError($"{name}:Faild");
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log($"{name}:Success");
        //     }
        // }

        // private void Case_2000()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0029,
        //         targetPattern = new InputType[]{
        //             InputType.ChargeSun,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.ChargeSun,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.IT0029), name);
        // }

        // private void Case_2001()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0030,
        //         targetPattern = new InputType[]{
        //             InputType.ChargeSun,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.ChargeSun,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.IT0030), name);
        // }

        // private void Case_2002()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0001,
        //         targetPattern = new InputType[]{
        //             InputType.None,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.None,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.IT0001), name);
        // }

        // private void Case_2003()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0001,
        //         targetPattern = new InputType[]{
        //             InputType.ChargeSun,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.None,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.None), name);
        // }

        // private void Case_2004()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0001,
        //         targetPattern = new InputType[]{
        //             InputType.ChargeMoon,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.None,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.None), name);
        // }

        // private void Case_2005()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0001,
        //         targetPattern = new InputType[]{
        //             InputType.None,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.ChargeSun,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.None), name);
        // }

        // private void Case_2006()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     ResultPatternState resultPatternState = new ResultPatternState()
        //     {
        //         inputTypeID = InputTypeID.IT0001,
        //         targetPattern = new InputType[]{
        //             InputType.None,
        //         },
        //     };
        //     ResultPatternState[] resultPatternStates = new ResultPatternState[]{ resultPatternState };
        //     InputRecord inputRecord = new InputRecord
        //     {
        //         Input = InputType.ChargeMoon,
        //         Time = 0f
        //     };
        //     List<InputRecord> inputHistory = new List<InputRecord>{ inputRecord };
        //     OutputResult(inputHistroy.CheckInputPatternDriver(resultPatternStates, inputHistory).Equals(InputTypeID.None), name);
        // }

        // private void Case_2007()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 2007).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 2007).Select(q => q).ToArray()[0];
        //     MainGameManager.Instance.InputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.GetCurrentInputDriver(MainGameManager.Instance.InputSystemsOwner).Equals(output.inputType), name);
        // }

        // private void Case_2008()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 2008).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 2008).Select(q => q).ToArray()[0];
        //     MainGameManager.Instance.InputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.GetCurrentInputDriver(MainGameManager.Instance.InputSystemsOwner).Equals(output.inputType), name);
        // }

        // private void Case_2009()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 2009).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 2009).Select(q => q).ToArray()[0];
        //     MainGameManager.Instance.InputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.GetCurrentInputDriver(MainGameManager.Instance.InputSystemsOwner).Equals(output.inputType), name);
        // }

        // private void Case_2010()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 2010).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 2010).Select(q => q).ToArray()[0];
        //     InputSystemsOwner inputSystemsOwner = GetComponent<InputSystemsOwner>();
        //     inputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.UpdateInputHistroyDriver(input.resultPatternStates, input.inputHistory.ToList(), input.cmdAcceptanceTimeMs, inputSystemsOwner)
        //         .Count
        //         == output.inputHistory
        //             .Count &&
        //     inputHistroy.InputTypeID.Equals(output.inputTypeID), name);
        //     Debug.Log($"{name}:{string.Join(",", output.inputHistory.Select(q => q.Time).ToArray())}");
        // }

        // private void Case_2011()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 2011).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 2011).Select(q => q).ToArray()[0];
        //     InputSystemsOwner inputSystemsOwner = GetComponent<InputSystemsOwner>();
        //     inputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.UpdateInputHistroyDriver(input.resultPatternStates, input.inputHistory.ToList(), input.cmdAcceptanceTimeMs, inputSystemsOwner)
        //         .Select(q => q.Input)
        //         .ToArray()
        //         .SequenceEqual(output.inputHistory
        //             .Select(q => q.Input)
        //             .ToArray()) &&
        //     inputHistroy.InputTypeID.Equals(output.inputTypeID), name);
        //     Debug.Log($"{name}:{string.Join(",", output.inputHistory.Select(q => q.Time).ToArray())}");
        // }

        // private void Case_2012()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 2012).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 2012).Select(q => q).ToArray()[0];
        //     InputSystemsOwner inputSystemsOwner = GetComponent<InputSystemsOwner>();
        //     inputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.UpdateInputHistroyDriver(input.resultPatternStates, input.inputHistory.ToList(), input.cmdAcceptanceTimeMs, inputSystemsOwner)
        //         .Select(q => q.Input)
        //         .ToArray()
        //         .SequenceEqual(output.inputHistory
        //             .Select(q => q.Input)
        //             .ToArray()) &&
        //     inputHistroy.InputTypeID.Equals(output.inputTypeID), name);
        //     Debug.Log($"{name}:{string.Join(",", output.inputHistory.Select(q => q.Time).ToArray())}");
        // }

        // private void Case_3000()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3000).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3000).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3001()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3001).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3001).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3002()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3002).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3002).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3003()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3003).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3003).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3004()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3004).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3004).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3005()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3005).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3005).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3006()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3006).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3006).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3007()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3007).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3007).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3008()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3008).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3008).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3009()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3009).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3009).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3010()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3010).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3010).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3011()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3011).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3011).Select(q => q).ToArray()[0];
        //     OutputResult(inputHistroy.CheckInputPatternDriver(input.resultPatternStates, input.inputHistory.ToList()).Equals(output.inputTypeID), name);
        // }

        // private void Case_3012()
        // {
        //     var name = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //     Stub.InputHistroyTest inputHistroyTest = GetComponent<Stub.InputHistroyTest>();
        //     var input = inputHistroyTest.Inputs.Where(q => q.Case_ == 3012).Select(q => q).ToArray()[0];
        //     var output = inputHistroyTest.Outputs.Where(q => q.Case_ == 3012).Select(q => q).ToArray()[0];
        //     MainGameManager.Instance.InputSystemsOwner.InputUI.SetInputUIs(input.isChargeSun, input.isChargeMoon, input.isScratch);
        //     OutputResult(inputHistroy.GetCurrentInputDriver(MainGameManager.Instance.InputSystemsOwner).Equals(output.inputType), name);
        // }
    }

    /// <summary>
    /// テスト用のインタフェース
    /// </summary>
    /// <see cref="Main.InputSystem.InputHistroy"/>
    /// <see cref="Main.InputSystem.InputUI"/>
    public interface IInputHistroyTest
    {
        public InputTypeID CheckInputPatternDriver(ResultPatternState[] resultPatternState, List<InputRecord> inputHistory);
        public InputType GetCurrentInputDriver(InputSystemsOwner inputSystemsOwner);
        public void SetInputUIs(bool isChargeSun, bool isChargeMoon, Vector2 isScratch);
        public List<InputRecord> UpdateInputHistroyDriver(ResultPatternState[] resultPatternState, List<InputRecord> inputHistory, float cmdAcceptanceTimeMs, InputSystemsOwner inputSystemsOwner);
    }
}
