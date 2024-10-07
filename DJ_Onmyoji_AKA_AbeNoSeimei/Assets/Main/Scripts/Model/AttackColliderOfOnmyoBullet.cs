using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.View;
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
        /// <summary>ラップ弾Model</summary>
        [SerializeField] private WrapBulletModel wrapBulletModel;
        /// <summary>式神スキル管理システムの情報</summary>
        private ShikigamiSkillSystemModel shikigamiSkillSystemModel;
        /// <summary>敵が攻撃範囲へ侵入した判定のトリガー</summary>
        [Tooltip("敵が攻撃範囲へ侵入した判定のトリガー")]
        [SerializeField] protected SearchRangeOfEnemyCollider searchRangeOfEnemyCollider;
        /// <summary>サブスキルタイプ</summary>
        public SubSkillType subSkillType;
        /// <summary>サブスキルランク</summary>
        public SkillRank subSkillRank;
        /// <summary>サブスキルランク</summary>
        public float subSkillValue;
        /// <summary>貫通ヒットリスト</summary>
        private List<Collider2D> penetrateHitList = new List<Collider2D>();

        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_ENEMY;
            circleCollider2D = GetComponent<CircleCollider2D>();
            penetrateHitList.Clear();
        }

        protected override void Start()
        {
            graffitiBulletModel = this.transform.parent.GetComponent<GraffitiBulletModel>();
            wrapBulletModel = this.transform.parent.GetComponent<WrapBulletModel>();
            shikigamiSkillSystemModel = FindObjectOfType<ShikigamiSkillSystemModel>();
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
            else if(ShikigamiType.Wrap.Equals(shikigamiType[0]) && SubSkillType.Penetrating.Equals(subSkillType))
            {
                //貫通ヒットリストに含まれる敵は無視（既に攻撃済みのため）
                if (penetrateHitList.Contains(other) || !other.gameObject.CompareTag("Enemy"))
                    return;

                //貫通ヒットリストに追加し、残貫通回数-1
                penetrateHitList.Add(other);
                subSkillValue = subSkillValue - 1f;

                //貫通回数がなくなれば消滅
                if (subSkillValue <= 0f)
                    base.OnTriggerEnter2D(other);

                //初回のヒット時にホーミング処理を解除
                if (searchRangeOfEnemyCollider != null)
                    searchRangeOfEnemyCollider.EndHoming();
            }
            else if (ShikigamiType.Wrap.Equals(shikigamiType[0]) && SubSkillType.Explosion.Equals(subSkillType))
            {
                //爆発判定
                if (wrapBulletModel != null)
                    wrapBulletModel.Explosion();

                //爆発ダメージを与えて、弾は消滅（直撃の相手には直撃ダメ＋爆風ダメが入るので、2倍ダメージ入る）
                base.OnTriggerEnter2D(other);
            }
            else if (SubSkillType.Paralysis.Equals(subSkillType) || SubSkillType.Knockback.Equals(subSkillType) || SubSkillType.Poison.Equals(subSkillType) || SubSkillType.Fire.Equals(subSkillType) || SubSkillType.Darkness.Equals(subSkillType) || SubSkillType.Curse.Equals(subSkillType))
            {
                //麻痺、突風（ノックバック）、毒、炎上、暗闇、呪詛
                DamageSufferedZoneOfEnemyModel damageSufferedZoneOfEnemyModel = other.GetComponent<DamageSufferedZoneOfEnemyModel>();

                if(damageSufferedZoneOfEnemyModel != null)
                    damageSufferedZoneOfEnemyModel.SetBadStatus(subSkillType, subSkillValue);

                base.OnTriggerEnter2D(other);
            }
            else if (SubSkillType.Heal.Equals(subSkillType))
            {
                DamageSufferedZoneOfEnemyModel damageSufferedZoneOfEnemyModel = other.GetComponent<DamageSufferedZoneOfEnemyModel>();
                //敵に当たった時だけ回復
                if (damageSufferedZoneOfEnemyModel != null)
                    shikigamiSkillSystemModel.HealResource(subSkillValue);
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
