using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Universal.Common;

namespace Main.Model
{
    public class ObjectsPoolModel : MonoBehaviour, IObjectsPoolModel
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>プール数の上限</summary>
        [Tooltip("プール数の上限")]
        [SerializeField] private int countLimit;
        /// <summary>プール完了</summary>
        public IReactiveProperty<bool> IsCompleted { get; private set; } = new BoolReactiveProperty();
        /// <summary>魔力弾のプレハブ</summary>
        [Tooltip("魔力弾のプレハブ")]
        [SerializeField] private Transform onmyoBulletPrefab;
        /// <summary>魔力弾配列</summary>
        private List<OnmyoBulletModel> _onmyoBulletModels = new List<OnmyoBulletModel>();
        /// <summary>魔力弾（ラップ用）のプレハブ</summary>
        [SerializeField] private Transform wrapBulletPrefab;
        /// <summary>魔力弾（ラップ用）配列</summary>
        private List<WrapBulletModel> _wrapBulletModels = new List<WrapBulletModel>();
        /// <summary>ダンスホールのプレハブ</summary>
        [SerializeField] private Transform danceHallPrefab;
        /// <summary>ダンスホール配列</summary>
        private List<DanceHallModel> _danceHallModels = new List<DanceHallModel>();
        /// <summary>敵のプレハブ</summary>
        [Tooltip("敵のプレハブ")]
        [SerializeField] private Transform enemyPrefab;
        /// <summary>敵配列</summary>
        private List<EnemyModel> _enemyModels = new List<EnemyModel>();

        public OnmyoBulletModel GetOnmyoBulletModel()
        {
            return GetInactiveComponent(_onmyoBulletModels, onmyoBulletPrefab, _transform);
        }

        public WrapBulletModel GetWrapBulletModel()
        {
            return GetInactiveComponent(_wrapBulletModels, wrapBulletPrefab, _transform);
        }

        public DanceHallModel GetDanceHallModel()
        {
            return GetInactiveComponent(_danceHallModels, danceHallPrefab, _transform);
        }

        public EnemyModel GetEnemyModel()
        {
            return GetInactiveComponent(_enemyModels, enemyPrefab, _transform);
        }

        private void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            countLimit = adminDataSingleton.AdminBean.ObjectsPoolModel.countLimit;
            if (_transform == null)
                _transform = transform;
            Debug.Log("プール開始");
            for (int i = 0; i < countLimit; i++)
            {
                // プレハブを生成してプールする
                _onmyoBulletModels.Add(GetClone(onmyoBulletPrefab, _transform).GetComponent<OnmyoBulletModel>());
                _wrapBulletModels.Add(GetClone(wrapBulletPrefab, _transform).GetComponent<WrapBulletModel>());
                _danceHallModels.Add(GetClone(danceHallPrefab, _transform).GetComponent<DanceHallModel>());
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
        /// 魔力弾（ラップ用）を取り出す
        /// </summary>
        /// <returns>魔力弾（ラップ用）</returns>
        public WrapBulletModel GetWrapBulletModel();
        /// <summary>
        /// ダンスホールを取り出す
        /// </summary>
        /// <returns>ダンスホール</returns>
        public DanceHallModel GetDanceHallModel();
        /// <summary>
        /// 敵を取り出す
        /// </summary>
        /// <returns>敵</returns>
        public EnemyModel GetEnemyModel();
    }
}
