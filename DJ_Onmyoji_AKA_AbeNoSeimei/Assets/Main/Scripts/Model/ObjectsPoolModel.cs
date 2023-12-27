using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Main.Model
{
    public class ObjectsPoolModel : MonoBehaviour, IObjectsPoolModel
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>プール数の上限</summary>
        [SerializeField] private int countLimit = 100;
        /// <summary>プール完了</summary>
        public IReactiveProperty<bool> IsCompleted { get; private set; } = new BoolReactiveProperty();
        /// <summary>魔力弾</summary>
        [SerializeField] private Transform onmyoBulletPrefab;
        /// <summary>魔力弾配列</summary>
        private List<OnmyoBulletModel> _onmyoBulletModels = new List<OnmyoBulletModel>();
        /// <summary>敵</summary>
        [SerializeField] private Transform enemyPrefab;
        /// <summary>敵配列</summary>
        private List<EnemyModel> _enemyModels = new List<EnemyModel>();

        public OnmyoBulletModel GetOnmyoBulletModel()
        {
            return GetInactiveComponent(_onmyoBulletModels, onmyoBulletPrefab, _transform);
        }

        public EnemyModel GetEnemyModel()
        {
            return GetInactiveComponent(_enemyModels, enemyPrefab, _transform);
        }

        private void Start()
        {
            if (_transform == null)
                _transform = transform;
            Debug.Log("プール開始");
            for (int i = 0; i < countLimit; i++)
            {
                // プレハブを生成してプールする
                _onmyoBulletModels.Add(GetClone(onmyoBulletPrefab, _transform).GetComponent<OnmyoBulletModel>());
                _enemyModels.Add(GetClone(enemyPrefab, _transform).GetComponent<EnemyModel>());
            }
            Debug.Log("プール完了");
            IsCompleted.Value = true;
        }

        /// <summary>
        /// プール内のクローンオブジェクトを取得
        /// </summary>
        /// <param name="cloneObject">プレハブ</param>
        /// <param name="parent">親</param>
        /// <returns>クローンオブジェクト</returns>
        private Transform GetClone(Transform cloneObject, Transform parent)
        {
            return Instantiate(cloneObject, parent);
        }

        private T GetInactiveComponent<T>(List<T> components, Transform prefab, Transform parent) where T : MonoBehaviour
        {
            var inactiveComponents = components.Where(q => !q.isActiveAndEnabled).ToArray();
            if (inactiveComponents.Length < 1)
            {
                Debug.LogWarning("プレハブ新規生成");
                var newComponent = GetClone(prefab, parent).GetComponent<T>();
                components.Add(newComponent);
                return newComponent;
            }
            else
            {
                return inactiveComponents[0];
            }
        }
    }

    public interface IObjectsPoolModel
    {
        /// <summary>
        /// 魔力弾を取り出す
        /// </summary>
        /// <returns>魔力弾</returns>
        public OnmyoBulletModel GetOnmyoBulletModel();
        /// <summary>
        /// 敵を取り出す
        /// </summary>
        /// <returns>敵</returns>
        public EnemyModel GetEnemyModel();
    }
}
