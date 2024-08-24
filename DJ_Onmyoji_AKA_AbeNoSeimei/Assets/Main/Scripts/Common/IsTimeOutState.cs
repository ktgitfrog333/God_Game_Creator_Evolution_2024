using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Common
{
    /// <summary>
    /// タイムアウト状態
    /// </summary>
    public enum IsTimeOutState
    {
        /// <summary>カウントダウン</summary>
        Playing,
        /// <summary>タイムアウト</summary>
        TimeOut,
        /// <summary>無限（ボス登場など）</summary>
        Infinite,
    }
}
