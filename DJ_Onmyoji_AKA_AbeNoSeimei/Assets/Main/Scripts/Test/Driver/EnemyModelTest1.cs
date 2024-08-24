using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Test.Driver
{
    public class EnemyModelTest1 : MonoBehaviour
    {
        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 70, 50, 30), "Kill Enemy"))
            {
                var enemyModel = FindObjectOfType<Main.Model.EnemyModel>();
                if (enemyModel != null)
                {
                    enemyModel.Kill();
                }
            }
        }
    }
}
