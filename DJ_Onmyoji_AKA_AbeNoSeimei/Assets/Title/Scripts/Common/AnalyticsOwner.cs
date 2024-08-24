using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
using System.Reflection;
using Unity.Services.Core;
using UniRx;

namespace Title.Common
{
    /// <summary>
    /// アナリティクス
    /// UnityCould > Analytics > Analyze > Dashboards > Game performance
    /// UnityCould > Analytics > Analyze > Custom Dashboards
    /// </summary>
    /// <see href="https://cloud.unity.com/home/organizations/15668132550890/projects/912df126-cafc-4ebc-bd7b-0351443d5411/environments/2e0bd5da-1608-4e0b-821d-9ef127d69b10/analytics/v2/dashboards/game-performance"/>
    public class AnalyticsOwner : MonoBehaviour, IAnalyticsOwner
    {
        /// <summary>初期化の成功</summary>
        public IReactiveProperty<bool> InitializeAsyncSuccessed { get; private set; } = new BoolReactiveProperty();

        private void Awake()
        {
            UnityServices.InitializeAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Initialization failed, handle the error
                    Debug.LogWarning("UnityServices初期化の失敗");
                }
                else
                {
                    // Initialization succeeded, you can now use Unity Services
                    InitializeAsyncSuccessed.Value = true;
                    Debug.Log("UnityServices初期化の成功");
                }
            });
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// シーンがロードされた場合
        /// イベントフック
        /// </summary>
        /// <param name="scene">シーン情報</param>
        /// <param name="mode">ロードするシーンモード</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // このメソッドのみシーンロード前にイベントフックするため書き方が異なる
            var currentMethodName = MethodBase.GetCurrentMethod().Name;
            InitializeAsyncSuccessed.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>()
                        {
                            { ParameterNameEventparams.CLIENTVERSION, Application.version },
                            { ParameterNameEventparams.PLATFORM, Application.platform.ToString() },
                            { ParameterNameEventparams.SCENENAME, scene.name },
                        };

                        AnalyticsService.Instance.CustomData(currentMethodName, parameters);
                    }
                });

        }

        public bool OnUpdateEventState(int eventState, string gameObjectName)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { ParameterNameEventparams.CLIENTVERSION, Application.version },
                    { ParameterNameEventparams.PLATFORM, Application.platform.ToString() },
                    { ParameterNameEventparams.EVENTSTATE, eventState },
                    { ParameterNameEventparams.GAMEOBJECT_NAME, gameObjectName },
                };

                var currentMethodName = MethodBase.GetCurrentMethod().Name;
                AnalyticsService.Instance.CustomData(currentMethodName, parameters);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// Event Manager内のカスタムイベントのパラメータ情報
        /// UnityCould > Analytics > Manage > Event Manager > Events > カスタムイベント
        /// </summary>
        /// <see href="https://cloud.unity.com/home/organizations/15668132550890/projects/912df126-cafc-4ebc-bd7b-0351443d5411/environments/2e0bd5da-1608-4e0b-821d-9ef127d69b10/analytics/v2/events"/>
        public class ParameterNameEventparams
        {
            /// <summary>クライアントバージョン（Project Settings > Player > Version）</summary>
            public static readonly string CLIENTVERSION = "clientVersion";
            /// <summary>プラットフォーム（Windows／Mac）</summary>
            public static readonly string PLATFORM = "platform";
            /// <summary>シーン名</summary>
            public static readonly string SCENENAME = "sceneName";
            /// <summary>イベント状態</summary>
            public static readonly string EVENTSTATE = "eventState";
            /// <summary>ゲームオブジェクト名</summary>
            public static readonly string GAMEOBJECT_NAME = "gameObjectName";
        }
    }

    /// <summary>
    /// アナリティクス
    /// インタフェース
    /// </summary>
    public interface IAnalyticsOwner
    {
        /// <summary>
        /// UIイベントが更新された際に発火。イベント状態を取得。
        /// </summary>
        /// <param name="eventState">イベント状態</param>
        /// <param name="gameObjectName">ゲームオブジェクト名</param>
        /// <returns>成功／失敗</returns>
        public bool OnUpdateEventState(int eventState, string gameObjectName);
    }
}
