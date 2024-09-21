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
                //  ●威力、レート、持続
                _moveDirection = Quaternion.Euler(eulerAngles) * (!updateConf.moveDirection.Equals(Vector2.zero) ?
                    updateConf.moveDirection : onmyoBulletConfig.moveDirection);
                _moveSpeed = updateConf.moveSpeed != null ? updateConf.moveSpeed.Value : onmyoBulletConfig.moveSpeed.Value;
                _disableTimeSec = updateConf.bulletLifeTime;
                // ラップのみ
                //  ●ホーミング性能
                if (0f < updateConf.homing)
                    if (!searchRangeOfEnemyCollider.SetRadiosOfCircleCollier2D(updateConf.homing))
                        throw new System.Exception("SetRadiosOfCircleCollier2D");
                Transform.position = position;
                if (!attackColliderOfOnmyoBullet.SetAttackPoint(updateConf.attackPoint))
                    throw new System.Exception("SetAttackPoint");

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
