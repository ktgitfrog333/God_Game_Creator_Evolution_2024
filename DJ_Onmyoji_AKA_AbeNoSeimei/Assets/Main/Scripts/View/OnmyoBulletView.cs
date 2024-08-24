using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 魔力弾
    /// ビュー
    /// </summary>
    public class OnmyoBulletView : MonoBehaviour, IOnmyoBulletView
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
    /// 魔力弾
    /// ビュー
    /// </summary>
    public interface IOnmyoBulletView : IMobView
    {

    }
}
