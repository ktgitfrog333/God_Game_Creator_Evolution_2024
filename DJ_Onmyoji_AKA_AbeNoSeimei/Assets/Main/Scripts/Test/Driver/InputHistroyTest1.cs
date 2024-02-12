using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Test.Driver
{
    public class InputHistroyTest1 : MonoBehaviour
    {
        [SerializeField] private List<float> sampleFloats = new List<float>();
        [SerializeField] private List<float> sampleFloats_2 = new List<float>();
        // Start is called before the first frame update
        void Start()
        {
            List<float> sampleFloats = new List<float>()
            {
                0f, .01f, .0001f, -.0001f, .012354f, -.03415f,
            };
            this.sampleFloats = sampleFloats;
            var ignoreZero = sampleFloats.Where(q => q != 0f)
                .Select(q => q)
                .ToList();
            sampleFloats_2 = ignoreZero;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
