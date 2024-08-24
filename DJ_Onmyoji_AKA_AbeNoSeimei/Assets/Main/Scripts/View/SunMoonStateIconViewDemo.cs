using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.View
{
    public class SunMoonStateIconViewDemo : MonoBehaviour
    {
        /// <summary>陰陽（昼夜）の状態</summary>
        public IReactiveProperty<float> OnmyoState { get; private set; } = new FloatReactiveProperty();

        [SerializeField, Range(-1.1f, 1.1f)] private float onmyoStateValue;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            OnmyoState.Value = onmyoStateValue;
        }
    }
}
