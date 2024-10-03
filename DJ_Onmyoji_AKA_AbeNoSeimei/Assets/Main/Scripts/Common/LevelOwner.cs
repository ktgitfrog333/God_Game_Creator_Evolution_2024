using Universal.Template;
using Universal.Common;
using Universal.Bean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Utility;
using System.Linq;

namespace Main.Common
{
    /// <summary>
    /// レベルのオーナー
    /// </summary>
    public class LevelOwner : MonoBehaviour, IMainGameManager, ILevelOwner
    {
        /// <summary>レベルの親オブジェクト</summary>
        [SerializeField] private Transform level;
        /// <summary>各ステージのプレハブ</summary>
        [SerializeField] private GameObject[] levelPrefabs;
        /// <summary>レベルがインスタンス済みか</summary>
        private readonly BoolReactiveProperty _isInstanced = new BoolReactiveProperty();
        /// <summary>レベルがインスタンス済みか</summary>
        public IReactiveProperty<bool> IsInstanced => _isInstanced;
        /// <summary>インスタンス済みレベル</summary>
        private Transform _instancedLevel;
        /// <summary>インスタンス済みレベル</summary>
        public Transform InstancedLevel => _instancedLevel;
        /// <summary>式神と画像を連携する情報</summary>
        [SerializeField] private ShikigamiInfoSplitesProp[] shikigamiInfoSplitesProps;
        /// <summary>選択したリワードID</summary>
        private List<RewardID> _selectedRewardIDs = new List<RewardID>();
        /// <summary>クリア報酬のコンテンツプロパティ</summary>
        private RewardContentProp[] _rewardContentProps;
        /// <summary>クリア報酬の強化プロパティ</summary>
        [SerializeField]
        private EnhanceProp[] enhanceProps = new EnhanceProp[]
        {
            new EnhanceProp()
            {
                level = EnhanceLevel.Mode1,
                soulMoney = 300
            },
            new EnhanceProp()
            {
                level = EnhanceLevel.Mode2,
                soulMoney = 500
            },
            new EnhanceProp()
            {
                level = EnhanceLevel.Mode3,
                soulMoney = 800
            },
        };
        /// <summary>
        /// サブスキルシナジー
        /// </summary>
        /// <remarks>
        /// 一つのサブスキルに複数のタグを指定する
        /// サブスキルの複数定義は禁止
        /// </remarks>
        [SerializeField]
        private SubSkillsSynergy[] subSkillsSynergies = new SubSkillsSynergy[]
        {
            new SubSkillsSynergy()
            {
                subSkillType = SubSkillType.Explosion,
                subSkillTags = new SubSkillTag[]
                {
                    SubSkillTag.ST0001,
                },
            },
            new SubSkillsSynergy()
            {
                subSkillType = SubSkillType.Penetrating,
                subSkillTags = new SubSkillTag[]
                {
                    SubSkillTag.ST0000,
                },
            },
            new SubSkillsSynergy()
            {
                subSkillType = SubSkillType.Spreading,
                subSkillTags = new SubSkillTag[]
                {
                    SubSkillTag.ST0000,
                },
            },
            new SubSkillsSynergy()
            {
                subSkillType = SubSkillType.ContinuousFire,
                subSkillTags = new SubSkillTag[]
                {
                    SubSkillTag.ST0000,
                },
            },
            new SubSkillsSynergy()
            {
                subSkillType = SubSkillType.Paralysis,
                subSkillTags = new SubSkillTag[]
                {
                    SubSkillTag.ST0002,
                },
            },
        };

        private void Reset()
        {
            level = GameObject.Find("Level").transform;
        }

        public void OnStart()
        {
            var disableLevels = GameObject.FindGameObjectsWithTag(ConstTagNames.TAG_NAME_LEVEL);
            if (0 < disableLevels.Length)
            {
                Debug.LogWarning("完成版ではレベルのプレハブをヒエラルキーから削除して下さい");
                foreach (var level in disableLevels)
                {
                    Debug.LogWarning($"レベル:[{level.name}]を無効化しました");
                    level.SetActive(false);
                }
            }
            // シーンIDを取得してステージをLevelオブジェクトの子要素としてインスタンスする
            var temp = new TemplateResourcesAccessory();
            // ステージIDの取得
            var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);

            _instancedLevel = Instantiate(levelPrefabs[datas.sceneId], Vector3.zero, Quaternion.identity, level).transform;
            if (_instancedLevel != null)
                _isInstanced.Value = true;
        }

        public RewardContentProp[] GetRewardContentProps()
        {
            try
            {
                var utility = new MainRewardsUtility();
                _rewardContentProps = utility.InstanceRewardTablesAndGetRewards(shikigamiInfoSplitesProps, enhanceProps, subSkillsSynergies);
                if (_rewardContentProps == null)
                    throw new System.Exception("InstanceRewardTablesAndGetRewards");

                return _rewardContentProps;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public bool AddRewardID(int index, bool isDrop = false)
        {
            try
            {
                if (isDrop)
                {
                    _selectedRewardIDs.Remove((RewardID)index);

                    return true;
                }

                var length = _selectedRewardIDs.Where(q => (int)q == index)
                    .ToArray()
                    .Length;
                if (length < 1)
                    _selectedRewardIDs.Add((RewardID)index);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSlots()
        {
            try
            {
                if (_rewardContentProps == null ||
                    _rewardContentProps.Length < 1)
                    throw new System.ArgumentNullException("スロットへセットするためにはプロパティが必要");

                var utility = new MainCommonUtility();
                var userData = utility.UserDataSingleton.UserBeanReloaded;
                var slots = userData.pentagramTurnTableInfo.slots.ToList();
                List<UserBean.PentagramTurnTableInfo.Slot> slotsAdd = new List<UserBean.PentagramTurnTableInfo.Slot> ();
                var rewardsUtility = new MainRewardsUtility();
                foreach (var rewardContentProp in rewardsUtility.MergeRewards(_rewardContentProps.Where(q => _selectedRewardIDs.Any(selectId => selectId.Equals(q.rewardID))).ToArray()))
                {
                    switch (rewardContentProp.rewardType)
                    {
                        case ClearRewardType.AddShikigami:
                            slotsAdd.Add(rewardsUtility.GetSlotUserBean(rewardContentProp));

                            break;
                        case ClearRewardType.EnhanceShikigami:
                            for (var i = 0; i < slots.Count; i++)
                            {
                                if (slots[i].slotId == rewardContentProp.detailProp.afterShikigamiInfoProp.slotId)
                                    slots[i] = rewardsUtility.GetSlotUserBean(rewardContentProp);
                            }

                            break;
                        case ClearRewardType.EnhancePlayer:
                            // ChatGPT-4o
                            for (int i = 0; i < slots.Count; i++)
                            {
                                // 条件に一致する場合に処理を実行
                                if (rewardContentProp.detailProp.playerInfoProp.afterPlayerInfoProps
                                    .Any(p => p.slotId == slots[i].slotId &&
                                    p.mainSkills.Any(ms => !ms.emphasisType.Equals(EmphasisType.Neutral))))
                                {
                                    // スロット情報を更新
                                    slots[i] = rewardsUtility.GetSlotUserBeanPlayerInfo(rewardContentProp);
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
                slots.AddRange(slotsAdd);
                userData.pentagramTurnTableInfo.slots = slots.OrderBy(q => q.slotId).ToArray();
                if (!utility.UserDataSingleton.SetAndSaveUserBean(userData))
                    throw new System.Exception("SetAndSaveUserBean");

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
    /// レベルのオーナー
    /// インターフェース
    /// </summary>
    public interface ILevelOwner
    {
        /// <summary>
        /// クリア報酬のコンテンツのプロパティを取得
        /// </summary>
        /// <returns>クリア報酬のコンテンツのプロパティ</returns>
        public RewardContentProp[] GetRewardContentProps();
        /// <summary>
        /// リワードIDを追加／解除
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="isDrop">ドロップ（解除）するか</param>
        /// <returns>成功／失敗</returns>
        public bool AddRewardID(int index, bool isDrop=false);
        /// <summary>
        /// スロットへセット
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetSlots();
    }

    /// <summary>
    /// 式神と画像を連携する情報
    /// </summary>
    [System.Serializable]
    public struct ShikigamiInfoSplitesProp
    {
        /// <summary>式神キャラクターID</summary>
        public ShikigamiCharacterID shikigamiCharacterID;
        /// <summary>イメージ</summary>
        public Sprite image;
    }

    /// <summary>
    /// 強化モード
    /// </summary>
    public enum EnhanceLevel
    {
        /// <summary>ノーマル相当</summary>
        Mode1 = 1,
        /// <summary>レア相当</summary>
        Mode2 = 2,
        /// <summary>Sレア相当</summary>
        Mode3 = 3,
    }

    [System.Serializable]
    /// <summary>
    /// 強化プロパティ
    /// </summary>
    public struct EnhanceProp
    {
        /// <summary>強化モード</summary>
        public EnhanceLevel level;
        /// <summary>魂の経験値</summary>
        public int soulMoney;
    }

    /// <summary>
    /// サブスキルシナジー
    /// </summary>
    [System.Serializable]
    public struct SubSkillsSynergy
    {
        /// <summary>サブスキルタイプ</summary>
        public SubSkillType subSkillType;
        /// <summary>サブスキルタグ</summary>
        public SubSkillTag[] subSkillTags;
    }
}
