using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;

namespace Main.Test.Driver
{
    public class ShikigamiSkillSystemModelTest1 : MonoBehaviour
    {
        [SerializeField] private RapidRecoveryType rapidRecoveryType;

        private void OnEnable()
        {
            ((IShikigamiSkillSystemModelTest1)GameObject.Find("ShikigamiSkillSystem").GetComponent<ShikigamiSkillSystemModel>()).SetRapidRecoveryState(rapidRecoveryType);
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

    public interface IShikigamiSkillSystemModelTest1
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rapidRecoveryType"></param>
        /// <returns></returns>
        /// <see cref="Main.Model.ShikigamiSkillSystemModel.CandleInfo"/>
        /// <see cref="Main.Model.ShikigamiSkillSystemModel.ShikigamiInfos"/>
        public bool SetRapidRecoveryState(RapidRecoveryType rapidRecoveryType);
    }
}
