using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using UnityEngine;
using Universal.Common;

namespace Main.Utility
{
    /// <summary>
    /// 式神タイプ別パラメータ管理
    /// 式神情報を元に設定値の内容を取得する
    /// </summary>
    /// <see href="https://www.notion.so/b17775a9a3b34f27a911c50454d5165e?pvs=4"/>
    /// <see href="https://www.notion.so/a72678495bbf42b2af5f88bcfcc29198?pvs=4"/>
    public class ShikigamiParameterUtility : IShikigamiParameterUtility
    {
        private AdminDataSingleton _adminDataSingleton;
        public AdminDataSingleton AdminDataSingleton => _adminDataSingleton != null ? _adminDataSingleton : GetAdminDataSingleton();
        private AdminDataSingleton GetAdminDataSingleton()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            return adminDataSingleton;
        }

        /// <summary>
        /// 時間間隔の取得
        /// </summary>
        /// <param name="shikigamiInfo">式神の情報</param>
        /// <param name="mainSkillType">スキルタイプ</param>
        /// <returns>時間間隔</returns>
        /// <exception cref="System.Exception">メインスキルプロパティが1つもない場合、または指定したスキルタイプのメインスキルプロパティが1つもない場合にスローされます</exception>
        public float GetActionRate(ShikigamiInfo shikigamiInfo, MainSkillType mainSkillType)
        {
            var skillLists = AdminDataSingleton.AdminBean.levelDesign.mainSkillLists;
            if (skillLists.Length < 1)
                throw new System.Exception($"{skillLists.Length}つのメインスキルプロパティから取得できない");

            var array = skillLists.Where(q => ((ShikigamiType)q.shikigamiType).Equals(shikigamiInfo.prop.type) &&
                ((MainSkillType)q.mainSkillType).Equals(mainSkillType) &&
                ((SkillRank)q.skillRank).Equals(GetMainSkillRank(shikigamiInfo, mainSkillType)))
                .Select(q => q.value)
                .ToArray();
            if (array.Length < 1)
                throw new System.Exception($"{skillLists.Length}つのメインスキルプロパティから取得できない[{shikigamiInfo.prop.type}][{mainSkillType}]");

            return array[0];
        }

        /// <summary>
        /// スキルランクの取得
        /// </summary>
        /// <param name="shikigamiInfo">式神の情報</param>
        /// <param name="mainSkillType">スキルタイプ</param>
        /// <returns>スキルランク</returns>
        /// <exception cref="System.Exception">メインスキルプロパティが1つもない場合、または指定したスキルタイプのメインスキルプロパティが1つもない場合にスローされます</exception>
        private SkillRank GetMainSkillRank(ShikigamiInfo shikigamiInfo, MainSkillType mainSkillType)
        {
            var skills = shikigamiInfo.prop.mainSkills;
            if (skills.Length < 1)
                throw new System.Exception($"{skills.Length}つのメインスキルプロパティから取得できない");

            var array = skills.Where(q => q.type.Equals(mainSkillType))
                .Select(q => q.rank)
                .ToArray();
            if (array.Length < 1)
                throw new System.Exception($"{skills.Length}つのメインスキルプロパティから取得できない[{mainSkillType}]");

            return array[0];
        }
    }

    /// <summary>
    /// 式神タイプ別パラメータ管理
    /// 式神情報を元に設定値の内容を取得する
    /// インタフェース
    /// </summary>
    public interface IShikigamiParameterUtility
    {
        /// <summary>
        /// 攻撃間隔の取得
        /// </summary>
        /// <param name="shikigamiInfo">式神の情報</param>
        /// <param name="mainSkillType">メインスキルタイプ</param>
        /// <returns>攻撃間隔</returns>
        public float GetActionRate(ShikigamiInfo shikigamiInfo, MainSkillType mainSkillType);
    }
}
