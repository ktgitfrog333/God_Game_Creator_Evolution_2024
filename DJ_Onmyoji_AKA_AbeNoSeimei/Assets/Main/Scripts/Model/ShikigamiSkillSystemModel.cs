using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Test.Stub;
using Main.Utility;
using UniRx;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// 式神スキル管理システム
    /// 下記のリソース管理を行う
    ///     ●蝋燭コスト
    ///     ●テンポレベル
    ///         ○ラップ
    ///         ○ダンス
    ///         ○グラフィティ
    /// コントローラーの操作に合わせてコストとレベルを更新
    /// モデル
    /// </summary>
    public class ShikigamiSkillSystemModel : MonoBehaviour
    {
        /// <summary>蠟燭の情報</summary>
        [SerializeField] private CandleInfo candleInfo;
        /// <summary>蠟燭の情報</summary>
        public CandleInfo CandleInfo => candleInfo;
        /// <summary>式神の情報</summary>
        private ShikigamiInfo[] _shikigamiInfos;
        /// <summary>式神の情報</summary>
        public ShikigamiInfo[] ShikigamiInfos => _shikigamiInfos;
        /// <summary>更新の補正値</summary>
        [SerializeField] private float updateCorrected = 1f;

        private void Reset()
        {
            candleInfo.limitCandleResorceMax = 10f;
        }

        private void Start()
        {
            candleInfo.CandleResource = new FloatReactiveProperty(candleInfo.LimitCandleResorceMax);
            candleInfo.IsOutCost = new BoolReactiveProperty();

            var utility = new InputSystemUtility();
            // TODO:StubからBeanへ変更
            var shikigamis = GetComponent<ShikigamiSkillSystemModelTest>();
            for (var i = 0; i < shikigamis.Shikigamis.Length; i++)
                shikigamis.Shikigamis[i].state.tempoLevel = new FloatReactiveProperty();
            _shikigamiInfos = shikigamis.Shikigamis;
            if (!utility.SetCandleResourceAndTempoLevelsInModel(candleInfo, _shikigamiInfos, updateCorrected, this))
                Debug.LogError("SetCandleResourceAndTempoLevels");
        }
    }

    /// <summary>
    /// 蠟燭の情報
    /// </summary>
    [System.Serializable]
    public struct CandleInfo
    {
        /// <summary>蠟燭リソース</summary>
        public IReactiveProperty<float> CandleResource { get; set; }
        /// <summary>リソース切れか</summary>
        public IReactiveProperty<bool> IsOutCost { get; set; }
        /// <summary>蠟燭リソース最大値</summary>
        public float limitCandleResorceMax;
        /// <summary>蠟燭リソース最大値</summary>
        public float LimitCandleResorceMax => limitCandleResorceMax;
    }
}
