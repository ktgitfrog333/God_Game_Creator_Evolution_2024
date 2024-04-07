using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Audio;
using Main.Common;
using Main.Model;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class ShikigamiSkillSystemModelTest3 : MonoBehaviour
    {
        private ShikigamiSkillSystemModel _shikigamiSkillSystemModel;
        [SerializeField] private InputSlipLoopState inputSlipLoopState;
        /// <summary>ループ中</summary>
        public bool isLooping;
        /// <summary>処理場で必要なBGMの情報</summary>
        [SerializeField] private BGMInfo[] bGMInfos;
        [SerializeField] private ClipToPlayBGM clipToPlayBGM;
        private float _elapsedTime = 0f;
        // Start is called before the first frame update
        void Start()
        {
            _shikigamiSkillSystemModel = GameObject.Find("ShikigamiSkillSystem").GetComponent<ShikigamiSkillSystemModel>();
            inputSlipLoopState.IsLooping = new BoolReactiveProperty();
        }

        // Update is called once per frame
        void Update()
        {
            inputSlipLoopState.IsLooping.Value = isLooping;
            if (inputSlipLoopState.IsLooping.Value)
            {
                var bpm = bGMInfos.Where(q => q.clipToPlayBGM.Equals(clipToPlayBGM))
                    .Select(q => q.bpm)
                    .ToArray();
                if (bpm.Length < 1)
                    throw new System.ArgumentNullException($"対象のBPMがBGM情報に存在しない:[{clipToPlayBGM}]");
                float beat = 60f / bpm[0];
                var limit = BeatLengthApp.GetTotalReverse(inputSlipLoopState, beat);
                // Debug.Log($"time:[{_elapsedTime}/{limit}]");
                if (limit <= _elapsedTime)
                {
                    if (!_shikigamiSkillSystemModel.UpdateCandleResourceOfAttackOnmyoTurret())
                        Debug.LogError("UpdateCandleResourceOfAttackOnmyoTurret");
                    _elapsedTime = 0f;
                }
                else
                    _elapsedTime += Time.deltaTime;
            }
        }
    }
}
