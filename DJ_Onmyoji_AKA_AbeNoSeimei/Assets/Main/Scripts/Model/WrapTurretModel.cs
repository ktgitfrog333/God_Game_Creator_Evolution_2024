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
        protected override OnmyoBulletConfig GetOnmyoBulletConfig()
        {
            return new OnmyoBulletConfig()
            {
                actionRate = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate),
                attackPoint = (int)_shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.AttackPoint),
                bulletLifeTime = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.BulletLifeTime),
                // 陰陽玉と発射角度が異なるため再設定
                moveDirection = new MainCommonUtility().AdminDataSingleton.AdminBean.WrapTurretModel.moveDirection,
            };
        }

        protected override bool ActionOfBullet(ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig onmyoBulletConfig)
        {
            return _turretUtility.CallInitialize(objectsPoolModel.GetWrapBulletModel(), RectTransform, onmyoBulletConfig);
        }
    }
}
