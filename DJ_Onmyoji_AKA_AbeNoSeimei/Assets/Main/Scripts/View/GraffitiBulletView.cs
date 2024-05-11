using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 魔力弾（グラフィティ用）
    /// ビュー
    /// </summary>
    public class GraffitiBulletView : MonoBehaviour, IGraffitiView
    {
        /// <summary>アニメータが存在するか</summary>
        public bool IsFoundAnimator => animatorView != null;
        /// <summary>アニメータのビュー</summary>
        [SerializeField] private AnimatorView animatorView;

        private void Reset()
        {
            if (GetComponentInChildren<AnimatorView>() != null)
                animatorView = GetComponentInChildren<AnimatorView>();

        }

        public bool PlayWalkingAnimation(float moveSpeed)
        {
            return animatorView.SetFloat(ParametersOfAnim.MoveSpeed, moveSpeed);
        }
    }

    /// <summary>
    /// 魔力弾（グラフィティ用）
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IGraffitiView : IMobView
    {

    }
}
