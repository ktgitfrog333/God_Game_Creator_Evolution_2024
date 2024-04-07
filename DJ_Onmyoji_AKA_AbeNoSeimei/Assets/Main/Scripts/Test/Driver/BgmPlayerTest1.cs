using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Audio;
using Main.Common;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class BgmPlayerTest1 : MonoBehaviour
    {
        [SerializeField] private InputSlipLoopState inputSlipLoopState;
        /// <summary>ループ中</summary>
        public bool isLooping;
        /// <summary>処理場で必要なBGMの情報</summary>
        [SerializeField] private BGMInfo[] bGMInfos;
        [SerializeField] private ClipToPlayBGM clipToPlayBGM;

        private AudioOwner _audioOwner;
        private float _elapsedTime = 0f;

        private void Start()
        {
            inputSlipLoopState.IsLooping = new BoolReactiveProperty();
            if (_audioOwner == null)
                _audioOwner = MainGameManager.Instance.AudioOwner;
            _audioOwner.PlayBGM(clipToPlayBGM);
        }

        void Update()
        {
            if (_audioOwner == null)
                _audioOwner = MainGameManager.Instance.AudioOwner;
            
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
                Debug.Log($"time:[{_elapsedTime}/{limit}]");
                if (limit <= _elapsedTime)
                {
                    _audioOwner.PlayBack(inputSlipLoopState);
                    _elapsedTime = 0f;
                }
                else
                    _elapsedTime += Time.deltaTime;
            }
        }
    }
}
