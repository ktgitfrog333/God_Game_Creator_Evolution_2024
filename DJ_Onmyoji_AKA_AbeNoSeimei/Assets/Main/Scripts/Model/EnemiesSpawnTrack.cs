using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Main.Model
{
    /// <summary>
    /// 敵をスポーンするトラックアセット
    /// </summary>
    [TrackColor(0.855f, 0.903f, 0.87f)]
    [TrackClipType(typeof(EnemiesSpawnClip))]
    [TrackBindingType(typeof(EnemiesSpawnModel))]
    public class EnemiesSpawnTrack : TrackAsset
    {
    }
}
