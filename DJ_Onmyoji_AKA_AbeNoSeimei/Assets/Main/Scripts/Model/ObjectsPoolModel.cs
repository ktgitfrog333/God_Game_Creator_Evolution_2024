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
        /// <summary>魔力弾</summary>
        [SerializeField] private Transform OnmyoBulletPrefab;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>プール数の上限</summary>
        [SerializeField] private int countLimit = 1000;
        /// <summary>プール完了</summary>
        public IReactiveProperty<bool> IsCompleted { get; private set; } = new BoolReactiveProperty();
        /// <summary>魔力弾配列</summary>
        private List<OnmyoBulletModel> _onmyoBulletModels = new List<OnmyoBulletModel>();

        public OnmyoBulletModel GetOnmyoBulletModel()
        {
            var bullets = _onmyoBulletModels.Where(q => !q.isActiveAndEnabled)
                .Select(q => q)
                .ToArray();
            if (bullets.Length < 1)
            {
                Debug.LogWarning("プレハブ新規生成");
                var newBullet = GetBullet(OnmyoBulletPrefab, _transform).GetComponent<OnmyoBulletModel>();
                return newBullet;
            }
            else
                return bullets[0];
        }

        private void Start()
        {
            if (_transform == null)
                _transform = transform;
            Debug.Log("プール開始");
            for (int i = 0; i < countLimit; i++)
            {
                // プレハブを生成してプールする
                _onmyoBulletModels.Add(GetBullet(OnmyoBulletPrefab, _transform).GetComponent<OnmyoBulletModel>());
            }
            Debug.Log("プール完了");
            IsCompleted.Value = true;
        }

        /// <summary>
        /// 魔力弾を取得
        /// </summary>
        /// <param name="onmyoBulletPrefab">魔力弾プレハブ</param>
        /// <param name="parent">親</param>
        /// <returns>魔力弾</returns>
        private Transform GetBullet(Transform onmyoBulletPrefab, Transform parent)
        {
            return Instantiate(onmyoBulletPrefab, parent);
        }
    }

    public interface IObjectsPoolModel
    {
        /// <summary>
        /// 魔力弾を取り出す
        /// </summary>
        /// <returns>魔力弾</returns>
        public OnmyoBulletModel GetOnmyoBulletModel();
    }
}
