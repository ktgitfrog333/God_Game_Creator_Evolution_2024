using System.Collections;
using System.Collections.Generic;
using Main.Common;
using Main.Model;
using Main.View;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Main.Presenter
{
    public class MainPresenterBossDemo : MonoBehaviour
    {
        /// <summary>クリア条件を満たす要素を管理するシステムのモデル</summary>
        [SerializeField] private ClearCountdownTimerSystemModel clearCountdownTimerSystemModel;
        /// <summary>カウントダウンタイマーの情報に合わせてUIを変化させるビュー</summary>
        [SerializeField] private ClearCountdownTimerCircleView clearCountdownTimerCircleView;
        /// <summary>陰陽（昼夜）の切り替えのモデル</summary>
        [SerializeField] private SunMoonSystemModel sunMoonSystemModel;
        /// <summary>陰陽（昼夜）のアイコンビュー</summary>
        [SerializeField] private SunMoonStateIconView sunMoonStateIconView;
        [SerializeField] private BossEnemyModel bossEnemyModel;
        [SerializeField] private BossEnemyView bossEnemyView;

        private void Reset()
        {
            clearCountdownTimerSystemModel = GameObject.Find("ClearCountdownTimerSystem").GetComponent<ClearCountdownTimerSystemModel>();
            clearCountdownTimerCircleView = GameObject.Find("SunMoonStateCircleGauge").GetComponent<ClearCountdownTimerCircleView>();
            sunMoonSystemModel = GameObject.Find("SunMoonSystem").GetComponent<SunMoonSystemModel>();
            sunMoonStateIconView = GameObject.Find("SunMoonStateIcon").GetComponent<SunMoonStateIconView>();
            bossEnemyModel = GameObject.FindWithTag(ConstTagNames.TAG_NAME_BOSS_ENEMY).GetComponent<BossEnemyModel>();
            bossEnemyView = GameObject.FindWithTag(ConstTagNames.TAG_NAME_BOSS_ENEMY).GetComponent<BossEnemyView>();
        }

        // Start is called before the first frame update
        void Start()
        {
            var isGoalReached = new BoolReactiveProperty();
            bossEnemyModel.KingAoandonProp.bossDirectionPhase.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    Observable.FromCoroutine<bool>(observer => clearCountdownTimerSystemModel.SetIsTimeOut(observer, x))
                        .Subscribe(_ => {})
                        .AddTo(gameObject);
                    Observable.FromCoroutine<bool>(observer => bossEnemyView.MovePointEntrance(observer, x))
                        .Where(x => x)
                        .Subscribe(_ =>
                        {
                            if (!bossEnemyModel.SetClearCount(0))
                                Debug.LogError("SetClearCount");
                        })
                        .AddTo(gameObject);
                });
            bossEnemyModel.KingAoandonProp.bossActionPhase.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (!bossEnemyView.Movement(x))
                        Debug.LogError("Movement");
                });
            isGoalReached.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x => Debug.Log($"isGoalReached:[{x}]"));
            clearCountdownTimerSystemModel.enabled = true;
            IClearCountdownTimerViewAdapter circleView = new ClearCountdownTimerCircleViewAdapter(clearCountdownTimerCircleView);
            clearCountdownTimerSystemModel.TimeSec.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // if (!circleView.Set(x, clearCountdownTimerSystemModel.LimitTimeSecMax))
                    //     Debug.LogError("SetAngle");
                });
            clearCountdownTimerSystemModel.IsTimeOut.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (!clearCountdownTimerSystemModel.SetIsActiveAndEnabled(x))
                        Debug.LogError("SetIsActiveAndEnabled");
                    if (!circleView.Set(0f, clearCountdownTimerSystemModel.LimitTimeSecMax, x))
                        Debug.LogError("SetAngle");
                    // TODO:モデル側かOwner側でクリア可否を制御する
                    switch ((IsTimeOutState)x)
                    {
                        case IsTimeOutState.TimeOut:
                            isGoalReached.Value = true;

                            break;
                        default:
                            // それ以外
                            break;
                    }
                    Observable.FromCoroutine<bool>(observer => clearCountdownTimerCircleView.PlayRepairAngleAnimation(observer, x))
                        .Subscribe(_ => {},
                            onError: exception => Debug.LogError("PlayRepairAngleAnimation"))
                        .AddTo(gameObject);
                });
            this.UpdateAsObservable()
                // .Select(_ => levelOwner.InstancedLevel.GetComponentInChildren<EnemiesSpawnModel>())
                // .Where(model => model != null)
                .Take(1)
                .Subscribe(model =>
                {
                    // enemiesSpawnModelがnullでないときの処理を設定
                    sunMoonSystemModel.OnmyoState.ObserveEveryValueChanged(x => x.Value)
                        .Subscribe(x =>
                        {
                            sunMoonStateIconView.SetRotate(x);
                            // if (!model.SetOnmyoState(x))
                            //     Debug.LogError("SetOnmyoState");
                            if (!clearCountdownTimerCircleView.SetColor(x))
                                Debug.LogError("SetColor");
                        });
                });
        }
    }
}
