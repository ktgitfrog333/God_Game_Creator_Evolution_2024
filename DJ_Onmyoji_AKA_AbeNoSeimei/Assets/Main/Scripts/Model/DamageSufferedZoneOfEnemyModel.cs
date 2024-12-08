using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using Universal.Common;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// 敵が攻撃を受ける判定のトリガー
    /// </summary>
    public class DamageSufferedZoneOfEnemyModel : DamageSufferedZoneModel
    {
        /// <summary>即死タグ用</summary>
        [SerializeField] private string[] deadTags = { ConstTagNames.TAG_NAME_PLAYER };
        /// <summary>プレイヤーに触れた</summary>
        public IReactiveProperty<bool> IsHitPlayer { get; private set; } = new BoolReactiveProperty();
        /// <summary>接触を無視する弾（接触済み）のリスト</summary>
        protected List<Collider2D> ignoredCollider2DList = new List<Collider2D>();
        /// <summary>状態異常ステータス</summary>
        public IReactiveProperty<SubSkillType> badStatus { get; private set; } = new ReactiveProperty<SubSkillType>(SubSkillType.None);
        /// <summary>状態異常ステータス継続時間</summary>
        public float badStatusSec { get; private set; }
        /// <summary>状態異常コルーチン</summary>
        private Coroutine badStatusCoroutine = null;

        private void Reset()
        {
            tags = new string[1];
            tags[0] = ConstTagNames.TAG_NAME_BULLET;
            deadTags = new string[1];
            deadTags[0] = ConstTagNames.TAG_NAME_PLAYER;
            ignoredCollider2DList.Clear();
        }

        protected override void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            invincibleTimeSec = adminDataSingleton.AdminBean.enemyModel.damageSufferedZoneOfEnemyModel.invincibleTimeSec;
            base.Start();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (ignoredCollider2DList.Contains(other))
                return;

            var attackColliderOfOnmyoBullet = other.GetComponent<AttackColliderOfOnmyoBullet>();
            if(attackColliderOfOnmyoBullet != null)
                if (SubSkillType.Penetrating.Equals(attackColliderOfOnmyoBullet.subSkillType))
                {
                    ignoredCollider2DList.Add(other);
                    StartCoroutine(RemoveColliderFromListAfterDelay(other, 2.0f));
                }

            base.OnTriggerEnter2D(other);

            if (_utility.IsCompareTagAndUpdateReactiveFlag(other, deadTags, IsHit))
                IsHitPlayer.Value = true;
        }

        public void OnTriggerEnter2DGraff(Collider2D other, OnmyoBulletConfig onmyoBulletConfig)
        {
            if (SubSkillType.Thunder.Equals(onmyoBulletConfig.subSkillType))
                base.OnTriggerEnter2DGraff(other, onmyoBulletConfig.subSkillValue);
            else if (SubSkillType.Poison.Equals(onmyoBulletConfig.subSkillType) ||
                SubSkillType.Darkness.Equals(onmyoBulletConfig.subSkillType) ||
                SubSkillType.Curse.Equals(onmyoBulletConfig.subSkillType))
            {
                if (_utility.IsCompareTagAndUpdateReactiveFlagPublic(other, tags, IsHit, shikigamiType))
                    SetBadStatus(onmyoBulletConfig.subSkillType, onmyoBulletConfig.subSkillValue);

                base.OnTriggerEnter2DGraff(other, 50.0f);
            }
            else
                base.OnTriggerEnter2DGraff(other, 50.0f);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            IsHitPlayer.Value = false;
            ignoredCollider2DList.Clear();
        }

        public void SetBadStatus(SubSkillType inputBadStatus, float inputBadStatusSec)
        {
            if (badStatusCoroutine != null)
            {
                StopCoroutine(badStatusCoroutine);
            }

            if (SubSkillType.Darkness.Equals(inputBadStatus) && savedShikigamiType.Length == 0)
            {
                savedShikigamiType = shikigamiType;
                shikigamiType = new ShikigamiType[] { ShikigamiType.Wrap, ShikigamiType.Dance, ShikigamiType.Graffiti, ShikigamiType.OnmyoTurret };
            }

            badStatus.Value = inputBadStatus;
            badStatusCoroutine = StartCoroutine(ResetBadStatusCoroutine(inputBadStatusSec));
        }

        IEnumerator ResetBadStatusCoroutine(float inputBadStatusSec)
        {
            yield return new WaitForSeconds(inputBadStatusSec);
            badStatus.Value = SubSkillType.None;

            if (savedShikigamiType.Length != 0)
            {
                shikigamiType = savedShikigamiType;
                savedShikigamiType = new ShikigamiType[0];
            }
        }
        
        // 指定した時間後に無視リストから除外（貫通弾）
        IEnumerator RemoveColliderFromListAfterDelay(Collider2D other, float delay)
        {
            // 指定した時間（秒数）を待つ
            yield return new WaitForSeconds(delay);

            // リストにotherがまだ含まれていれば削除
            if (ignoredCollider2DList.Contains(other))
                ignoredCollider2DList.Remove(other);
        }
    }
}
