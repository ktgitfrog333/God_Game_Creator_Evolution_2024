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
    /// ダンスホールビュー
    /// コメント
    /// </summary>
    public class DanceHallView : MonoBehaviour
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

        private void OnEnable()
        {
            // エフェクトプールからエフェクトを取得して再生させる
            _effectsPoolModel = _effectUtility.FindOrInstantiateForGetEffectsPoolModel(effectsPoolPrefab);
            System.IDisposable updateAsObservable = this.UpdateAsObservable().Subscribe(_ => {});
            _isCompletedObservableDisposable?.Dispose(); // 前のIsCompletedのObserverを破棄
            _isCompletedObservableDisposable = _effectsPoolModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(x =>
                {
                    _danceShockwave = _effectsPoolModel.GetDanceShockwave();
                    updateAsObservable.Dispose();
                    Transform transform = this.transform;
                    updateAsObservable = this.UpdateAsObservable()
                        .Subscribe(_ => _danceShockwave.position = transform.position);
                    _danceShockwave.gameObject.SetActive(true);
                    var particleSystems = _danceShockwave.GetComponentsInChildren<ParticleSystem>();
                    foreach (var particleSystem in particleSystems)
                        particleSystem.Play();
                    Observable.FromCoroutine(() => _effectsPoolModel.WaitForAllParticlesToStop(particleSystems))
                        .Subscribe(_ => _danceShockwave.gameObject.SetActive(false))
                        .AddTo(gameObject);
                });
        }

        private void OnDisable()
        {
            _isCompletedObservableDisposable?.Dispose(); // IsCompletedのObserverを破棄
        }
    }
}
