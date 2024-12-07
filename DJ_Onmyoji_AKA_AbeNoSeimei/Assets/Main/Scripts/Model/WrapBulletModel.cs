using Main.Common;
using Main.View;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 魔力弾（ラップ用）
    /// モデル
    /// </summary>
    public class WrapBulletModel : BulletModel, IBulletModel
    {
        /// <summary>爆発判定用コライダー</summary>
        private CircleCollider2D circleCollider2DExplosion;
        /// <summary>ラップ弾View</summary>
        [SerializeField] private WrapBulletView wrapBulletView;

        protected override void Start()
        {
            base.Start();
            var onmyoBulletView = GetComponent<OnmyoBulletView>();
            if (onmyoBulletView.IsFoundAnimator)
                this.ObserveEveryValueChanged(_ => Transform.position)
                    .Pairwise()
                    .Subscribe(pair =>
                    {
                        var moveSpeed = Mathf.Abs(pair.Current.sqrMagnitude - pair.Previous.sqrMagnitude);
                        if (0f < moveSpeed)
                            if (!onmyoBulletView.PlayWalkingAnimation(moveSpeed))
                                Debug.LogError("PlayWalkingAnimation");
                    });

        }

        public bool Initialize(Vector2 position, Vector3 eulerAngles, OnmyoBulletConfig updateConf)
        {
            try
            {
                // 陰陽玉／ラップ
                //  ●威力、レート、持続、サブスキルタイプ
                _moveDirection = Quaternion.Euler(eulerAngles) * (!updateConf.moveDirection.Equals(Vector2.zero) ?
                    updateConf.moveDirection : onmyoBulletConfig.moveDirection);
                _moveSpeed = updateConf.moveSpeed != null ? updateConf.moveSpeed.Value : onmyoBulletConfig.moveSpeed.Value;
                _disableTimeSec = updateConf.bulletLifeTime;
                onmyoBulletConfig.subSkillType = updateConf.subSkillType;
                onmyoBulletConfig.subSkillRank = updateConf.subSkillRank;
                onmyoBulletConfig.subSkillValue = updateConf.subSkillValue;

                // ラップのみ
                //  ●ホーミング性能
                if (0f < updateConf.homing)
                    if (!searchRangeOfEnemyCollider.SetRadiosOfCircleCollier2D(updateConf.homing))
                        throw new System.Exception("SetRadiosOfCircleCollier2D");
                Transform.position = position;
                if (!attackColliderOfOnmyoBullet.SetAttackPoint(updateConf.attackPoint))
                    throw new System.Exception("SetAttackPoint");

                //サブスキルタイプが爆発の場合は、専用Colliderを設定
                if (SubSkillType.Explosion.Equals(updateConf.subSkillType))
                {
                    circleCollider2DExplosion = GetComponent<CircleCollider2D>();
                    circleCollider2DExplosion.radius = updateConf.subSkillValue;
                    wrapBulletView = GetComponent<WrapBulletView>();
                    wrapBulletView.effectSize = updateConf.subSkillValue;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void Explosion()
        {
            foreach (var obj in objectsInContact)
            {
                var circleCollider2D = obj.GetComponent<CircleCollider2D>();
                var damageSufferedZoneOfEnemyModel = obj.GetComponent<DamageSufferedZoneOfEnemyModel>();
                damageSufferedZoneOfEnemyModel.OnTriggerEnter2DGraff(circleCollider2D, 100.0f);
            }

            //爆発判定
            if (wrapBulletView != null)
                wrapBulletView.Explosion();
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
