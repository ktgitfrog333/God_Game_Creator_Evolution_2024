using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;

namespace Main.Test.Driver
{
    /// <summary>
    /// 敵
    /// モデル
    /// テスト
    /// </summary>
    public class EnemyModelTest : MonoBehaviour
    {
        private void OnEnable()
        {
            ((IEnemyModelTest)GameObject.Find("EnemyA").GetComponent<EnemyModel>()).KillOfEnemies();
        }
    }

    public interface IEnemyModelTest
    {
        /// <summary>
        /// State.IsDead.Value = trueにするなど
        /// </summary>
        /// <returns></returns>
        /// <see cref="Main.Model.EnemyModel.State"/>
        public bool KillOfEnemies();
    }
}
