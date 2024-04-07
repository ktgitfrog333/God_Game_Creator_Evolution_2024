using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 敵
    /// ビュー
    /// </summary>
    public class EnemyView : MonoBehaviour, IEnemyView
    {
        /// <summary>ボディスプライトのビュー</summary>
        [SerializeField] private BodySpriteView bodySpriteView;
        /// <summary>終了時間</summary>
        [SerializeField] float[] durations = { 1.25f, 1.25f };
        /// <summary>スケールのパターン</summary>
        [SerializeField] float[] scales = { 1.1f, .9f };
        /// <summary>アニメータが存在するか</summary>
        public bool IsFoundAnimator => animatorView != null;
        /// <summary>アニメータのビュー</summary>
        [SerializeField] private AnimatorView animatorView;

        private void Reset()
        {
            bodySpriteView = GetComponentInChildren<BodySpriteView>();
            if (GetComponentInChildren<AnimatorView>() != null)
                animatorView = GetComponentInChildren<AnimatorView>();
        }

        private void OnEnable()
        {
            if (bodySpriteView != null)
                if (!bodySpriteView.PlayScalingLoopAnimation(durations, scales))
                    Debug.LogError("PlayScalingLoopAnimation");
        }

        public bool PlayWalkingAnimation(float moveSpeed)
        {
            return animatorView.SetFloat(ParametersOfAnim.MoveSpeed, moveSpeed);
        }
    }

    /// <summary>
    /// 敵
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IEnemyView
    {
        /// <summary>
        /// 歩くアニメーションを再生
        /// </summary>
        /// <param name="moveSpeed">移動速度</param>
        /// <returns>成功／失敗</returns>
        public bool PlayWalkingAnimation(float moveSpeed);
    }
}
