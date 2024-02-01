using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Main.Test.Driver
{
    public class SunMoonSystemModelTest : MonoBehaviour
    {
        [SerializeField, Range(-1f, 1f)] private float onmyoState;
        // Start is called before the first frame update
        void Start()
        {
            SunMoonSystemModel sunMoonSystemModel = null;
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (sunMoonSystemModel == null)
                        sunMoonSystemModel = GameObject.Find("SunMoonSystem").GetComponent<SunMoonSystemModel>();
                    ((ISunMoonSystemModelTest)sunMoonSystemModel).SetOnmyoState(onmyoState);
                });
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

    /// <see cref="Main.Model.SunMoonSystemModel"/>
    public interface ISunMoonSystemModelTest
    {
        /// <see cref="Main.Model.SunMoonSystemModel.OnmyoState"/>
        public bool SetOnmyoState(float onmyoState);
    }
}
