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
        protected override OnmyoBulletConfig InitializeOnmyoBulletConfig()
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

        protected override OnmyoBulletConfig ReLoadOnmyoBulletConfig(OnmyoBulletConfig config)
        {
            config.actionRate = _shikigamiUtility.GetMainSkillValueAddValueBuffMax(_shikigamiInfo, MainSkillType.ActionRate);
            config.attackPoint = (int)_shikigamiUtility.GetMainSkillValueAddValueBuffMax(_shikigamiInfo, MainSkillType.AttackPoint);

            return config;
        }

        protected override bool ActionOfBullet(ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig onmyoBulletConfig)
        {
            return _turretUtility.CallInitialize(objectsPoolModel.GetWrapBulletModel(), RectTransform, onmyoBulletConfig);
        }

        public override bool UpdateTempoLvValue(float tempoLevel, ShikigamiType shikigamiType)
        {
            try
            {
                switch (shikigamiType)
                {
                    case ShikigamiType.Wrap:
                        if (_shikigamiInfo.state.tempoLevel.HasValue)
                            _shikigamiInfo.state.tempoLevel.Value = tempoLevel;

                        break;
                    default:
                        break;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
