using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Main.Utility;

namespace Main.Model
{
    /// <summary>
    /// ラップ
    /// モデル
    /// </summary>
    public class WrapTurretModel : TurretModel
    {
        /// <summary>設定</summary>
        [SerializeField] private OnmyoBulletConfig onmyoBulletConfig;

        protected override void Awake()
        {
            var utility = new MainCommonUtility();
            // 陰陽玉と発射角度が異なるため再設定
            onmyoBulletConfig.moveDirection = utility.AdminDataSingleton.AdminBean.WrapTurretModel.moveDirection;
            base.Awake();
        }

        protected override IEnumerator InstanceCloneObjects(float instanceRateTimeSec, ObjectsPoolModel objectsPoolModel)
        {
            // 一定間隔で弾を生成するための実装
            while (true)
            {
                var bullet = objectsPoolModel.GetOnmyoBulletModel();
                if (!bullet.Initialize(CalibrationFromTarget(RectTransform),
                    RectTransform.parent.eulerAngles,
                    _utility.GetMainSkillValue(_shikigamiInfo, MainSkillType.BulletLifeTime),
                    (int)_utility.GetMainSkillValue(_shikigamiInfo, MainSkillType.AttackPoint),
                    onmyoBulletConfig.moveDirection))
                    Debug.LogError("Initialize");
                if (!bullet.isActiveAndEnabled)
                    bullet.gameObject.SetActive(true);
                yield return new WaitForSeconds(instanceRateTimeSec);
            }
        }
    }
}
