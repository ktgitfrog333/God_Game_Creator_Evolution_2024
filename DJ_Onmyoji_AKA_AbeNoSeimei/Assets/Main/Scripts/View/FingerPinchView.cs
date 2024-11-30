using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// つまむ右手
    /// ビュー
    /// </summary>
    public class FingerPinchView : HandUniversalView
    {
        /// <summary>スイングの角度の強さ</summary>
        [SerializeField] private Vector3 strength = new Vector3(0, 0, 15f);
        /// <summary>振動の回数</summary>
        [SerializeField] private int vibrato = 3;

        protected override void Reset()
        {
            base.Reset();
            endPosition = Vector3.zero;
            duration = 1.25f;
        }

        protected override Tweener PlaySwipeAnimation(float duration, RectTransform rectTransform, Vector2 endPosition)
        {
            return rectTransform.DOShakeRotation(duration, strength, vibrato, randomness: 90, fadeOut: true)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}
