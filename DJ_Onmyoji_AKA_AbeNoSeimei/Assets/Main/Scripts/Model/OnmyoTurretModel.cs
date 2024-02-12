using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// 陰陽玉（陰陽砲台）
    /// モデル
    /// </summary>
    public class OnmyoTurretModel : TurretModel
    {
        protected override OnmyoBulletConfig InitializeOnmyoBulletConfig()
        {
            return new OnmyoBulletConfig()
            {
                actionRate = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate),
                attackPoint = (int)_shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.AttackPoint),
                bulletLifeTime = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.BulletLifeTime),
            };
        }

        protected override OnmyoBulletConfig ReLoadOnmyoBulletConfig(OnmyoBulletConfig config)
        {
            // 陰陽玉はレベルアップなし
            return config;
        }

        protected override bool ActionOfBullet(ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig onmyoBulletConfig)
        {
            return _turretUtility.CallInitialize(objectsPoolModel.GetOnmyoBulletModel(), RectTransform, onmyoBulletConfig);
        }

        public override bool UpdateTempoLvValue(float tempoLevel, ShikigamiType shikigamiType)
        {
            // 陰陽玉はレベルアップなし
            return true;
        }
    }
}
