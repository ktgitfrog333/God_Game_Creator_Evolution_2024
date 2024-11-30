using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Utility;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// スポーンの抽象クラス
    /// </summary>
    public abstract class SpawnModel : MonoBehaviour
    {
        /// <summary>オブジェクトプール</summary>
        [Tooltip("オブジェクトプール")]
        [SerializeField] protected Transform objectsPoolPrefab;
        /// <summary>クローンオブジェクトを生成する時間間隔（秒）</summary>
        [Tooltip("クローンオブジェクトを生成する時間間隔（秒）")]
        [SerializeField] protected float instanceRateTimeSec = .5f;
        /// <summary>スポーンのユーティリティ</summary>
        protected SpawnUtility _spawnUtility = new SpawnUtility();
        /// <summary>オブジェクトプール</summary>
        protected ObjectsPoolModel _poolModel;

        protected virtual void Start()
        {
            _poolModel = _spawnUtility.FindOrInstantiateForGetObjectsPoolModel(objectsPoolPrefab);
            _poolModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        if (!InstanceCloneObjects(instanceRateTimeSec, _poolModel))
                            Debug.LogError("InstanceCloneObjects");
                    }
                });
        }

        /// <summary>
        /// ターゲットが生成されるまで待機
        /// </summary>
        /// <param name="observer">トランスフォーム</param>
        /// <returns>コルーチン</returns>
        protected IEnumerator WaitForTarget(System.IObserver<Transform> observer)
        {
            Transform target = null;
            while (target == null)
            {
                var obj = GameObject.FindGameObjectWithTag(ConstTagNames.TAG_NAME_PLAYER);
                if (obj != null)
                    target = obj.transform;
                yield return null;
            }
            observer.OnNext(target);
        }

        /// <summary>
        /// オブジェクトを生成
        /// </summary>
        /// <param name="instanceRateTimeSec">オブジェクトを生成する時間間隔（秒）</param>
        /// <param name="objectsPoolModel">オブジェクトプール</param>
        /// <returns>成功／失敗</returns>
        protected abstract bool InstanceCloneObjects(float instanceRateTimeSec, ObjectsPoolModel objectsPoolModel);
    }
}
