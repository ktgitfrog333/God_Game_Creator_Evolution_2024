using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 左手
    /// ビュー
    /// </summary>
    public class HandLeftView : HandUniversalView
    {
        protected override Tweener PlaySwipeAnimation(float duration, RectTransform rectTransform, Vector2 endPosition)
        {
            return rectTransform.DOAnchorPos(endPosition, duration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
