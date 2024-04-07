using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;
using Main.Common;

namespace Main.Test.Driver
{
    public class ShikigamiSkillSystemModelTest2 : MonoBehaviour
    {
        [SerializeField] private JockeyCommandType jockeyCommandType;
        private void OnEnable()
        {
            GameObject.Find("ShikigamiSkillSystem").GetComponent<ShikigamiSkillSystemModel>().ForceZeroAndRapidRecoveryCandleResource(jockeyCommandType);
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
