using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.Collections.LowLevel.Unsafe;
using Main.Utility;
using Universal.Common;
using Main.Common;
using Universal.Bean;

namespace Main.Model
{
    /// <summary>
    /// 陰陽玉（陰陽砲台）
    /// </summary>
    public class OnmyoTurretModel : SpawnModel
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>Rectトランスフォーム</summary>
        private RectTransform RectTransform => Transform as RectTransform;
        /// <summary>式神タイプ別パラメータ管理</summary>
        private ShikigamiParameterUtility _utility = new ShikigamiParameterUtility();
        /// <summary>式神の情報</summary>
        private ShikigamiInfo _shikigamiInfo;
        public int InstanceID { get; private set; }

        private void Awake()
        {
            InstanceID = GetInstanceID();
        }

        protected override void Start()
        {
            var model = GameObject.Find(Common.ConstGameObjectNames.GAMEOBJECT_NAME_PENTAGRAMTURNTABLE).GetComponent<PentagramTurnTableModel>();
            _shikigamiInfo = _utility.GetShikigamiInfo(model.PentagramTurnTableInfo, InstanceID);
            instanceRateTimeSec = _utility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate);
            base.Start();
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
                    (int)_utility.GetMainSkillValue(_shikigamiInfo, MainSkillType.AttackPoint)))
                    Debug.LogError("Initialize");
                if (!bullet.isActiveAndEnabled)
                    bullet.gameObject.SetActive(true);
                yield return new WaitForSeconds(instanceRateTimeSec);
            }
        }

        /// <summary>
        /// ターゲット位置を元に調整（From）
        /// </summary>
        /// <param name="rectTransform">UIターゲット情報</param>
        /// <returns>成功／失敗</returns>
        private Vector3 CalibrationFromTarget(RectTransform rectTransform)
        {
            // UI要素のローカル座標を取得
            Vector3 localPosition = rectTransform.localPosition;
            // ローカル座標をワールド座標に変換
            Vector3 worldPosition = rectTransform.parent.TransformPoint(localPosition);
            return worldPosition;
        }

    }
}
