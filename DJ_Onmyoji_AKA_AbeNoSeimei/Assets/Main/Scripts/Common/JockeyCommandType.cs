using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// ジョッキーコマンドタイプ
    /// </summary>
    public enum JockeyCommandType
    {
        /// <summary>技無し（リセット）</summary>
        None = -1,
        /// <summary>ホールド</summary>
        Hold,
        /// <summary>スクラッチ</summary>
        Scratch,
        /// <summary>バックスピン</summary>
        BackSpin,
        /// <summary>スリップループ</summary>
        SlipLoop,
        /// <summary>スリップループ</summary>
        SlipLoopEnd,
    }    
}
