using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    public class EnemyModel : MonoBehaviour
    {
        /// <summary>ダメージ判定</summary>
        [SerializeField] private DamageSufferedZoneOfEnemyModel damageSufferedZoneModel;
        /// <summary>当たったか</summary>
        public IReactiveProperty<bool> IsHit => damageSufferedZoneModel.IsHit;

        private void Reset()
        {
            damageSufferedZoneModel = GetComponentInChildren<DamageSufferedZoneOfEnemyModel>();
        }
    }
}
