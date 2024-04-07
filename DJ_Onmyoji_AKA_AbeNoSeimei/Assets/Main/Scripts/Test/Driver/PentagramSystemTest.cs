using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Model;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class PentagramSystemTest : MonoBehaviour
    {
        [SerializeField] private PentagramSystemTestStub pentagramSystemTestStub;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            pentagramSystemTestStub = ((IPentagramSystemTest)GameObject.Find("PentagramSystem").GetComponent<PentagramSystemModel>()).GetBackSpinDriver();
        }
    }

    public interface IPentagramSystemTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputBackSpinState"></param>
        /// <param name="inputHistory"></param>
        /// <param name="angleSum"></param>
        /// <param name="jockeyCommandType"></param>
        /// <returns></returns>
        /// <see cref="Main.Utility.JockeyCommandUtility.SetBackSpin(InputBackSpinState, IReactiveProperty{int})"/>
        /// <see cref="Main.Model.PentagramSystemModel._jockeyCommandUtility"/>
        public PentagramSystemTestStub GetBackSpinDriver();
    }

    [System.Serializable]
    public struct PentagramSystemTestStub
    {
        /// <summary>入力座標</summary>
        public Vector2 inputVelocityValue;
        /// <summary>入力保持時間（秒）</summary>
        public float recordInputTimeSec;
        /// <summary>入力保持最大時間（秒）</summary>
        public float recordInputTimeSecLimit;
        /// <summary>入力検知の角度</summary>
        public float targetAngle;
        public float angleSum;
        public JockeyCommandType jockeyCommandType;
        public List<Vector2> inputHistory;
    }
}
