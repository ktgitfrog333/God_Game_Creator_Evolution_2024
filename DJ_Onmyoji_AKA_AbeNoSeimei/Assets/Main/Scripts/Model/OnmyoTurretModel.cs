using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.Collections.LowLevel.Unsafe;

namespace Main.Model
{
    /// <summary>
    /// 陰陽玉（陰陽砲台）
    /// </summary>
    public class OnmyoTurretModel : MonoBehaviour
    {
        /// <summary>オブジェクトプール</summary>
        [Tooltip("オブジェクトプール")]
        [SerializeField] private Transform objectsPoolPrefab;
        /// <summary>弾を発射する時間間隔（秒）</summary>
        [Tooltip("弾を発射する時間間隔（秒）")]
        [SerializeField] private float instanceRateTimeSec = .5f;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;
        private RectTransform RectTransform => Transform as RectTransform;

        private void Start()
        {
            var pool = GameObject.Find("ObjectsPool");
            ObjectsPoolModel poolModel;
            if (pool == null)
                poolModel = Instantiate(objectsPoolPrefab).GetComponent<ObjectsPoolModel>();
            else
                poolModel = pool.GetComponent<ObjectsPoolModel>();
            
            poolModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                        StartCoroutine(InstanceBullets(instanceRateTimeSec, poolModel));
                });
        }

        /// <summary>
        /// 弾を生成
        /// </summary>
        /// <param name="instanceRateTimeSec">弾を発射する時間間隔（秒）</param>
        /// <param name="objectsPoolModel">オブジェクトプール</param>
        /// <returns>コルーチン</returns>
        private IEnumerator InstanceBullets(float instanceRateTimeSec, ObjectsPoolModel objectsPoolModel)
        {
            // 一定間隔で弾を生成するための実装
            while (true)
            {
                var bullet = objectsPoolModel.GetOnmyoBulletModel();
                if (!bullet.Initialize(CalibrationFromTarget(RectTransform), RectTransform.parent.eulerAngles))
                    Debug.LogError("Initialize");
                if (!bullet.isActiveAndEnabled)
                    bullet.gameObject.SetActive(true);
                yield return new WaitForSeconds(instanceRateTimeSec);
            }
        }

        /// <summary>
        /// ターゲット位置を元に調整（From）
        /// </summary>
        /// <param name="rectTransform">UIターゲット情報</param>
        /// <returns>成功／失敗</returns>
        private Vector3 CalibrationFromTarget(RectTransform rectTransform)
        {
            // UI要素のローカル座標を取得
            Vector3 localPosition = rectTransform.localPosition;
            // ローカル座標をワールド座標に変換
            Vector3 worldPosition = rectTransform.parent.TransformPoint(localPosition);
            return worldPosition;
        }

    }
}
