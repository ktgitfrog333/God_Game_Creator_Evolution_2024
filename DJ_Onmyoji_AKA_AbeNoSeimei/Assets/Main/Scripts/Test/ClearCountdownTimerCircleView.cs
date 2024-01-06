using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Test
{
    public class ClearCountdownTimerCircleView : MonoBehaviour
    {
        [SerializeField,Range(0f, 10f)] private float timeSec = 10f;
        public IReactiveProperty<float> TimeSec { get; private set; } = new FloatReactiveProperty();
        [SerializeField] private float limitTimeSecMax = 10f;
        public float LimitTimeSecMax => limitTimeSecMax;

        private void Update()
        {
            TimeSec.Value = timeSec;
        }
    }
}
