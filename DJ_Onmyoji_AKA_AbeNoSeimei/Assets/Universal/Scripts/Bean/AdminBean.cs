using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Bean
{
    /// <summary>
    /// 管理者データ
    /// </summary>
    [System.Serializable]
    public class AdminBean
    {
        /// <summary>
        /// デフォルトのBGM名
        /// </summary>
        private static readonly int[] PLAYBGMNAMES_DEFAULT = {1,2,3,4,0,0,0,0,0,0,0,0,0,0,0};
        /// <summary>
        /// デフォルトの最終ステージ
        /// </summary>
        private static readonly int[] FINALSTAGES_DEFAULT = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1};
        /// <summary>
        /// デフォルトのスカイボックス
        /// </summary>
        private static readonly int[] SKYBOXS_DEFAULT = {1,2,0,0,0,0,0,0,0,0,0,0,0,0,0};
        /// <summary>
        /// デフォルトのカウントダウンタイマー
        /// </summary>
        private static readonly float[] CLEAR_COUNTDOWN_TIMER_DEFAULT = {300f,5f,5f,5f,5f,5f,5f,5f,5f,5f,5f,5f,5f,5f,5f};

        /// <summary>
        /// BGM名
        /// </summary>
        public int[] playBgmNames = PLAYBGMNAMES_DEFAULT;
        /// <summary>
        /// 最終ステージ
        /// </summary>
        public int[] finalStages = FINALSTAGES_DEFAULT;
        /// <summary>
        /// スカイボックス
        /// </summary>
        public int[] skyBoxs = SKYBOXS_DEFAULT;
        /// <summary>
        /// カウントダウンタイマー
        /// </summary>
        public float[] clearCountdownTimer = CLEAR_COUNTDOWN_TIMER_DEFAULT;
        public EnemyModel EnemyModel = new EnemyModel();
        public EnemiesSpawnModel EnemiesSpawnModel = new EnemiesSpawnModel();
        public ObjectsPoolModel ObjectsPoolModel = new ObjectsPoolModel();
        public OnmyoBulletModel OnmyoBulletModel = new OnmyoBulletModel();
        public WrapTurretModel WrapTurretModel = new WrapTurretModel();
        public GraffitiTurretModel GraffitiTurretModel = new GraffitiTurretModel();
        public PentagramSystemModel PentagramSystemModel = new PentagramSystemModel();
        public PentagramTurnTableView PentagramTurnTableView = new PentagramTurnTableView();
        public PentagramTurnTableModel PentagramTurnTableModel = new PentagramTurnTableModel();
        public PlayerModel PlayerModel = new PlayerModel();
        public SunMoonSystemModel sunMoonSystemModel = new SunMoonSystemModel();
        public LevelDesign levelDesign = new LevelDesign()
        {
            mainSkillLists = new MainSkillList[]
            {
                new MainSkillList()
                {
                    shikigamiType = 3,
                    mainSkillType = 1,
                    skillRank = 0,
                    value = .5f,
                },
                new MainSkillList()
                {
                    shikigamiType = 3,
                    mainSkillType = 3,
                    skillRank = 0,
                    value = 1f,
                },
                new MainSkillList()
                {
                    shikigamiType = 3,
                    mainSkillType = 2,
                    skillRank = 0,
                    value = 10f,
                },
            },
        };
        public AdminBean()
        {

        }
        /// <summary>
        /// AdminBeanのコピーを作成します
        /// </summary>
        /// <param name="adminBean">コピー元のAdminBean</param>
        public AdminBean(AdminBean adminBean)
        {
            playBgmNames = adminBean.playBgmNames;
            finalStages = adminBean.finalStages;
            skyBoxs = adminBean.skyBoxs;
            clearCountdownTimer = adminBean.clearCountdownTimer;
            EnemyModel = adminBean.EnemyModel;
            EnemiesSpawnModel = adminBean.EnemiesSpawnModel;
            ObjectsPoolModel = adminBean.ObjectsPoolModel;
            OnmyoBulletModel = adminBean.OnmyoBulletModel;
            WrapTurretModel = adminBean.WrapTurretModel;
            PentagramSystemModel = adminBean.PentagramSystemModel;
            PentagramTurnTableView = adminBean.PentagramTurnTableView;
            PentagramTurnTableModel = adminBean.PentagramTurnTableModel;
            PlayerModel = adminBean.PlayerModel;
            sunMoonSystemModel = adminBean.sunMoonSystemModel;
            levelDesign = adminBean.levelDesign;
        }
    }

    [System.Serializable]
    public class EnemyModel
    {
        public prop prop = new prop(1f, 3);
        public DamageSufferedZoneOfEnemyModel DamageSufferedZoneOfEnemyModel = new DamageSufferedZoneOfEnemyModel(0f);
    }

    [System.Serializable]
    public class DamageSufferedZoneOfEnemyModel
    {
        public float invincibleTimeSec;
        public DamageSufferedZoneOfEnemyModel() {}
        public DamageSufferedZoneOfEnemyModel(float invincibleTimeSec)
        {
            this.invincibleTimeSec = invincibleTimeSec;
        }
    }

    [System.Serializable]
    public class prop
    {
        public float moveSpeed;
        public int hpMax;
        public prop()
        {

        }
        public prop(float moveSpeed, int hpMax)
        {
            this.moveSpeed = moveSpeed;
            this.hpMax = hpMax;
        }
    }

    [System.Serializable]
    public class EnemiesSpawnModel
    {
        public float invincibleTimeSec = .5f;
    }

    [System.Serializable]
    public class ObjectsPoolModel
    {
        public int countLimit = 100;
    }

    [System.Serializable]
    public class PentagramSystemModel
    {
        public float autoSpinSpeed = .01f;
    }

    [System.Serializable]
    public class OnmyoBulletModel
    {
        /// <summary>移動方向</summary>
        public Vector2 moveDirection = Vector2.down;
        /// <summary>移動速度</summary>
        public float moveSpeed = 8f;
    }

    [System.Serializable]
    public class WrapTurretModel
    {
        /// <summary>移動方向</summary>
        public Vector2 moveDirection = Vector2.up;
    }

    [System.Serializable]
    public class GraffitiTurretModel
    {
        /// <summary>移動方向</summary>
        public Vector2 moveDirection = Vector2.up;
    }

    [System.Serializable]
    public class PentagramTurnTableView
    {
        public float angleCorrectionValue = 5f;
    }

    [System.Serializable]
    public class PentagramTurnTableModel
    {
        public float distance = 4.5f;
    }

    [System.Serializable]
    public class PlayerModel
    {
        public prop prop = new prop(4f, 10);
        public DamageSufferedZoneOfPlayerModel DamageSufferedZoneOfPlayerModel = new DamageSufferedZoneOfPlayerModel(1f);
    }

    [System.Serializable]
    public class DamageSufferedZoneOfPlayerModel
    {
        public float invincibleTimeSec;
        public DamageSufferedZoneOfPlayerModel() {}
        public DamageSufferedZoneOfPlayerModel(float invincibleTimeSec)
        {
            this.invincibleTimeSec = invincibleTimeSec;
        }
    }

    [System.Serializable]
    public class SunMoonSystemModel
    {
        public float[] durations =
        {
            1.75f,
        };
    }

    [System.Serializable]
    public class LevelDesign
    {
        public MainSkillList[] mainSkillLists;
    }

    [System.Serializable]
    public class MainSkillList
    {
        public int shikigamiType;
        public int mainSkillType;
        public int skillRank;
        public float value;
        public float valueBuffMax;
    }
}
