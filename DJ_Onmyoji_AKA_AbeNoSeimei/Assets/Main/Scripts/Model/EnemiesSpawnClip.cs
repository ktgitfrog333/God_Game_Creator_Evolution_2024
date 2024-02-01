using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Main.Model
{
    /// <summary>
    /// 敵をスポーンするプレイアブルのクリップ
    /// </summary>
    [System.Serializable]
    public class EnemiesSpawnClip : PlayableAsset
    {
        /// <summary>レベル内に存在する敵をスポーンするオブジェクト</summary>
        [SerializeField] private ExposedReference<GameObject> enemiesSpawnInLevel;
        /// <summary>敵のスポーンテーブル</summary>
        [SerializeField] private EnemiesSpawnTable enemiesSpawnTable;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            // ここでEnemiesSpawnBehaviourに必要な設定を行う
            var playable = ScriptPlayable<EnemiesSpawnBehaviour>.Create(graph);
            var model = enemiesSpawnInLevel.Resolve(graph.GetResolver()).GetComponent<EnemiesSpawnModel>();
            playable.GetBehaviour().EnemiesSpawnModel = model;
            playable.GetBehaviour().EnemiesSpawnTable = enemiesSpawnTable;

            return playable;
        }
    }

    /// <summary>
    /// 敵のスポーンに関するロジックをここに書く
    /// </summary>
    public class EnemiesSpawnBehaviour : PlayableBehaviour
    {
        /// <summary>敵をスポーン</summary>
        public EnemiesSpawnModel EnemiesSpawnModel { get; set; }
        /// <summary>敵のスポーンテーブル</summary>
        public EnemiesSpawnTable EnemiesSpawnTable { get; set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            DoIsApplicationPlaying(() =>
            {
                // シークバーが一定時間に達したかどうかをチェックする
                if (0 <= playable.GetTime())
                {
                    EnemiesSpawnModel.SetEnemiesSpawnTable(EnemiesSpawnTable);
                    if (!EnemiesSpawnModel.isActiveAndEnabled)
                        EnemiesSpawnModel.gameObject.SetActive(true);
                }
            });
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            DoIsApplicationPlaying(() =>
            {
                // プレイアブルが再生中でなく、前のフレームでプレイアブルが評価中（再生中）であったかどうかをチェックする。
                if (info.evaluationType == FrameData.EvaluationType.Playback)
                {
                    EnemiesSpawnModel.gameObject.SetActive(false);
                }
            });
        }

        public override void OnGraphStop(Playable playable)
        {
            DoIsApplicationPlaying(() => EnemiesSpawnModel.gameObject.SetActive(false));
        }

        /// <summary>
        /// ゲームアプリの実行中にのみ実行させる
        /// </summary>
        /// <param name="action">実行する処理</param>
        private void DoIsApplicationPlaying(System.Action action)
        {
            if (Application.isPlaying)
            {
                action();
            }
            else if (!Application.isPlaying)
            {
                Debug.LogWarning("ゲームがプレイ中でないため、敵のスポーンは実行されません。");
            }
        }
    }
}
