using Effect.Model;
using Effect.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// プレイヤー
    /// </summary>
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        /// <summary>エフェクトプールプレハブ</summary>
        [SerializeField] private Transform effectsPoolPrefab;
        /// <summary>エフェクトユーティリティ</summary>
        private EffectUtility _effectUtility = new EffectUtility();
        /// <summary>エフェクトプールモデル</summary>
        private EffectsPoolModel _effectsPoolModel = new EffectsPoolModel();
        /// <summary>プレイヤーのヒットエフェクト</summary>
        private Transform _hitEffect;
        /// <summary>プレイヤーがやられた時のエフェクト</summary>
        private Transform _playerDownEffect;

        private void Start()
        {
            // エフェクトプールからエフェクトを取得して再生させる
            _effectsPoolModel = _effectUtility.FindOrInstantiateForGetEffectsPoolModel(effectsPoolPrefab);
            _effectsPoolModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    _hitEffect = _effectsPoolModel.GetHitEffect();
                    //_playerDownEffect = _effectsPoolModel.GetPlayerDownEffect();
                });
        }

        /// <summary>
        /// ヒットエフェクトを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayHitEffect()
        {
            try
            {
                if (_hitEffect == null)
                    return false;

                var particleSystems = _hitEffect.GetComponentsInChildren<ParticleSystem>();
                _hitEffect.gameObject.SetActive(true);
                _hitEffect.position = transform.position;
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

        /// <summary>
        /// プレイヤーがやられた時のエフェクトを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayEnemyDownEffect()
        {
            try
            {
                if (_playerDownEffect == null)
                    return false;

                _playerDownEffect.gameObject.SetActive(true);
                _playerDownEffect.position = transform.position;

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
    /// ビュー
    /// プレイヤー
    /// インターフェース
    /// </summary>
    public interface IPlayerView
    {
        /// <summary>
        /// ヒットエフェクトを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayHitEffect();
        /// <summary>
        /// 敵がやられた時のエフェクトを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayEnemyDownEffect();
    }
}
