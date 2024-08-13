using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Select.Common;
using Select.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

namespace Select.Model
{
    /// <summary>
    /// MIDIJack用
    /// イベントシステム
    /// </summary>
    public class EventSystemMidiJackModel : MonoBehaviour
    {
        /// <summary>入力無視の時間（秒）</summary>
        [SerializeField] private float unDeadTimeSec = .2f;
        /// <summary>スクラッチ入力の調整値</summary>
        [SerializeField] private float sensibilityScratch = 5f;
        /// <summary>ステージセレクトのモデル</summary>
        [SerializeField] private StageSelectModel stageSelectModel;

        private void Reset()
        {
            stageSelectModel = GameObject.Find("StageSelect").GetComponent<StageSelectModel>();
        }

        private void Start()
        {
            var ignoreInitialInput = new BoolReactiveProperty(true);
            var isLockScroll = new BoolReactiveProperty();
            // ポーズ画面が開かれた直後の入力を無視する処理
            this.UpdateAsObservable()
                .Select(_=> Time.timeScale)
                .DistinctUntilChanged() // 値が変わったときだけストリームに流す
                .Subscribe(x =>
                {
                    if (x == 0f)
                        DOVirtual.DelayedCall(.5f, () => ignoreInitialInput.Value = false);
                    else if (x == 1f)
                        ignoreInitialInput.Value = true;
                });
            // UIオブジェクトが有効となった場合に制御する
            Button currentButton = null;
            this.UpdateAsObservable()
                .Where(_ => EventSystem.current != null)
                .Select(_ => EventSystem.current.currentSelectedGameObject)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    currentButton = stageSelectModel.Buttons
                        .FirstOrDefault(y => y == x.GetComponent<Button>());
                });
            this.UpdateAsObservable()
                .Select(_ => SelectGameManager.Instance)
                .Where(x => x != null &&
                    x.InputSystemsOwner.CurrentInputMode.Value == (int)InputMode.MidiJackDDJ200)
                .Select(x => x.InputSystemsOwner.InputMidiJackDDJ200)
                .Subscribe(x =>
                {
                    if (currentButton != null &&
                        currentButton.isActiveAndEnabled &&
                        sensibilityScratch < Mathf.Abs(x.Scratch) &&
                        !isLockScroll.Value)
                    {
                        isLockScroll.Value = true;
                        DOVirtual.DelayedCall(unDeadTimeSec, () => isLockScroll.Value = false);
                        if (0f < x.Scratch)
                        {
                            if (!Scroll(EventSystemMidiJackModelScroll.Next, ref currentButton))
                                Debug.LogError("Scroll");
                        }
                        else if (x.Scratch < 0f)
                        {
                            if (!Scroll(EventSystemMidiJackModelScroll.Back, ref currentButton))
                                Debug.LogError("Scroll");
                        }
                    }
                    if (currentButton != null &&
                        currentButton.isActiveAndEnabled &&
                        x.Cue)
                        ExecuteEvents.Execute(currentButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                    if (currentButton != null &&
                        currentButton.isActiveAndEnabled &&
                        x.PlayOrPause)
                        ExecuteEvents.Execute(currentButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.cancelHandler);
                });
        }

        /// <summary>
        /// スクロールする
        /// </summary>
        /// <param name="eventSystemMidiJackModelScroll">スクロール方向</param>
        /// <param name="currentButton">現在のボタン</param>
        /// <returns>成功／失敗</returns>
        private bool Scroll(EventSystemMidiJackModelScroll eventSystemMidiJackModelScroll, ref Button currentButton)
        {
            try
            {
                switch (eventSystemMidiJackModelScroll)
                {
                    case EventSystemMidiJackModelScroll.Back:
                        if (currentButton.navigation.selectOnUp != null)
                        {
                            currentButton = (Button)currentButton.navigation.selectOnUp;
                            EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
                        }

                        break;
                    case EventSystemMidiJackModelScroll.Next:
                        if (currentButton.navigation.selectOnDown != null)
                        {
                            currentButton = (Button)currentButton.navigation.selectOnDown;
                            EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
                        }

                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException($"指定不可な条件:[{eventSystemMidiJackModelScroll}]");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// スクロール方向
        /// </summary>
        public enum EventSystemMidiJackModelScroll
        {
            /// <summary>前へ</summary>
            Back = 0,
            /// <summary>次へ</summary>
            Next = 1,
        }
    }
}
