using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Main.Common;
using UniRx;
using UnityEngine;
using Universal.Common;
using Universal.Template;

namespace Main.Model
{
    /// <summary>
    /// クリア条件を満たす要素を管理するシステム
    /// モデル
    /// </summary>
    public class ClearCountdownTimerSystemModel : MonoBehaviour, IClearCountdownTimerSystemModel
    {
        /// <summary>タイマー</summary>
        public IReactiveProperty<float> TimeSec { get; private set; } = new FloatReactiveProperty();
        /// <summary>時間切れか</summary>
        public IReactiveProperty<int> IsTimeOut { get; private set; } = new IntReactiveProperty();
        /// <summary>制限時間（秒）</summary>
        private float _limitTimeSecMax;
        /// <summary>制限時間（秒）</summary>
        public float LimitTimeSecMax => _limitTimeSecMax;

        private void Awake()
        {
            enabled = false;
        }

        private void Start()
        {
            // 初期処理
            // 下記のフィールドへ対応するクラスをインスタンスして初期値をセット
            var temp = new TemplateResourcesAccessory();
            var user = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            var admin = temp.LoadSaveDatasJsonOfAdminBean(ConstResorcesNames.ADMIN_DATA);
            _limitTimeSecMax = admin.clearCountdownTimer[user.sceneId - 1];
            TimeSec.Value = _limitTimeSecMax;
        }

        private void Update()
        {
            // カウントダウンタイマー処理
            // フレーム時間の差異を埋めつつ、正確な時間間隔で下記のフィールドを更新する
            TimeSec.Value -= Time.deltaTime;
            // また、TimeSecが0以下へなった際にカウントダウン処理は行わず、下記のフィールドを更新する
            if (TimeSec.Value <= 0)
            {
                TimeSec.Value = 0;
                IsTimeOut.Value = (int)IsTimeOutState.TimeOut;
            }
        }

        public bool SetIsActiveAndEnabled(int isTimeOutState)
        {
            try
            {
                switch ((IsTimeOutState)isTimeOutState)
                {
                    case IsTimeOutState.TimeOut:
                        enabled = true;

                        break;
                    default:
                        // それ以外
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
    /// クリア条件を満たす要素を管理するシステム
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IClearCountdownTimerSystemModel
    {
        /// <summary>
        /// アクティブ状態をセット
        /// </summary>
        /// <param name="isTimeOutState">タイムアウト状態</param>
        /// <returns>成功／失敗</returns>
        public bool SetIsActiveAndEnabled(int isTimeOutState);
    }
}
