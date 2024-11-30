using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// イコライザー操作のガイド用UI
    /// ビュー
    /// </summary>
    public class GuideUITheEqualizerView : GuideUITheUniversalView
    {
        protected override void Reset()
        {
            base.Reset();
            var structs = guideUITheUniversalStructs;
            foreach (var @struct in structs.Select((p, i) => new { Content = p, Index = i }))
            {
                switch (@struct.Index)
                {
                    case 0:
                        structs[@struct.Index].duration = 1f;

                        break;
                    case 1:
                        structs[@struct.Index].duration = 2.5f;

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
