using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// ペンダグラムターンテーブル
    /// プレゼンタから伝達された入力を元に出力を行う
    /// Imageコンポーネントへ入力操作を行う
    /// モデル
    /// </summary>
    public class PentagramTurnTableModel : MonoBehaviour
    {
        /// <summary>陰陽玉（陰陽砲台）プレハブ</summary>
        [Tooltip("陰陽玉（陰陽砲台）プレハブ")]
        [SerializeField] private Transform onmyoTurretPrefab;
        /// <summary>円の中心から外周への距離</summary>
        [Tooltip("円の中心から外周への距離")]
        [SerializeField] private float distance = 2f;

        private void Start()
        {
            // オブジェクトを中心に5つの位置へプレハブを生成
            // 対象位置は下記の条件に従う
            // ・中心から五角形とした場合に各頂点を座標とする
            var t = transform;
            float angleStep = 360f / 5;
            for (int i = 0; i < 5; i++)
            {
                float angle = (angleStep * i + 15) * Mathf.Deg2Rad;
                Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
                Transform turret = Instantiate(onmyoTurretPrefab, position, Quaternion.identity);
                turret.SetParent(t, false);
            }
        }
    }
}
