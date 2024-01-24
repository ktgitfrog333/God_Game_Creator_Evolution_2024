using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Common;
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
    public class ShikigamiSkillSystemModel : MonoBehaviour, IShikigamiSkillSystemModel
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
        [SerializeField] private float[] updateCorrected = { 1f, 1f};
        /// <summary>InputSystemのユーティリティ</summary>
        private InputSystemUtility _inputSysUtility = new InputSystemUtility();
        /// <summary>急速回復を行う時間（秒）</summary>
        [SerializeField] private float rapidRecoveryTimeSec = 40f;

        private void Reset()
        {
            candleInfo.limitCandleResorceMax = 10f;
        }

        private void Start()
        {
            candleInfo.CandleResource = new FloatReactiveProperty(candleInfo.LimitCandleResorceMax);
            candleInfo.IsOutCost = new BoolReactiveProperty();
            var utility = new ShikigamiParameterUtility();
            var shikigamis = utility.GetPentagramTurnTableInfo().slots.Select(q => q.prop.shikigamiInfo).ToArray();
            for (var i = 0; i < shikigamis.Length; i++)
                shikigamis[i].state.tempoLevel = new FloatReactiveProperty();
            _shikigamiInfos = shikigamis;
            if (!_inputSysUtility.SetCandleResourceAndTempoLevelsInModel(candleInfo, _shikigamiInfos, updateCorrected[0], this))
                Debug.LogError("SetCandleResourceAndTempoLevels");
        }

        public bool UpdateCandleResource(JockeyCommandType jkeyCmdTypeCurrent, JockeyCommandType jkeyCmdTypePrevious)
        {
            return _inputSysUtility.UpdateCandleResourceByPentagram(jkeyCmdTypeCurrent, jkeyCmdTypePrevious, candleInfo, updateCorrected[1], this);
        }

        public bool ForceZeroAndRapidRecoveryCandleResource(JockeyCommandType jockeyCommandType)
        {
            try
            {
                switch (jockeyCommandType)
                {
                    case JockeyCommandType.BackSpin:
                        if (!_inputSysUtility.ResetCandleResourceAndBuffAllTempoLevelsByPentagram(candleInfo, _shikigamiInfos, rapidRecoveryTimeSec, this))
                            throw new System.Exception("ResetCandleResourceAndBuffAllTempoLevelsByPentagram");

                        break;
                    default:
                        // 他のコマンドはここでは扱わない
                        break;
                }

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
    /// 式神スキル管理システム
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IShikigamiSkillSystemModel
    {
        /// <summary>
        /// リソースを更新
        /// 引数の+-は考慮せずリソースは消費される
        /// </summary>
        /// <param name="jkeyCmdTypeCurrent">ジョッキーコマンドタイプ</param>
        /// <param name="jkeyCmdTypePrevious">1つ前のジョッキーコマンドタイプ</param>
        /// <returns>成功／失敗</returns>
        public bool UpdateCandleResource(JockeyCommandType jkeyCmdTypeCurrent, JockeyCommandType jkeyCmdTypePrevious);
        /// <summary>
        /// 強制的にリソースが0になり、
        /// その後リソースの急速回復が始まる
        /// </summary>
        /// <param name="jockeyCommandType">ジョッキーコマンドタイプ</param>
        /// <returns>成功／失敗</returns>
        public bool ForceZeroAndRapidRecoveryCandleResource(JockeyCommandType jockeyCommandType);
    }
}
