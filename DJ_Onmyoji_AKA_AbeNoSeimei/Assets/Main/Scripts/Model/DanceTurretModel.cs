using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// ダンス
    /// モデル
    /// </summary>
    public class DanceTurretModel : TurretModel
    {
        private int criticalNum = 0;

        protected override OnmyoBulletConfig InitializeOnmyoBulletConfig()
        {
            return new OnmyoBulletConfig()
            {
                actionRate = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate),
                attackPoint = (int)_shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.AttackPoint),
                bulletLifeTime = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.BulletLifeTime),
                range = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.Range),
                subSkillType = _shikigamiUtility.GetSubSkillType(_shikigamiInfo),
                subSkillRank = _shikigamiUtility.GetSubSkillRank(_shikigamiInfo),
                subSkillValue = _shikigamiUtility.GetSubSkillValue(_shikigamiInfo),
                // 陰陽玉と発射角度が異なるため再設定
                moveSpeed = 0f,
                trackingOfAny = RectTransform,
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
            //サブスキルが急所の場合、4回に1回の攻撃力、範囲を強化
            if (SubSkillType.Critical.Equals(onmyoBulletConfig.subSkillType))
            {
                criticalNum += 1;

                if (criticalNum >= 4)
                {
                    onmyoBulletConfig.attackPoint = (int)(_shikigamiUtility.GetMainSkillValueAddValueBuffMax(_shikigamiInfo, MainSkillType.AttackPoint) * onmyoBulletConfig.subSkillValue);
                    onmyoBulletConfig.range = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.Range) * onmyoBulletConfig.subSkillValue;
                    criticalNum = 0;
                }
            }
            return _turretUtility.CallInitialize(objectsPoolModel.GetDanceHallModel(), RectTransform, onmyoBulletConfig);
        }

        public override bool UpdateTempoLvValue(float tempoLevel, ShikigamiType shikigamiType)
        {
            try
            {
                switch (shikigamiType)
                {
                    case ShikigamiType.Dance:
                        if (_shikigamiInfo.state.tempoLevel != null)
                        {
                            _shikigamiInfo.state.tempoLevel.Value = tempoLevel;

                            //オーラのサイズを変更
                            if (auraRectTransform != null)
                                auraRectTransform.sizeDelta = new Vector2(tempoLevel + 1.0f, tempoLevel + 1.0f);

                            MainGameManager.Instance.AudioOwner.SetEQ(MapValue(tempoLevel), "EQ_MID");
                        }
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
