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
    /// 魔力弾（ラップ用）
    /// ビュー
    /// </summary>
    public class WrapBulletView : OnmyoBulletView
    {
        /// <summary>エフェクトプールプレハブ</summary>
        [SerializeField] private Transform effectsPoolPrefab;
        /// <summary>エフェクトユーティリティ</summary>
        private EffectUtility _effectUtility = new EffectUtility();
        /// <summary>エフェクトプールモデル</summary>
        private EffectsPoolModel _effectsPoolModel = new EffectsPoolModel();
        /// <summary>ダンスの衝撃波</summary>
        private Transform _danceShockwave;
        /// <summary>エフェクトプール生成済みか監視</summary>
        private System.IDisposable _isCompletedObservableDisposable;
        /// <summary>エフェクトサイズ</summary>
        public float effectSize;
        /// <summary>パーティクルシステムのメインモジュール</summary>
        private ParticleSystem.MainModule _mainModule;

        public void Explosion()
        {
            // エフェクトプールからエフェクトを取得して再生させる
            _effectsPoolModel = _effectUtility.FindOrInstantiateForGetEffectsPoolModel(effectsPoolPrefab);
            _danceShockwave = _effectsPoolModel.GetDanceShockwave();
            Transform transform = this.transform;
            _danceShockwave.position = transform.position;
            _danceShockwave.gameObject.SetActive(true);
            var particleSystems = _danceShockwave.GetComponentsInChildren<ParticleSystem>();
            foreach (var particleSystem in particleSystems)
            {
                _mainModule = particleSystem.main;
                _mainModule.startSize = effectSize * 3.0f;
                particleSystem.Play();
            }
            Observable.FromCoroutine(() => _effectsPoolModel.WaitForAllParticlesToStop(particleSystems))
                .Subscribe(_ => _danceShockwave.gameObject.SetActive(false))
                .AddTo(gameObject);
        }

        private void OnDisable()
        {
            _isCompletedObservableDisposable?.Dispose(); // IsCompletedのObserverを破棄
        }
    }
}
