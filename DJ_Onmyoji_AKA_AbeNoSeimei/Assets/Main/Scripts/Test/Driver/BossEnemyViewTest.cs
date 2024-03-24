using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.View;
using UnityEngine;

namespace Main.Test.Driver
{
    public class BossEnemyViewTest : MonoBehaviour
    {
        [SerializeField] private BossEnemyView bossEnemyView;
        private void Reset()
        {
            bossEnemyView = GameObject.Find("BossEnemy01KingAoandon").GetComponent<BossEnemyView>();
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(20,40,80,20), $"{BossActionPhase.Attack}"))
                if (!bossEnemyView.Movement((int)BossActionPhase.Attack))
                    Debug.LogError("Movement");
            if (GUI.Button(new Rect(20,70,80,20), $"{BossActionPhase.DamageRight}"))
                if (!bossEnemyView.Movement((int)BossActionPhase.DamageRight))
                    Debug.LogError("Movement");
            if (GUI.Button(new Rect(20,100,80,20), $"{BossActionPhase.DamageLeft}"))
                if (!bossEnemyView.Movement((int)BossActionPhase.DamageLeft))
                    Debug.LogError("Movement");
            if (GUI.Button(new Rect(20,130,80,20), $"{BossActionPhase.DamageAll}"))
                if (!bossEnemyView.Movement((int)BossActionPhase.DamageAll))
                    Debug.LogError("Movement");
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
