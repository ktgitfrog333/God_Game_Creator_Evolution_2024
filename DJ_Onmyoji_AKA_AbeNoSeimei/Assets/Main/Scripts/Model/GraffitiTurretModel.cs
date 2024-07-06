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
    public class GraffitiTurretModel : TurretModel, IWrapTurretModel
    {
        protected override OnmyoBulletConfig InitializeOnmyoBulletConfig()
        {
            return new OnmyoBulletConfig()
            {
                actionRate = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate),
                bulletLifeTime = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.BulletLifeTime),
                // 陰陽玉と発射角度が異なるため再設定
                range = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.Range),
                debuffEffectLifeTime = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.DebuffEffectLifeTime),
                attackPoint = 1,
            };
        }

        protected override OnmyoBulletConfig ReLoadOnmyoBulletConfig(OnmyoBulletConfig config)
        {
            config.actionRate = _shikigamiUtility.GetMainSkillValueAddValueBuffMax(_shikigamiInfo, MainSkillType.ActionRate);
            config.attackPoint = 1;

            return _turretUtility.UpdateMoveDirection(_bulletCompass, config);
        }

        protected override bool ActionOfBullet(ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig onmyoBulletConfig)
        {
            return _turretUtility.CallInitialize(objectsPoolModel.GetGraffitiBulletModel(), RectTransform, onmyoBulletConfig);
        }

        public override bool UpdateTempoLvValue(float tempoLevel, ShikigamiType shikigamiType)
        {
            try
            {
                switch (shikigamiType)
                {
                    case ShikigamiType.Graffiti:
                        if (_shikigamiInfo.state.tempoLevel != null)
                        {
                            _shikigamiInfo.state.tempoLevel.Value = tempoLevel;

                            //オーラのサイズを変更
                            if (auraRectTransform != null)
                                auraRectTransform.sizeDelta = new Vector2(tempoLevel + 1.0f, tempoLevel + 1.0f);

                            MainGameManager.Instance.AudioOwner.SetEQ(MapValue(tempoLevel), "EQ_LOW");
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

        public bool InitializeBulletCompass(Vector2 fromPosition, Vector2 danceVector)
        {
            return _turretUtility.InitializeBulletCompass(ref _bulletCompass,
                (new Vector2(RectTransform.position.x, RectTransform.position.y) - fromPosition).normalized,
                danceVector);
        }

        public bool SetBulletCompassType(BulletCompassType bulletCompassType)
        {
            return _turretUtility.SetBulletCompassType(ref _bulletCompass, bulletCompassType);
        }
    }
}
