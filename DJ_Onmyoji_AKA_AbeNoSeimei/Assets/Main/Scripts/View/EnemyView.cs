using Effect.Model;
using Effect.Utility;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
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
        /// <summary>エフェクトプールプレハブ</summary>
        [SerializeField] private Transform effectsPoolPrefab;
        /// <summary>エフェクトユーティリティ</summary>
        private EffectUtility _effectUtility = new EffectUtility();
        /// <summary>エフェクトプールモデル</summary>
        private EffectsPoolModel _effectsPoolModel = new EffectsPoolModel();
        /// <summary>ダンスの衝撃波</summary>
        private Transform _hitEffect;

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

        private void Start()
        {
            // エフェクトプールからエフェクトを取得して再生させる
            _effectsPoolModel = _effectUtility.FindOrInstantiateForGetEffectsPoolModel(effectsPoolPrefab);
            _effectsPoolModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    _hitEffect = _effectsPoolModel.GetHitEffect();
                });
        }

        public bool PlayWalkingAnimation(float moveSpeed)
        {
            return animatorView.SetFloat(ParametersOfAnim.MoveSpeed, moveSpeed);
        }

        public bool PlayHitEffect()
        {
            try
            {
                if (_hitEffect == null)
                    return true;

                var particleSystems = _hitEffect.GetComponentsInChildren<ParticleSystem>();
                _hitEffect.position = transform.position;
                _hitEffect.gameObject.SetActive(true);
                foreach (var particleSystem in particleSystems)
                    particleSystem.Play();
                Observable.FromCoroutine(() => _effectsPoolModel.WaitForAllParticlesToStop(particleSystems))
                    .Subscribe(_ => _hitEffect.gameObject.SetActive(false))
                    .AddTo(gameObject);

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
        /// <summary>
        /// ヒットエフェクトを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayHitEffect();
    }
}
