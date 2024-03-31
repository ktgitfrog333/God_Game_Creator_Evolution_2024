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
    public class WrapTurretModel : TurretModel, IWrapTurretModel
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
            config.actionRate = _shikigamiUtility.GetMainSkillValueAddValueBuffMax(_shikigamiInfo, MainSkillType.ActionRate);
            config.attackPoint = (int)_shikigamiUtility.GetMainSkillValueAddValueBuffMax(_shikigamiInfo, MainSkillType.AttackPoint);

            return _turretUtility.UpdateMoveDirection(_bulletCompass, config);
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
                        if (_shikigamiInfo.state.tempoLevel != null)
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

    /// <summary>
    /// ラップ
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IWrapTurretModel
    {
        /// <summary>
        /// 弾の角度を動的にセット初期化
        /// </summary>
        /// <param name="fromPosition">中央位置</param>
        /// <param name="danceVector">ダンスの向き</param>
        /// <returns>成功／失敗</returns>
        public bool InitializeBulletCompass(Vector2 fromPosition, Vector2 danceVector);
        /// <summary>
        /// 弾の角度タイプをセット
        /// </summary>
        /// <param name="bulletCompassType">弾の角度タイプ</param>
        /// <returns>成功／失敗</returns>
        public bool SetBulletCompassType(BulletCompassType bulletCompassType);
    }
}
