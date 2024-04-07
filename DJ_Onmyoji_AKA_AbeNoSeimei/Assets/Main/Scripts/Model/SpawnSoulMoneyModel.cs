using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UnityEngine;
using Main.View;
using UniRx;

namespace Main.Model
{
    /// <summary>
    /// 魂の経験値スポーン
    /// モデル
    /// </summary>
    public class SpawnSoulMoneyModel : MonoBehaviour, ISpawnSoulMoneyModel
    {
        /// <summary>オブジェクトプール</summary>
        [Tooltip("オブジェクトプール")]
        [SerializeField] protected Transform objectsPoolPrefab;
        /// <summary>オブジェクトプールモデル</summary>
        private ObjectsPoolModel _poolModel;
        /// <summary>オブジェクトプールモデル</summary>
        public ObjectsPoolModel PoolModel => _poolModel != null ?
            _poolModel :
            _poolModel = new SpawnUtility().FindOrInstantiateForGetObjectsPoolModel(objectsPoolPrefab);
        /// <summary>経験値獲得をSubject</summary>
        private Subject<SoulMoneyView> onSoulMoneyGeted = new Subject<SoulMoneyView>();
        /// <summary>経験値獲得をSubject</summary>
        public System.IObservable<SoulMoneyView> OnSoulMoneyGeted => onSoulMoneyGeted;

        public bool InstanceCloneObjects(Vector3 position, EnemiesProp enemiesProp)
        {
            try
            {
                var soulMoney = PoolModel.GetSoulMoneyView();
                soulMoney.transform.position = position;
                soulMoney.SetEnemiesProp(enemiesProp);
                soulMoney.IsGeted.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        if (x)
                        {
                            // IsGetedが変更されたときにSubjectを通じて通知
                            onSoulMoneyGeted.OnNext(soulMoney);
                        }
                    });
                soulMoney.gameObject.SetActive(true);

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
    /// 魂の経験値スポーン
    /// モデル
    /// インターフェース
    /// </summary>
    public interface ISpawnSoulMoneyModel
    {
        /// <summary>
        /// オブジェクトを生成
        /// </summary>
        /// <param name="position">生成位置</param>
        /// <param name="enemiesProp">敵のプロパティ</param>
        /// <returns>成功／失敗</returns>
        public bool InstanceCloneObjects(Vector3 position, EnemiesProp enemiesProp);
    }
}
