using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Select.Model
{
    /// <summary>
    /// ステージセレクト
    /// モデル
    /// </summary>
    public class StageSelectModel : MonoBehaviour
    {
        /// <summary>エリアコンテンツのモデル</summary>
        [SerializeField] private AreaContentModel[] areaContentModels;
        /// <summary>実行イベント</summary>
        public IReactiveProperty<int>[] EventStates => areaContentModels.Select(q => q.EventState).ToArray();

        private void Reset()
        {
            areaContentModels = GetComponentsInChildren<AreaContentModel>();
        }
    }
}
