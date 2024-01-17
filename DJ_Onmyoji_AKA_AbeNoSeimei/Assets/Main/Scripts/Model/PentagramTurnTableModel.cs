using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
using Main.Utility;
using UnityEngine;
using Universal.Common;

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
        /// <summary>ラッププレハブ</summary>
        [SerializeField] private Transform wrapTurretPrefab;
        /// <summary>ダンスプレハブ</summary>
        [SerializeField] private Transform danceTurretPrefab;
        /// <summary>グラフィティプレハブ</summary>
        [SerializeField] private Transform graffitiTurretPrefab;
        /// <summary>円の中心から外周への距離</summary>
        [Tooltip("円の中心から外周への距離")]
        [SerializeField] private float distance;
        /// <summary>ペンダグラムターンテーブル情報</summary>
        private PentagramTurnTableInfo _pentagramTurnTableInfo;
        /// <summary>ペンダグラムターンテーブル情報</summary>
        public PentagramTurnTableInfo PentagramTurnTableInfo => _pentagramTurnTableInfo;

        private void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            // 式神の情報を内部管理する
            // どのスロットへどの式神がセットされているかを管理する処理を呼び出してメンバー変数へセット
            var utility = new ShikigamiParameterUtility();
            _pentagramTurnTableInfo = utility.GetPentagramTurnTableInfo();
            var slots = _pentagramTurnTableInfo.slots;

            // オブジェクトを中心に5つの位置へプレハブを生成
            // 対象位置は下記の条件に従う
            // ・中心から五角形とした場合に各頂点を座標とする
            distance = adminDataSingleton.AdminBean.PentagramTurnTableModel.distance;
            var t = transform;
            float angleStep = 360f / 5;
            for (int i = 0; i < 5; i++)
            {
                foreach (var item in slots.Select((p, i) => new { Content = p, Index = i})
                    .Where(q => q.Content.slotId.Equals((SlotId)i)))
                {
                    float angle = (angleStep * i + 15) * Mathf.Deg2Rad;
                    Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
                    var slot = slots[item.Index];
                    Transform turret = Instantiate(GetTargetOfPrefab(slot.shikigamiInfo.prop.type), position, Quaternion.identity);
                    slot.instanceId = turret.GetComponent<TurretModel>().InstanceID;
                    slots[item.Index] = slot;
                    turret.SetParent(t, false);
                }
            }
        }

        /// <summary>
        /// 対象のプレハブを取得
        /// </summary>
        /// <param name="shikigamiType">式神タイプ</param>
        /// <returns>プレハブ</returns>
        /// <exception cref="System.NotImplementedException">プレハブ未実装</exception>
        /// <exception cref="System.Exception">例外エラー</exception>
        private Transform GetTargetOfPrefab(ShikigamiType shikigamiType)
        {
            switch (shikigamiType)
            {
                case ShikigamiType.Wrap:
                    return wrapTurretPrefab;
                case ShikigamiType.Dance:
                    return danceTurretPrefab;
                case ShikigamiType.Graffiti:
                    return graffitiTurretPrefab;
                case ShikigamiType.OnmyoTurret:
                    return onmyoTurretPrefab;
                default:
                    throw new System.Exception("例外エラー");
            }
        }
    }
}
