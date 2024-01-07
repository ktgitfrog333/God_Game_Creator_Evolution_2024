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

    /// <summary>
    /// 式神の情報
    /// </summary>
    [System.Serializable]
    public struct ShikigamiInfo
    {
        /// <summary>プロパティ</summary>
        public Prop prop;
        /// <summary>ステート</summary>
        public State state;

        /// <summary>
        /// プロパティ
        /// </summary>
        [System.Serializable]
        public struct Prop
        {
            /// <summary>式神タイプ</summary>
            public ShikigamiType type;
            /// <summary>レベル</summary>
            public int level;
            /// <summary>ラップ</summary>
            public Wrap wrap;
            /// <summary>ダンス</summary>
            public Dance dance;
            /// <summary>グラフィティ</summary>
            public Graffiti graffiti;

            /// <summary>
            /// ラップ
            /// </summary>
            [System.Serializable]
            public struct Wrap
            {
                /// <summary>攻撃間隔</summary>
                public float attackRate;
                /// <summary>攻撃力</summary>
                public float attackLevel;
                // T.B.D 他ステータスを追加
            }

            /// <summary>
            /// ダンス
            /// </summary>
            [System.Serializable]
            public struct Dance
            {
                /// <summary>攻撃間隔</summary>
                public float attackRate;
                // T.B.D 他ステータスを追加
            }

            /// <summary>
            /// グラフィティ
            /// </summary>
            [System.Serializable]
            public struct Graffiti
            {
                /// <summary>行動間隔</summary>
                public float doRate;
                // T.B.D 他ステータスを追加
            }
        }

        /// <summary>
        /// ステート
        /// </summary>
        public struct State
        {
            /// <summary>テンポレベル</summary>
            public IReactiveProperty<float> tempoLevel;
        }
    }
}
