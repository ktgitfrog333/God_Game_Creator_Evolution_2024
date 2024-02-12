using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    public class EnemyModelDemo : MonoBehaviour
    {
        [SerializeField] private EnemyModel enemyModel;
        [SerializeField] private Vector2 position = new Vector2(-4.06f, -0.59f);
        [SerializeField] private Transform target;
        public void Case_0()
        {
            if (!enemyModel.Initialize(position, target))
                Debug.LogError("Initialize");
            if (!enemyModel.isActiveAndEnabled)
                enemyModel.gameObject.SetActive(true);
        }

        private void Reset()
        {
            enemyModel = GameObject.Find("Enemy").GetComponent<EnemyModel>();
            target = GameObject.Find("Player").transform;
        }
    }
}
