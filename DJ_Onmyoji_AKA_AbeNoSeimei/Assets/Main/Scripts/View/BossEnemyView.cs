using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ボス敵
    /// ビュー
    /// </summary>
    public class BossEnemyView : EnemyView, IBossEnemyView
    {
        /// <summary>アニメータのビュー</summary>
        [SerializeField] private AnimatorView animatorView;
        /// <summary>キング青行灯のプロパティ</summary>
        [SerializeField] private KingAoandonProp kingAoandonProp = new KingAoandonProp()
        {
            durations = new float[]
            {
                2f,
            },
        };
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;

        private void Reset()
        {
            animatorView = GetComponentInChildren<AnimatorView>();
            foreach (Transform child in transform)
                if (child.name.Equals("TargetPointEntrance"))
                    kingAoandonProp.targetPointEntrance = child;
        }

        public IEnumerator MovePointEntrance(System.IObserver<bool> observer, int bossDirectionPhase)
        {
            switch ((BossDirectionPhase)bossDirectionPhase)
            {
                case BossDirectionPhase.Entrance:
                    Transform.DOMove(kingAoandonProp.targetPointEntrance.position, kingAoandonProp.durations[0])
                        .OnComplete(() => observer.OnNext(true));

                    break;
                default:
                    // それ以外
                    observer.OnNext(false);

                    break;
            }
            yield return null;
        }

        public bool Movement(int bossActionPhase)
        {
            try
            {
                switch ((BossActionPhase)bossActionPhase)
                {
                    case BossActionPhase.Idle:
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopRight, false))
                            throw new System.Exception("SetBool");
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopLeft, false))
                            throw new System.Exception("SetBool");

                        break;
                    case BossActionPhase.Attack:
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopRight, false))
                            throw new System.Exception("SetBool");
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopLeft, false))
                            throw new System.Exception("SetBool");

                        break;
                    case BossActionPhase.DamageRight:
                        if (!animatorView.SetTrigger(ParametersOfAnim.DamageRight))
                            throw new System.Exception("SetTrigger");
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopRight, true))
                            throw new System.Exception("SetBool");

                        break;
                    case BossActionPhase.DamageLeft:
                        if (!animatorView.SetTrigger(ParametersOfAnim.DamageLeft))
                            throw new System.Exception("SetTrigger");
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopLeft, true))
                            throw new System.Exception("SetBool");

                        break;
                    case BossActionPhase.DamageAll:
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopRight, true))
                            throw new System.Exception("SetBool");
                        if (!animatorView.SetBool(ParametersOfAnim.DamageLoopLeft, true))
                            throw new System.Exception("SetBool");

                        break;
                    default:
                        // それ以外
                        break;
                }

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
    /// ボス敵
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IBossEnemyView
    {
        /// <summary>
        /// 指定の位置へ登場する演出
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="bossDirectionPhase">ボス演出フェーズ</papram>
        /// <returns>成功／失敗</returns>
        public IEnumerator MovePointEntrance(System.IObserver<bool> observer, int bossDirectionPhase);
        /// <summary>
        /// アニメーション実行
        /// </summary>
        /// <param name="bossActionPhase">ボス行動フェーズ</param>
        /// <returns>成功／失敗</returns>
        public bool Movement(int bossActionPhase);
    }

    /// <summary>
    /// キング青行灯のプロパティ
    /// </summary>
    [System.Serializable]
    public struct KingAoandonProp
    {
        /// <summary>
        /// ターゲット座標
        /// </summary>
        public Transform targetPointEntrance;
        /// <summary>アニメーション終了時間</summary>
        public float[] durations;
    }
}
