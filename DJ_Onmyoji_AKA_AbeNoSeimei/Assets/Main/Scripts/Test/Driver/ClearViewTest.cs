using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class ClearViewTest : MonoBehaviour
    {
        public IReactiveProperty<float> TimeSec { get; private set; } = new FloatReactiveProperty();
        [SerializeField] private float timeSec;
        /// <summary>魂のお金</summary>
        public IReactiveProperty<int> SoulMoney { get; private set; } = new IntReactiveProperty();
        [SerializeField] private int soulMoney;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            TimeSec.Value = timeSec;
            SoulMoney.Value = soulMoney;
        }
    }
}
