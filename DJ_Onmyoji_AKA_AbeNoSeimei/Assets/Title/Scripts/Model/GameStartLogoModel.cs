using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx.Triggers;
using UniRx;
using Title.Common;
using Title.InputSystem;
using DG.Tweening;

namespace Title.Model
{

    /// <summary>
    /// モデル
    /// ゲーム開始ロゴ
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class GameStartLogoModel : UIEventController
    {
        /// <summary>入力無視の時間（秒）</summary>
        [SerializeField] private float unDeadTimeSec = .2f;
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>ボタン</summary>
        private Button Button => _button == null ? _button = GetComponent<Button>() : _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;
        /// <summary>イベントトリガー</summary>
        private EventTrigger EventTrigger => _eventTrigger == null ? _eventTrigger = GetComponent<EventTrigger>() : _eventTrigger;

        private void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => TitleGameManager.Instance)
                .Where(x => x != null)
                .Select(x => x.InputSystemsOwner)
                .Where(x => x.CurrentInputMode.Value == (int)InputMode.MidiJackDDJ200)
                .Take(1)
                .Subscribe(_ =>
                {
                    this.ObserveEveryValueChanged(x => x.isActiveAndEnabled)
                        .Where(x => x)
                        .Subscribe(_ =>
                        {
                            DOVirtual.DelayedCall(unDeadTimeSec, () =>
                            {
                                Button.enabled = true;
                                EventTrigger.enabled = true;
                            });
                        });
                });

        }

        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonEnabled(bool enabled)
        {
            try
            {
                Button.enabled = enabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventTriggerEnabled(bool enabled)
        {
            try
            {
                EventTrigger.enabled = enabled;

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
