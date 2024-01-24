using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Utility;
using Main.Common;
using UniRx;
using Universal.Utility;

namespace Main.Model
{
    /// <summary>
    /// 砲台系
    /// モデル
    /// </summary>
    public abstract class TurretModel : SpawnModel, ITurretModel
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
        /// <summary>ジョッキーコマンドタイプ</summary>
        private JockeyCommandType _jockeyCommandType = JockeyCommandType.None;
        /// <summary>クローンオブジェクトを生成する時間間隔（秒）のバフ補正値</summary>
        [SerializeField] private float instanceRateTimeSecCorrection = 2f;
        /// <summary>クローンオブジェクトを生成する時間間隔（秒）のホールド補正値</summary>
        private const float INSTANCE_RATE_TIME_SEC_STOP = 60f * 60f;

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
            float elapsedTime = 0f;
            float timeSec;
            // 一定間隔で弾を生成するための実装
            while (true)
            {
                switch (_jockeyCommandType)
                {
                    case JockeyCommandType.Hold:
                        timeSec = INSTANCE_RATE_TIME_SEC_STOP;

                        break;
                    case JockeyCommandType.Scratch:
                        timeSec = instanceRateTimeSec / instanceRateTimeSecCorrection;

                        break;
                    default:
                        // デフォルト値
                        timeSec = instanceRateTimeSec;

                        break;
                }

                // 待機時間に到達していない場合はスキップ、到達していれば実行
                if (timeSec <= elapsedTime)
                {
                    if (!ActionOfBullet(objectsPoolModel, config))
                        Debug.LogError("ActionOfBullet");
                    elapsedTime = 0f;
                }
                else
                    elapsedTime += Time.deltaTime;

                yield return null;
            }
        }

        public bool SetJockeyCommandType(JockeyCommandType jockeyCommandType)
        {
            try
            {
                _jockeyCommandType = jockeyCommandType;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// 砲台系
    /// モデル
    /// インターフェース
    /// </summary>
    public interface ITurretModel
    {
        /// <summary>
        /// ジョッキーコマンドタイプをセット
        /// </summary>
        /// <param name="jockeyCommandType">ジョッキーコマンドタイプ</param>
        /// <returns>成功／失敗</returns>
        public bool SetJockeyCommandType(JockeyCommandType jockeyCommandType);
    }
}
