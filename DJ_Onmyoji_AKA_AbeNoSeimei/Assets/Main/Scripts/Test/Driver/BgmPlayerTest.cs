using System.Collections;
using System.Collections.Generic;
using Main.Audio;
using Main.Common;
using Main.View;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class BgmPlayerTest : MonoBehaviour
    {
        private void OnEnable()
        {
            Observable.FromCoroutine<bool>(observer => GameObject.Find("PentagramTurnTable").GetComponent<PentagramTurnTableView>().PlayDirectionBackSpin(observer, JockeyCommandType.BackSpin))
                .Subscribe(_ => {})
                .AddTo(gameObject);
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
