using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 攻撃を与える判定のトリガー
    /// </summary>
    public class AttackColliderOfOnmyoBullet : DamageSufferedZoneModel, IAttackColliderOfOnmyoBullet
    {
        /// <summary>円形コライダー2D</summary>
        [SerializeField] private CircleCollider2D circleCollider2D;
        /// <summary>グラフィティ弾Model</summary>
        [SerializeField] private GraffitiBulletModel graffitiBulletModel;

        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_ENEMY;
            circleCollider2D = GetComponent<CircleCollider2D>();
        }

        protected override void Start()
        {
            graffitiBulletModel = this.transform.parent.GetComponent<GraffitiBulletModel>();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (ShikigamiType.Graffiti.Equals(shikigamiType[0]))
            {
                //弾の通常当たり判定を無効化(当たり判定を無効化しないと連続Hitするため)
                if (circleCollider2D != null)
                    circleCollider2D.enabled = false;
                //グラフィティエリアを展開
                if (graffitiBulletModel != null)
                    graffitiBulletModel.StartGraffAttack();
            }
            else
                base.OnTriggerEnter2D(other);
        }

        protected override void OnDisable()
        {
            IsHit.Value = false;
            if (circleCollider2D != null)
                circleCollider2D.enabled = true;
        }

        public bool SetRadiosOfCircleCollier2D(float radios)
        {
            try
            {
                circleCollider2D.radius = radios;

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
    /// 攻撃を与える判定のトリガー
    /// インターフェース
    /// </summary>
    public interface IAttackColliderOfOnmyoBullet : IUniversalCollider
    {
    }

    /// <summary>
    /// 判定のトリガー
    /// インターフェース
    /// </summary>
    public interface IUniversalCollider
    {
        /// <summary>
        /// 円形コライダー2Dの半径をセット
        /// </summary>
        /// <param name="radios">半径</param>
        /// <returns>成功／失敗</returns>
        public bool SetRadiosOfCircleCollier2D(float radios);
    }
}
