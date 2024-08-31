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
        /// <summary>遊び方確認ページのモデル</summary>
        [SerializeField] private TutorialViewPageModel[] tutorialViewPageModels;

        private void Reset()
        {
            stageSelectModel = GameObject.Find("StageSelect").GetComponent<StageSelectModel>();
            tutorialViewPageModels = GameObject.Find("Tutorial").GetComponentsInChildren<TutorialViewPageModel>();
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
            Button currentTutorialButton = null;
            this.UpdateAsObservable()
                .Where(_ => EventSystem.current != null)
                .Select(_ => EventSystem.current.currentSelectedGameObject)
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    currentButton = stageSelectModel.Buttons
                        .FirstOrDefault(y => y == x.GetComponent<Button>());
                    currentTutorialButton = tutorialViewPageModels.Select(q => q.Button)
                        .FirstOrDefault(y => y == x.GetComponent<Button>());
                });
            this.UpdateAsObservable()
                .Select(_ => SelectGameManager.Instance)
                .Where(x => x != null &&
                    x.InputSystemsOwner.CurrentInputMode.Value == (int)InputMode.MidiJackDDJ200)
                .Select(x => x.InputSystemsOwner.InputMidiJackDDJ200)
                .Subscribe(x =>
                {
                    if (sensibilityScratch < Mathf.Abs(x.Scratch) &&
                        !isLockScroll.Value)
                    {
                        isLockScroll.Value = true;
                        DOVirtual.DelayedCall(unDeadTimeSec, () => isLockScroll.Value = false);
                        if (0f < x.Scratch)
                        {
                            if (currentButton != null &&
                                currentButton.isActiveAndEnabled)
                            {
                                if (!Scroll(EventSystemMidiJackModelScroll.Next, ref currentButton, ScrollRoute.Vertical))
                                    Debug.LogError("Scroll");
                            }
                            if (currentTutorialButton != null &&
                                currentTutorialButton.isActiveAndEnabled)
                            {
                                if (!Scroll(EventSystemMidiJackModelScroll.Next, ref currentTutorialButton, ScrollRoute.Horizontal))
                                    Debug.LogError("Scroll");
                            }
                        }
                        else if (x.Scratch < 0f)
                        {
                            if (currentButton != null &&
                                currentButton.isActiveAndEnabled)
                            {
                                if (!Scroll(EventSystemMidiJackModelScroll.Back, ref currentButton, ScrollRoute.Vertical))
                                    Debug.LogError("Scroll");
                            }
                            if (currentTutorialButton != null &&
                                currentTutorialButton.isActiveAndEnabled)
                            {
                                if (!Scroll(EventSystemMidiJackModelScroll.Back, ref currentTutorialButton, ScrollRoute.Horizontal))
                                    Debug.LogError("Scroll");
                            }
                        }
                    }
                    if (currentButton != null &&
                        currentButton.isActiveAndEnabled)
                    {
                        if (!AddExecuteEvents(currentButton, x))
                            Debug.LogError("AddExecuteEvents");
                    }
                    if (currentTutorialButton != null &&
                        currentTutorialButton.isActiveAndEnabled)
                    {
                        if (!AddExecuteEvents(currentTutorialButton, x))
                            Debug.LogError("AddExecuteEvents");
                    }
                });
        }

        private bool AddExecuteEvents(Button currentButton, InputMidiJackDDJ200 inputMidiJackDDJ200)
        {
            try
            {
                if (currentButton != null &&
                    currentButton.isActiveAndEnabled &&
                    inputMidiJackDDJ200.Cue)
                    ExecuteEvents.Execute(currentButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                if (currentButton != null &&
                    currentButton.isActiveAndEnabled &&
                    inputMidiJackDDJ200.PlayOrPause)
                    ExecuteEvents.Execute(currentButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.cancelHandler);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// スクロールする
        /// </summary>
        /// <param name="eventSystemMidiJackModelScroll">スクロール方向</param>
        /// <param name="currentButton">現在のボタン</param>
        /// <param name="scrollRoute">スクロール往復移動</param>
        /// <returns>成功／失敗</returns>
        private bool Scroll(EventSystemMidiJackModelScroll eventSystemMidiJackModelScroll, ref Button currentButton, ScrollRoute scrollRoute)
        {
            try
            {
                switch (eventSystemMidiJackModelScroll)
                {
                    case EventSystemMidiJackModelScroll.Back:
                        switch (scrollRoute)
                        {
                            case ScrollRoute.Horizontal:
                                if (currentButton.navigation.selectOnLeft != null)
                                {
                                    currentButton = (Button)currentButton.navigation.selectOnLeft;
                                    EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
                                }

                                break;
                            case ScrollRoute.Vertical:
                                if (currentButton.navigation.selectOnUp != null)
                                {
                                    currentButton = (Button)currentButton.navigation.selectOnUp;
                                    EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
                                }

                                break;
                        }

                        break;
                    case EventSystemMidiJackModelScroll.Next:
                        switch (scrollRoute)
                        {
                            case ScrollRoute.Horizontal:
                                if (currentButton.navigation.selectOnRight != null)
                                {
                                    currentButton = (Button)currentButton.navigation.selectOnRight;
                                    EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
                                }

                                break;
                            case ScrollRoute.Vertical:
                                if (currentButton.navigation.selectOnDown != null)
                                {
                                    currentButton = (Button)currentButton.navigation.selectOnDown;
                                    EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
                                }

                                break;
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

        /// <summary>
        /// スクロール往復移動
        /// </summary>
        public enum ScrollRoute
        {
            /// <summary>横方向</summary>
            Horizontal,
            /// <summary>縦方向</summary>
            Vertical,
        }
    }
}
