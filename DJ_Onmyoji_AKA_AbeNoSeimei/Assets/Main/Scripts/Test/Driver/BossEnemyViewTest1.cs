using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.View;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class BossEnemyViewTest1 : MonoBehaviour
    {
        [SerializeField] private BossEnemyView bossEnemyView;
        private void Reset()
        {
            bossEnemyView = GameObject.Find("BossEnemy01KingAoandon").GetComponent<BossEnemyView>();
        }
        private void OnGUI()
        {
            if (GUI.Button(new Rect(20,40,80,20), $"{BossDirectionPhase.Wait}"))
                Observable.FromCoroutine<bool>(observer => bossEnemyView.MovePointEntrance(observer, (int)BossDirectionPhase.Wait))
                    .Subscribe(x => Debug.Log($"{BossDirectionPhase.Wait}:[{x}]"))
                    .AddTo(gameObject);
            if (GUI.Button(new Rect(20,70,80,20), $"{BossDirectionPhase.Entrance}"))
                Observable.FromCoroutine<bool>(observer => bossEnemyView.MovePointEntrance(observer, (int)BossDirectionPhase.Entrance))
                    .Subscribe(x => Debug.Log($"{BossDirectionPhase.Entrance}:[{x}]"))
                    .AddTo(gameObject);
            if (GUI.Button(new Rect(20,100,80,20), $"{BossDirectionPhase.Exit}"))
                Observable.FromCoroutine<bool>(observer => bossEnemyView.MovePointEntrance(observer, (int)BossDirectionPhase.Exit))
                    .Subscribe(x => Debug.Log($"{BossDirectionPhase.Exit}:[{x}]"))
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
