using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Common;

namespace Universal.Bean
{
    [System.Serializable]
    /// <summary>
    /// ユーザー情報を保持するクラス
    /// </summary>
    public class UserBean
    {
        /// <summary>
        /// デフォルトのシーンID
        /// </summary>
        private static readonly int SCENEID_DEFAULT = 1;
        
        /// <summary>
        /// 全てのシーンID
        /// </summary>
        private readonly int SCENEID_ALL = 1;
        
        
        /// <summary>
        /// デフォルトのクリアステータス
        /// </summary>
        private readonly int[] STATE_DEFAULT = {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        
        /// <summary>
        /// 全解放時のクリアステータス
        /// </summary>
        private readonly int[] STATE_ALL = {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2};

        /// <summary>
        /// シーンID
        /// </summary>
        public int sceneId = SCENEID_DEFAULT;
        
        /// <summary>
        /// クリアステータス
        /// </summary>
        public int[] state = new int[ConstBeanRules.STATELENGTH];
        
        /// <summary>
        /// オーディオボリュームインデックス
        /// </summary>
        public int audioVolumeIndex = 5;
        
        /// <summary>
        /// BGMボリュームインデックス
        /// </summary>
        public int bgmVolumeIndex = 5;
        
        /// <summary>
        /// SEボリュームインデックス
        /// </summary>
        public int seVolumeIndex = 5;
        
        /// <summary>
        /// 振動有効インデックス
        /// </summary>
        public int vibrationEnableIndex = 0;

        public PentagramTurnTableInfo pentagramTurnTableInfo = new PentagramTurnTableInfo()
        {
            slots = new PentagramTurnTableInfo.Slot[]
            {
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 0,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        type = 3,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                    },
                },
                new PentagramTurnTableInfo.Slot()
                {
                    slotId = 1,
                    shikigamiInfo = new ShikigamiInfo()
                    {
                        type = 3,
                        level = 1,
                        mainSkills = new ShikigamiInfo.MainSkill[]
                        {
                            new ShikigamiInfo.MainSkill
                            {
                                type = 1,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 3,
                                rank = 0,
                            },
                            new ShikigamiInfo.MainSkill
                            {
                                type = 2,
                                rank = 0,
                            },
                        },
                    },
                },
            }
        };

        /// <summary>魂のお金</summary>
        public int soulMoney = 0;
        /// <summary>入力モード</summary>
        public int inputMode = 0;

        /// <summary>
        /// ユーザー情報を保持するクラス
        /// </summary>
        public UserBean(EnumLoadMode enumLoadMode=EnumLoadMode.Continue)
        {
            switch (enumLoadMode)
            {
                case EnumLoadMode.Continue:
                    break;
                case EnumLoadMode.Default:
                    sceneId = SCENEID_DEFAULT;
                    state = STATE_DEFAULT;

                    break;
                case EnumLoadMode.All:
                    sceneId = SCENEID_ALL;
                    state = STATE_ALL;

                    break;
            }
        }

        /// <summary>
        /// 式神の情報
        /// </summary>
        /// <see cref="Main.Common.ShikigamiInfo"/>
        [System.Serializable]
        public class ShikigamiInfo
        {
            /// <summary>式神タイプ</summary>
            public int type;
            /// <summary>レベル</summary>
            public int level;
            /// <summary>メインスキル</summary>
            public MainSkill[] mainSkills;
            /// <summary>サブスキル</summary>
            public SubSkill[] subSkills;

            /// <summary>
            /// メインスキル
            /// </summary>
            [System.Serializable]
            public class MainSkill
            {
                /// <summary>メインスキルタイプ</summary>
                public int type;
                /// <summary>スキルランク</summary>
                public int rank;
            }

            /// <summary>
            /// サブスキル
            /// </summary>
            [System.Serializable]
            public class SubSkill
            {
                /// <summary>サブスキルタイプ</summary>
                public int type;
                /// <summary>スキルランク</summary>
                public int rank;
            }
        }

        /// <summary>
        /// ペンダグラムターンテーブル情報
        /// </summary>
        /// <see cref="Main.Common.PentagramTurnTableInfo"/>
        [System.Serializable]
        public class PentagramTurnTableInfo
        {
            /// <summary>スロット</summary>
            public Slot[] slots;

            /// <summary>
            /// スロット
            /// </summary>
            [System.Serializable]
            public class Slot
            {
                /// <summary>スロット番号</summary>
                public int slotId;
                /// <summary>式神の情報</summary>
                public ShikigamiInfo shikigamiInfo;
            }
        }

        /// <summary>
        /// ユーザー情報を保持するクラス
        /// </summary>
        public UserBean(UserBean userBean)
        {
            sceneId = userBean.sceneId;
            state = userBean.state;
            audioVolumeIndex = userBean.audioVolumeIndex;
            bgmVolumeIndex = userBean.bgmVolumeIndex;
            seVolumeIndex = userBean.seVolumeIndex;
            vibrationEnableIndex = userBean.vibrationEnableIndex;
            pentagramTurnTableInfo = userBean.pentagramTurnTableInfo;
            soulMoney = userBean.soulMoney;
            inputMode = userBean.inputMode;
        }
    }
}
