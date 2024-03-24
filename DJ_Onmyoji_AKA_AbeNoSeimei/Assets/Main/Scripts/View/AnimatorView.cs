using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// アニメータ
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimatorView : MonoBehaviour, IAnimatorView
    {
        /// <summary>アニメータ</summary>
        [SerializeField] private Animator animator;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        public bool SetTrigger(ParametersOfAnim parametersOfAnim)
        {
            try
            {
                animator.SetTrigger($"{parametersOfAnim}");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetBool(ParametersOfAnim parametersOfAnim, bool enabled)
        {
            try
            {
                animator.SetBool($"{parametersOfAnim}", enabled);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// アニメータ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IAnimatorView
    {
        /// <summary>
        /// セットトリガー
        /// ダメージ
        /// </summary>
        /// <param name="parametersOfAnim">アニメータのパラメータ</param>
        /// <returns>成功／失敗</returns>
        public bool SetTrigger(ParametersOfAnim parametersOfAnim);
        /// <summary>
        /// セットブール
        /// ダメージループ
        /// </summary>
        /// <param name="parametersOfAnim">アニメータのパラメータ</param>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetBool(ParametersOfAnim parametersOfAnim, bool enabled);
    }

    /// <summary>
    /// アニメータのパラメータ
    /// </summary>
    public enum ParametersOfAnim
    {
        /// <summary>右手ダウン</summary>
        DamageRight,
        /// <summary>左手ダウン</summary>
        DamageLeft,
        /// <summary>右手ダウンからのループ</summary>
        DamageLoopRight,
        /// <summary>左手ダウンからのループ</summary>
        DamageLoopLeft,
    }
}
