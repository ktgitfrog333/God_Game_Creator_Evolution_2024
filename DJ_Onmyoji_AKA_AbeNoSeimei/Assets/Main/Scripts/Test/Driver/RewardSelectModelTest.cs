using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class RewardSelectModelTest : MonoBehaviour
    {
        [SerializeField] private int targetIndex;
        public IReactiveProperty<int> TargetIndex { get; private set; } = new IntReactiveProperty();

        // Start is called before the first frame update
        void Start()
        {
            TargetIndex.Value = targetIndex;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
