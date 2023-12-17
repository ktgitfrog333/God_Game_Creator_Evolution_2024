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
        }
    }
}
