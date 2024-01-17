using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Main.Utility;

namespace Main.Model
{
    /// <summary>
    /// グラフィティ
    /// モデル
    /// </summary>
    public class GraffitiTurretModel : TurretModel
    {
        protected override OnmyoBulletConfig GetOnmyoBulletConfig()
        {
            return new OnmyoBulletConfig()
            {
                actionRate = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate),
                bulletLifeTime = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.BulletLifeTime),
                // 陰陽玉と発射角度が異なるため再設定
                range = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.Range),
                moveDirection = new MainCommonUtility().AdminDataSingleton.AdminBean.GraffitiTurretModel.moveDirection,
                debuffEffectLifeTime = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.DebuffEffectLifeTime),
            };
        }

        protected override bool ActionOfBullet(ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig onmyoBulletConfig)
        {
            return _turretUtility.CallInitialize(objectsPoolModel.GetGraffitiBulletModel(), RectTransform, onmyoBulletConfig);
        }
    }
}
