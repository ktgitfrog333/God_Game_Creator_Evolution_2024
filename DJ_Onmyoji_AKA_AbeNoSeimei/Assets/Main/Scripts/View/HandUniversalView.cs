using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 手関係のパーツ
    /// ビュー
    /// </summary>
    public abstract class HandUniversalView : MonoBehaviour
    {
        /// <summary>終了時間</summary>
        [SerializeField] protected float duration = .65f;
        /// <summary>DOTweenアニメーション管理</summary>
        private Tweener _tweener;
        /// <summary>RectTransform</summary>
        [SerializeField] private RectTransform rectTransform;
        /// <summary>ゴール（To）位置</summary>
        [SerializeField] protected Vector2 endPosition = new Vector2(-82f, -61f);

        protected virtual void Reset()
        {
            rectTransform = transform as RectTransform;
        }

        private void OnEnable()
        {
            if (_tweener != null)
                _tweener.Restart();
        }

        private void Start()
        {
            _tweener = PlaySwipeAnimation(duration, rectTransform, endPosition);
        }

        /// <summary>
        /// スワイプアニメーションを再生
        /// </summary>
        /// <param name="duration">終了時間</param>
        /// <param name="rectTransform">RectTransform</param>
        /// <param name="endPosition">ゴール（To）位置</param>
        /// <returns>DOTweenアニメーション管理</returns>
        protected abstract Tweener PlaySwipeAnimation(float duration, RectTransform rectTransform, Vector2 endPosition);
    }
}
