using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// ペンダグラムターンテーブル情報
    /// </summary>
    [System.Serializable]
    public struct PentagramTurnTableInfo
    {
        /// <summary>スロット</summary>
        public Slot[] slots;

        /// <summary>
        /// スロット
        /// </summary>
        [System.Serializable]
        public struct Slot
        {
            /// <summary>プロパティ</summary>
            public Prop prop;
            /// <summary>ステート</summary>
            public State state;

            /// <summary>
            /// プロパティ
            /// </summary>
            [System.Serializable]
            public struct Prop
            {
                /// <summary>スロット番号</summary>
                public SlotId slotId;
                /// <summary>式神の情報</summary>
                public ShikigamiInfo shikigamiInfo;
                /// <summary>オブジェクトID</summary>
                public int instanceId;
            }

            /// <summary>
            /// ステート
            /// </summary>
            [System.Serializable]
            public struct State
            {
                
            }
        }
    }
}
