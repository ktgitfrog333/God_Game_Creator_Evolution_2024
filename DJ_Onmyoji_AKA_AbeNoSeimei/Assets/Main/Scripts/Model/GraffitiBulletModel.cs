using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using UniRx;
using Main.View;
using Effect.Model;
using Effect.Utility;
using UniRx.Triggers;
using Universal.Utility;

namespace Main.Model
{
    /// <summary>
    /// 魔力弾（グラフィティ用）
    /// モデル
    /// </summary>
    public class GraffitiBulletModel : BulletModel, IBulletModel
    {
        /// <summary>最小範囲</summary>
        private const float RANGE_MIN = 0f;
        /// <summary>最大範囲</summary>
        private float _rangeMax;
        /// <summary>経過時間</summary>
        IReactiveProperty<float> elapsedTime = new FloatReactiveProperty();
        /// <summary>最大範囲</summary>
        private bool _StartedGraffAttack = false;
        /// <summary>グラフィティ効果時間</summary>
        private float _debuffEffectLifeTime = 0f;
        /// <summary>グラフィティ効果範囲Sprite</summary>
        public SpriteRenderer graffSprite;
        /// <summary>グラフィティ効果範囲コライダー</summary>
        private CircleCollider2D circleCollider2DGraff;

        public bool Initialize(Vector2 position, Vector3 eulerAngles, OnmyoBulletConfig updateConf)
        {
            try
            {
                // グラフティ
                //  ●持続、効果時間、レート、範囲
                _moveDirection = Quaternion.Euler(eulerAngles) * (!updateConf.moveDirection.Equals(Vector2.zero) ?
                    updateConf.moveDirection : onmyoBulletConfig.moveDirection);
                _moveSpeed = updateConf.moveSpeed != null ? updateConf.moveSpeed.Value : onmyoBulletConfig.moveSpeed.Value;
                _disableTimeSec = updateConf.bulletLifeTime;
                onmyoBulletConfig.subSkillType = updateConf.subSkillType;
                onmyoBulletConfig.subSkillRank = updateConf.subSkillRank;
                onmyoBulletConfig.subSkillValue = updateConf.subSkillValue;
                Transform.position = position;
                if (0f < updateConf.range)
                    _rangeMax = updateConf.range;
                _debuffEffectLifeTime = updateConf.debuffEffectLifeTime;
                if (!attackColliderOfOnmyoBullet.SetAttackPoint(updateConf.attackPoint))
                    throw new System.Exception("SetAttackPoint");

                circleCollider2DGraff = GetComponent<CircleCollider2D>();

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        protected override void Start()
        {
            base.Start();
            var graffitiBulletView = GetComponent<GraffitiBulletView>();
            if (graffitiBulletView.IsFoundAnimator)
                this.ObserveEveryValueChanged(_ => Transform.position)
                    .Pairwise()
                    .Subscribe(pair =>
                    {
                        var moveSpeed = Mathf.Abs(pair.Current.sqrMagnitude - pair.Previous.sqrMagnitude);
                        if (0f < moveSpeed)
                            if (!graffitiBulletView.PlayWalkingAnimation(moveSpeed))
                                Debug.LogError("PlayWalkingAnimation");
                    });

            // 一定間隔でダメージ判定するための実装
            Observable.Interval(System.TimeSpan.FromSeconds(1))
                .Where(_ => _StartedGraffAttack)
                .Subscribe(_ =>
                {
                    foreach (var obj in objectsInContact)
                    {
                        var circleCollider2D = obj.GetComponent<CircleCollider2D>();
                        var damageSufferedZoneOfEnemyModel = obj.GetComponent<DamageSufferedZoneOfEnemyModel>();
                        damageSufferedZoneOfEnemyModel.OnTriggerEnter2DGraff(circleCollider2D);
                    }
                }).AddTo(this);

        }

        protected override void OnEnable()
        {
            base.OnEnable();
            elapsedTime.Value = 0f;
            _StartedGraffAttack = false;
            objectsInContact.Clear();

            if (circleCollider2DGraff != null)
                circleCollider2DGraff.radius = _rangeMax;
            if (graffSprite != null)
            {
                graffSprite.enabled = false;
                graffSprite.transform.localScale = new Vector3(_rangeMax * 2.5f, _rangeMax * 2.5f, 1.0f);
            }
                
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!_turretUtility.UpdateScale(elapsedTime, _disableTimeSec, RANGE_MIN, _rangeMax, attackColliderOfOnmyoBullet))
                Debug.LogError("UpdateScale");
        }

        public void StartGraffAttack()
        {
            if (!_StartedGraffAttack)
            {
                //接敵地点から動かないようにする
                _moveSpeed = 0f;
                _StartedGraffAttack = true;
                //グラフィティの持続タイマー起動
                StartCoroutine(GeneralUtility.ActionsAfterDelay(_debuffEffectLifeTime, () => gameObject.SetActive(false)));
                graffSprite.enabled = true;
            }
        }

        // 接触しているオブジェクトを格納するリスト
        private List<GameObject> objectsInContact = new List<GameObject>();
        // タグを指定
        private string targetTag = "GraffTarget";

        void OnTriggerEnter2D(Collider2D other)
        {
            // 特定のタグを持つオブジェクトがトリガーに入った場合にリストに追加
            if (other.CompareTag(targetTag))
            {
                objectsInContact.Add(other.gameObject);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // 特定のタグを持つオブジェクトがトリガーから出た場合にリストから削除
            if (other.CompareTag(targetTag))
            {
                objectsInContact.Remove(other.gameObject);
            }
        }
    }
}
