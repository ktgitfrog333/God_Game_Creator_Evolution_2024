using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Title.Common;
using Title.InputSystem;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

namespace Title.Model
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
        [SerializeField] private float sensibilityScratch = 40f;
        /// <summary>PushGameStartのモデル</summary>
        [SerializeField] private PushGameStartLogoModel pushGameStartLogoModel;
        /// <summary>GameStartLogoのモデル</summary>
        [SerializeField] private GameStartLogoModel gameStartLogoModel;
        /// <summary>GameExitLogoのモデル</summary>
        [SerializeField] private GameExitLogoModel gameExitLogoModel;
        /// <summary>GameExitConfirmYesLogoのモデル</summary>
        [SerializeField] private GameExitConfirmYesLogoModel gameExitConfirmYesLogoModel;
        /// <summary>GameExitConfirmNoLogoのモデル</summary>
        [SerializeField] private GameExitConfirmNoLogoModel gameExitConfirmNoLogoModel;
        /// <summary>BGMスライダーボリュームのモデル</summary>
        [SerializeField] private SliderVolModelsBgm[] sliderVolModelsBgms;
        /// <summary>SEスライダーボリュームのモデル</summary>
        [SerializeField] private SliderVolModelsSe[] sliderVolModelsSes;
        /// <summary>バイブレーション機能ラジオボタンのONのモデル</summary>
        [SerializeField] private OnVibrationModel onVibrationModel;
        /// <summary>バイブレーション機能ラジオボタンのOFFのモデル</summary>
        [SerializeField] private OffVibrationModel offVibrationModel;
        /// <summary>セーブデータ消去ボタンのモデル</summary>
        [SerializeField] private ResetSaveDataModel resetSaveDataModel;
        /// <summary>オプション設定リセットのボタンのモデル</summary>
        [SerializeField] private ResetConfigModel resetConfigModel;
        /// <summary>全ステージ解放のモデル</summary>
        [SerializeField] private AllLevelReleasedModel allLevelReleasedModel;
        /// <summary>決定ボタンのモデル</summary>
        [SerializeField] private FixModel fixModel;
        /// <summary>戻るボタンのモデル</summary>
        [SerializeField] private BackModel backModel;
        /// <summary>チュートリアルのモデル</summary>
        [SerializeField] private TutorialLogoModel tutorialLogoModel;

        private void Reset()
        {
            pushGameStartLogoModel = GameObject.Find("PushGameStartLogo").GetComponent<PushGameStartLogoModel>();
            gameStartLogoModel = GameObject.Find("GameStartLogo").GetComponent<GameStartLogoModel>();
            gameExitLogoModel = GameObject.Find("GameExitLogo").GetComponent<GameExitLogoModel>();
            gameExitConfirmYesLogoModel = GameObject.Find("GameExitConfirmYesLogo").GetComponent<GameExitConfirmYesLogoModel>();
            gameExitConfirmNoLogoModel = GameObject.Find("GameExitConfirmNoLogo").GetComponent<GameExitConfirmNoLogoModel>();
            List<SliderVolModelsBgm> sliderVolModelsBgmList = new List<SliderVolModelsBgm>();
            foreach (Transform child in GameObject.Find("SliderBgm").transform.GetChild(3))
                sliderVolModelsBgmList.Add(child.GetComponent<SliderVolModelsBgm>());
            sliderVolModelsBgms = sliderVolModelsBgmList.ToArray();
            List<SliderVolModelsSe> sliderVolModelsSeList = new List<SliderVolModelsSe>();
            foreach (Transform child in GameObject.Find("SliderSe").transform.GetChild(3))
                sliderVolModelsSeList.Add(child.GetComponent<SliderVolModelsSe>());
            sliderVolModelsSes = sliderVolModelsSeList.ToArray();
            onVibrationModel = GameObject.Find("OnVibration").GetComponent<OnVibrationModel>();
            offVibrationModel = GameObject.Find("OffVibration").GetComponent<OffVibrationModel>();
            resetSaveDataModel = GameObject.Find("ResetSaveData").GetComponent<ResetSaveDataModel>();
            resetConfigModel = GameObject.Find("ResetConfig").GetComponent<ResetConfigModel>();
            allLevelReleasedModel = GameObject.Find("AllLevelReleased").GetComponent<AllLevelReleasedModel>();
            fixModel = GameObject.Find("Fix").GetComponent<FixModel>();
            backModel = GameObject.Find("Back").GetComponent<BackModel>();
            tutorialLogoModel = GameObject.Find("TutorialLogo").GetComponent<TutorialLogoModel>();
        }

        private void Start()
        {
            // TODO: PushGameStartに関する制御
            var ignoreInitialInput = new BoolReactiveProperty();
            var isLockScroll = new BoolReactiveProperty();
            var wasCuePreseed = new BoolReactiveProperty();
            var wasPlayOrPausePreseed = new BoolReactiveProperty();
            // オブジェクトの配列を作成
            List<GameObject> modalObjects = new List<GameObject>{
                pushGameStartLogoModel.gameObject,
                gameStartLogoModel.gameObject,
                gameExitLogoModel.gameObject,
                gameExitConfirmYesLogoModel.gameObject,
                gameExitConfirmNoLogoModel.gameObject,
                onVibrationModel.gameObject,
                offVibrationModel.gameObject,
                resetSaveDataModel.gameObject,
                resetConfigModel.gameObject,
                allLevelReleasedModel.gameObject,
                fixModel.gameObject,
                backModel.gameObject,
                tutorialLogoModel.gameObject,
            };
            modalObjects.AddRange(sliderVolModelsBgms.Select(q => q.gameObject).ToArray());
            modalObjects.AddRange(sliderVolModelsSes.Select(q => q.gameObject).ToArray());
            // モーダル画面の表示状態を監視して入力無視を設定
            Observable.Merge(modalObjects.Select(ObserveObjectActivation))
                .Subscribe(isActive =>
                {
                    if (isActive)
                    {
                        ignoreInitialInput.Value = true;
                        wasCuePreseed.Value = false;
                        wasPlayOrPausePreseed.Value = false;
                        DOVirtual.DelayedCall(unDeadTimeSec, () => ignoreInitialInput.Value = false);
                    }
                });

            Selectable currentSelectable = null;
            this.UpdateAsObservable()
                .Where(_ => EventSystem.current != null)
                .Select(_ => EventSystem.current.currentSelectedGameObject)
                .Where(x => x != null)
                .Subscribe(x => currentSelectable = x.GetComponent<Selectable>());
            this.UpdateAsObservable()
                .Select(_ => TitleGameManager.Instance)
                .Where(x => x != null &&
                    x.InputSystemsOwner.CurrentInputMode.Value == (int)InputMode.MidiJackDDJ200)
                .Select(x => x.InputSystemsOwner.InputMidiJackDDJ200)
                .Subscribe(x =>
                {
                    if (currentSelectable != null &&
                        currentSelectable.isActiveAndEnabled &&
                        sensibilityScratch < Mathf.Abs(x.Scratch) &&
                        !isLockScroll.Value)
                    {
                        isLockScroll.Value = true;
                        DOVirtual.DelayedCall(unDeadTimeSec, () => isLockScroll.Value = false);
                        if (0f < x.Scratch)
                        {
                            if (!Scroll(EventSystemMidiJackModelScroll.Next, ref currentSelectable))
                                Debug.LogError("Scroll");
                        }
                        else if (x.Scratch < 0f)
                        {
                            if (!Scroll(EventSystemMidiJackModelScroll.Back, ref currentSelectable))
                                Debug.LogError("Scroll");
                        }
                    }
                    if (currentSelectable != null &&
                         currentSelectable.isActiveAndEnabled &&
                         !ignoreInitialInput.Value)
                    {
                        if (x.Cue &&
                            !wasCuePreseed.Value)
                        {
                            wasCuePreseed.Value = true;
                            ExecuteEvents.Execute(currentSelectable.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
                        }
                        else if (!x.Cue)
                            wasCuePreseed.Value = false;
                        if (x.PlayOrPause &&
                            !wasPlayOrPausePreseed.Value)
                        {
                            wasPlayOrPausePreseed.Value = true;
                            ExecuteEvents.Execute(currentSelectable.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.cancelHandler);
                        }
                        else if (!x.PlayOrPause)
                            wasPlayOrPausePreseed.Value = false;
                    }
                });
        }

        /// <summary>
        /// オブジェクトのアクティブ状態を監視する
        /// </summary>
        private System.IObservable<bool> ObserveObjectActivation(GameObject obj)
        {
            return obj.ObserveEveryValueChanged(o => o.activeSelf)
                .Where(isActive => isActive);  // アクティブになった時のみ
        }

        /// <summary>
        /// スクロールする
        /// </summary>
        /// <param name="eventSystemMidiJackModelScroll">スクロール方向</param>
        /// <param name="currentSelectable">現在のボタン</param>
        /// <returns>成功／失敗</returns>
        private bool Scroll(EventSystemMidiJackModelScroll eventSystemMidiJackModelScroll, ref Selectable currentSelectable)
        {
            try
            {
                Selectable nextSelectable = null;

                switch (eventSystemMidiJackModelScroll)
                {
                    case EventSystemMidiJackModelScroll.Back:
                        nextSelectable = currentSelectable.navigation.selectOnUp;
                        break;
                    case EventSystemMidiJackModelScroll.Next:
                        nextSelectable = currentSelectable.navigation.selectOnDown;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException($"指定不可な条件:[{eventSystemMidiJackModelScroll}]");
                }

                if (nextSelectable != null)
                {
                    currentSelectable = nextSelectable;
                    EventSystem.current.SetSelectedGameObject(currentSelectable.gameObject);
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
