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
        protected ShikigamiParameterUtility _shikigamiUtility = new ShikigamiParameterUtility();
        /// <summary>砲台系ユーティリティ</summary>
        protected TurretUtility _turretUtility = new TurretUtility();
        /// <summary>式神の情報</summary>
        protected ShikigamiInfo _shikigamiInfo;
        /// <summary>インスタンスID</summary>
        public int InstanceID { get; private set; }

        protected virtual void Awake()
        {
            InstanceID = GetInstanceID();
        }

        protected override void Start()
        {
            var model = GameObject.Find(ConstGameObjectNames.GAMEOBJECT_NAME_PENTAGRAMTURNTABLE).GetComponent<PentagramTurnTableModel>();
            _shikigamiInfo = _shikigamiUtility.GetShikigamiInfo(model.PentagramTurnTableInfo, InstanceID);
            instanceRateTimeSec = _shikigamiUtility.GetMainSkillValue(_shikigamiInfo, MainSkillType.ActionRate);
            base.Start();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>魔力弾の設定</returns>
        protected abstract OnmyoBulletConfig GetOnmyoBulletConfig();

        /// <summary>
        /// 魔力弾／円舞範囲／デバフ魔力弾の制御
        /// </summary>
        /// <param name="objectsPoolModel">オブジェクトプール</param>
        /// <returns>成功／失敗</returns>
        protected abstract bool ActionOfBullet(ObjectsPoolModel objectsPoolModel, OnmyoBulletConfig onmyoBulletConfig);

        protected override IEnumerator InstanceCloneObjects(float instanceRateTimeSec, ObjectsPoolModel objectsPoolModel)
        {
            var config = GetOnmyoBulletConfig();
            // 一定間隔で弾を生成するための実装
            while (true)
            {
                if (!ActionOfBullet(objectsPoolModel, config))
                    Debug.LogError("ActionOfBullet");
                yield return new WaitForSeconds(instanceRateTimeSec);
            }
        }
    }
}
