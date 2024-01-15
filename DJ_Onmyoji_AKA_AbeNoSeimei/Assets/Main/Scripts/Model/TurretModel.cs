using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Utility;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// 砲台系
    /// モデル
    /// </summary>
    public abstract class TurretModel : SpawnModel
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>Rectトランスフォーム</summary>
        protected RectTransform RectTransform => Transform as RectTransform;
        /// <summary>式神タイプ別パラメータ管理</summary>
        protected ShikigamiParameterUtility _utility = new ShikigamiParameterUtility();
        /// <summary>式神の情報</summary>
        protected ShikigamiInfo _shikigamiInfo;
        public int InstanceID { get; private set; }

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
        }

        protected override void Start()
        {
            var model = GameObject.Find(ConstGameObjectNames.GAMEOBJECT_NAME_PENTAGRAMTURNTABLE).GetComponent<PentagramTurnTableModel>();
            _shikigamiInfo = _utility.GetShikigamiInfo(model.PentagramTurnTableInfo, InstanceID);
            instanceRateTimeSec = _utility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate);
            base.Start();
        }

        /// <summary>
        /// ターゲット位置を元に調整（From）
        /// </summary>
        /// <param name="rectTransform">UIターゲット情報</param>
        /// <returns>成功／失敗</returns>
        protected Vector3 CalibrationFromTarget(RectTransform rectTransform)
        {
            // UI要素のローカル座標を取得
            Vector3 localPosition = rectTransform.localPosition;
            // ローカル座標をワールド座標に変換
            Vector3 worldPosition = rectTransform.parent.TransformPoint(localPosition);
            return worldPosition;
        }
    }
}
