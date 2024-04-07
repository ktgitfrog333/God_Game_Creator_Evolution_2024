using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Select.Utility;
using UniRx;
using UniRx.Triggers;
using Select.Common;

namespace Select.Model
{
    /// <summary>
    /// エリアコンテンツ
    /// モデル
    /// </summary>
    public class AreaContentModel : UIEventController, IAreaContentModel
    {
        /// <summary>ステージ番号</summary>
        private int _index = -1;
        /// <summary>番号置換の正規表現</summary>
        private readonly Regex _regex = new Regex("^.*_");
        /// <summary>ステージ番号</summary>
        public int Index
        {
            get
            {
                // ステージ番号セットの初期処理
                if (_index < 0)
                    _index = int.Parse(_regex.Replace(name, ""));
                if (_index < 0)
                    throw new System.Exception("置換失敗");
                return _index;
            }
        }
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;

        private void Start()
        {
            this.UpdateAsObservable()
                .Select(_ => SelectGameManager.Instance)
                .Where(x => x != null)
                .Select(x => x.Presenter)
                .Where(x => x != null)
                .Take(1)
                .Subscribe(x =>
                {
                    x.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                        .Where(x => x)
                        .Subscribe(_ =>
                        {
                            var utilityCommon = new SelectCommonUtility();
                            if (utilityCommon.UserDataSingleton.UserBean.sceneId == Index)
                                SetSelectedGameObject();
                            // 初期処理
                            if (!SetButtonEnabled(false))
                                Debug.LogError("SetButtonEnabled");
                            if (!SetEventTriggerEnabled(false))
                                Debug.LogError("SetEventTriggerEnabled");
                        });
                });
        }

        public bool SetButtonEnabled(bool enabled)
        {
            return _selectUGUIsModelUtility.SetButtonEnabledOfButton(enabled, _button, Transform);
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            return _selectUGUIsModelUtility.SetEventTriggerEnabledOfEventTrigger(enabled, _eventTrigger, Transform);
        }
    }

    /// <summary>
    /// エリアコンテンツ
    /// モデル
    /// インターフェース
    /// </summary>
    public interface IAreaContentModel
    {
        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonEnabled(bool enabled);
        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventTriggerEnabled(bool enabled);
    }
}
