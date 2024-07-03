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
        [SerializeField] private float[] updateCorrected = { 1f, 1f };
        /// <summary>更新の補正値（MidiJack）</summary>
        [SerializeField] private float[] updateCorrectedMidiJack = { 1f, 1f };
        /// <summary>InputSystemのユーティリティ</summary>
        private InputSystemUtility _inputSysUtility = new InputSystemUtility();
        /// <summary>演出の再生時間</summary>
        [SerializeField] private float[] durations = { 1.5f, 3.0f };
        /// <summary>スリップループ時、陰陽砲台のみ特殊レート値</summary>
        [SerializeField] private float onmyoSlipLoopRate = 15f;

        private void Start()
        {
            var utilityCommon = new MainCommonUtility();
            var adminDataSingleton = utilityCommon.AdminDataSingleton;
            candleInfo.limitCandleResorceMax = adminDataSingleton.AdminBean.shikigamiSkillSystemModel.candleInfo.limitCandleResorceMax;
            candleInfo.rapidRecoveryTimeSec = adminDataSingleton.AdminBean.shikigamiSkillSystemModel.candleInfo.rapidRecoveryTimeSec;
            candleInfo.rapidRecoveryRate = adminDataSingleton.AdminBean.shikigamiSkillSystemModel.candleInfo.rapidRecoveryRate;
            candleInfo.CandleResource = new FloatReactiveProperty(candleInfo.LimitCandleResorceMax);
            candleInfo.IsOutCost = new BoolReactiveProperty();
            candleInfo.rapidRecoveryState = new IntReactiveProperty((int)RapidRecoveryType.None);
            candleInfo.isStopRecovery = new BoolReactiveProperty();
            var utility = new ShikigamiParameterUtility();
            var shikigamis = utility.GetPentagramTurnTableInfo().slots.Select(q => q.prop.shikigamiInfo).ToArray();
            for (var i = 0; i < shikigamis.Length; i++)
            {
                shikigamis[i].state.tempoLevel = new FloatReactiveProperty();
                shikigamis[i].state.tempoLevelRevertState = new IntReactiveProperty((int)RapidRecoveryType.None);
            }
            _shikigamiInfos = shikigamis;
            if (!_inputSysUtility.SetCandleResourceAndTempoLevelsInModel(candleInfo, _shikigamiInfos, updateCorrected[0], updateCorrectedMidiJack[0], this))
                Debug.LogError("SetCandleResourceAndTempoLevels");
        }

        public bool UpdateCandleResource(JockeyCommandType jkeyCmdTypeCurrent, JockeyCommandType jkeyCmdTypePrevious)
        {
            return _inputSysUtility.UpdateCandleResourceByPentagram(jkeyCmdTypeCurrent, jkeyCmdTypePrevious, candleInfo, updateCorrected[1], updateCorrectedMidiJack[1], this);
        }

        public bool ForceZeroAndRapidRecoveryCandleResource(JockeyCommandType jockeyCommandType)
        {
            try
            {
                switch (jockeyCommandType)
                {
                    case JockeyCommandType.BackSpin:
                        if (!_inputSysUtility.ResetCandleResourceAndBuffAllTempoLevelsByPentagram(candleInfo, _shikigamiInfos, durations, this))
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

        public bool UpdateCandleResourceOfAttackOnmyoTurret()
        {
            return _inputSysUtility.UpdateCandleResourceByPentagram(candleInfo, _shikigamiInfos, onmyoSlipLoopRate);
        }

        public bool SetIsStopRecovery(JockeyCommandType jockeyCommandType)
        {
            try
            {
                switch (jockeyCommandType)
                {
                    case JockeyCommandType.SlipLoop:
                        candleInfo.isStopRecovery.Value = true;

                        break;
                    default:
                        candleInfo.isStopRecovery.Value = false;

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
        /// <summary>急速回復を行う時間（秒）</summary>
        public float rapidRecoveryTimeSec;
        /// <summary>急速回復効果</summary>
        public float rapidRecoveryRate;
        /// <summary>急速回復ステート</summary>
        public IReactiveProperty<int> rapidRecoveryState { get; set; }
        /// <summary>回復停止状態か（スリップループのみ使用）</summary>
        public IReactiveProperty<bool> isStopRecovery;
    }

    /// <summary>
    /// 急速回復実行タイプ
    /// </summary>
    public enum RapidRecoveryType
    {
        /// <summary>なし</summary>
        None,
        /// <summary>準備中</summary>
        Reserve,
        /// <summary>効果発動中</summary>
        Doing,
        /// <summary>完了</summary>
        Done,
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
        /// <summary>
        /// 陰陽砲台の攻撃によるリソース消費更新
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool UpdateCandleResourceOfAttackOnmyoTurret();
        /// <summary>
        /// 回復停止状態か（スリップループのみ使用）をセット
        /// </summary>
        /// <param name="jockeyCommandType">ジョッキーコマンドタイプ</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsStopRecovery(JockeyCommandType jockeyCommandType);
    }
}
