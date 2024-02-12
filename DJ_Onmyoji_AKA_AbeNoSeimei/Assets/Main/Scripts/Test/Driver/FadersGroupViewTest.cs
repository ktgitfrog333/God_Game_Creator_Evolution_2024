using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Main.Common;

namespace Main.Test.Driver
{
    public class FadersGroupViewTest : MonoBehaviour
    {
        public IReactiveProperty<int> IsOpen { get; private set; } = new IntReactiveProperty();
        [SerializeField] private bool isOpen;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            IsOpen.Value = isOpen ? (int)EnumFadeState.Open : (int)EnumFadeState.Close;
        }
    }
}
