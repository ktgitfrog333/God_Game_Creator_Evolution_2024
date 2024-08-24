using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Universal.Template;
using Title.Common;
using Title.Template;
using UniRx;
using Universal.Common;

namespace Title.Model
{
    /// <summary>
    /// モデル
    /// BGMスライダー
    /// </summary>
    [RequireComponent(typeof(Slider))]
    [RequireComponent(typeof(EventTrigger))]
    public class SliderBgmModel : UIEventController
    {
        /// <summary>スライダー</summary>
        private Slider _slider;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;
        /// <summary>ボリューム番号</summary>
        private readonly IntReactiveProperty _index = new IntReactiveProperty();
        /// <summary>ボリューム番号</summary>
        public IReactiveProperty<int> Index => _index;

        protected override void OnEnable()
        {
            base.OnEnable();
            var tTResources = new TemplateResourcesAccessory();
            var datas = tTResources.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            _index.Value = datas.bgmVolumeIndex;
        }

        /// <summary>
        /// インデックス番号をセット
        /// </summary>
        /// <param name="index">インデックス番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetIndex(int index)
        {
            try
            {
                var tTValidation = new TitleTemplateOptionalInputValueValidation();
                if (tTValidation.CheckBgmValueAndGetResultState(index).Equals(EnumResponseState.Success))
                    _index.Value = index;
                else
                    throw new System.Exception("値チェックのエラー");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
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
                if (_slider == null)
                    _slider = GetComponent<Slider>();
                _slider.enabled = enabled;
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
                if (_eventTrigger == null)
                    _eventTrigger = GetComponent<EventTrigger>();
                _eventTrigger.enabled = enabled;

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
