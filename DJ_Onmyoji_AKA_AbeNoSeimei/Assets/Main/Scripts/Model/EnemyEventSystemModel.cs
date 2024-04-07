using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 敵イベントを管理する
    /// モデル
    /// </summary>
    public class EnemyEventSystemModel : MonoBehaviour
    {
        /// <summary>オブジェクトプールのプレハブ</summary>
        [SerializeField] private Transform objectsPoolPrefab;
        /// <summary>敵</summary>
        private List<EnemyModel> _enemies = new List<EnemyModel>();
        /// <summary>敵の死亡イベントを通知するためのSubject</summary>
        private Subject<EnemyModel> _onEnemyDead = new Subject<EnemyModel>();
        /// <summary>敵の死亡イベントを外部に公開するためのReadOnlyReactiveProperty</summary>
        public System.IObservable<EnemyModel> OnEnemyDead => _onEnemyDead;

        private void Start()
        {
            SpawnUtility spawnUtility = new SpawnUtility();
            ObjectsPoolModel objectsPoolModel = spawnUtility.FindOrInstantiateForGetObjectsPoolModel(objectsPoolPrefab);
            objectsPoolModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(x =>
                {
                    foreach (var enemy in objectsPoolModel.GetEnemiesModel())
                        if (!AddEnemies(ref _enemies, enemy, _onEnemyDead))
                            Debug.LogError("AddEnemies");
                    objectsPoolModel.OnEnemyInstanced.Subscribe(enemy =>
                    {
                        if (!AddEnemies(ref _enemies, enemy, _onEnemyDead))
                            Debug.LogError("AddEnemies");
                    });
                });
        }

        private void OnDestroy()
        {
            // オブジェクトが破棄されたときに、全ての購読を解除
            _onEnemyDead.OnCompleted();
        }

        /// <summary>
        /// 敵情報を追加する
        /// </summary>
        /// <param name="enemies">敵</param>
        /// <param name="enemy">敵</param>
        /// <param name="onEnemyDead">敵の死亡イベント</param>
        /// <returns>成功／失敗</returns>
        private bool AddEnemies(ref List<EnemyModel> enemies, EnemyModel enemy, Subject<EnemyModel> onEnemyDead)
        {
            try
            {
                enemies.Add(enemy);
                // 敵の死亡イベントを購読
                enemy.State.IsDead.Where(isDead => isDead)
                    .Subscribe(_ => onEnemyDead.OnNext(enemy))
                    .AddTo(this);
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
