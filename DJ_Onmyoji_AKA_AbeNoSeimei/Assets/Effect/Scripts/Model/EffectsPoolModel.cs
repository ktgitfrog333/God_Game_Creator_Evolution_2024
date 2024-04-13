using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Effect.Model
{
    /// <summary>
    /// エフェクトプール
    /// モデル
    /// </summary>
    public class EffectsPoolModel : MonoBehaviour, IEffectsPoolModel
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>プール数の上限</summary>
        [Tooltip("プール数の上限")]
        [SerializeField] private int countLimit = 30;
        /// <summary>プール完了</summary>
        public IReactiveProperty<bool> IsCompleted { get; private set; } = new BoolReactiveProperty();
        /// <summary>ダンスの衝撃波</summary>
        [SerializeField] private Transform danceShockwavePrefab;
        /// <summary>ダンスの衝撃波</summary>
        private List<Transform> _danceShockwave = new List<Transform>();
        /// <summary>ラップの爆発</summary>
        [SerializeField] private Transform shikigamiWrapExplosionPrefab;
        /// <summary>ラップの爆発</summary>
        private List<ParticleSystem> _shikigamiWrapExplosion = new List<ParticleSystem>();

        private void Start()
        {
            Debug.Log("プール開始");
            for (int i = 0; i < countLimit; i++)
            {
                _shikigamiWrapExplosion.Add(InstancePrefabDisabledAndGetClone(shikigamiWrapExplosionPrefab, Transform).GetComponent<ParticleSystem>());
                _danceShockwave.Add(InstancePrefabDisabledAndGetClone(danceShockwavePrefab, Transform).GetComponent<Transform>());
            }
            Debug.Log("プール完了");
            IsCompleted.Value = true;
        }

        /// <summary>
        /// プレハブを元に無効状態で生成
        /// ゲームオブジェクトを取得
        /// </summary>
        /// <param name="prefab">プレハブ</param>
        /// <param name="parent">親オブジェクト</param>
        /// <returns>生成後のオブジェクト</returns>
        private GameObject InstancePrefabDisabledAndGetClone(Transform prefab, Transform parent)
        {
            var obj = Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);

            return obj.gameObject;
        }

        public ParticleSystem GetShikigamiWrapExplosion()
        {
            var inactiveComponents = _shikigamiWrapExplosion.Where(q => !q.transform.gameObject.activeSelf).ToArray();
            if (inactiveComponents.Length < 1)
            {
                Debug.LogWarning("プレハブ新規生成");
                var obj = Instantiate(shikigamiWrapExplosionPrefab, Transform);
                _shikigamiWrapExplosion.Add(obj.GetComponent<ParticleSystem>());
                return obj.GetComponent<ParticleSystem>();
            }
            else
                return inactiveComponents[0];
        }

        public Transform GetDanceShockwave()
        {
            var inactiveComponents = _danceShockwave.Where(q => !q.transform.gameObject.activeSelf).ToArray();
            if (inactiveComponents.Length < 1)
            {
                Debug.LogWarning("プレハブ新規生成");
                var obj = Instantiate(danceShockwavePrefab, Transform);
                _danceShockwave.Add(obj.GetComponent<Transform>());
                return obj.GetComponent<Transform>();
            }
            else
                return inactiveComponents[0];
        }
    }

    /// <summary>
    /// エフェクトプール
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IEffectsPoolModel
    {
        /// <summary>
        /// 式神ラップ用の爆発エフェクトを取得
        /// </summary>
        /// <returns>パーティクルシステム</returns>
        public ParticleSystem GetShikigamiWrapExplosion();
        /// <summary>
        /// ダンスの衝撃波のエフェクトを取得
        /// </summary>
        /// <returns>トランスフォーム</returns>
        public Transform GetDanceShockwave();
    }
}
