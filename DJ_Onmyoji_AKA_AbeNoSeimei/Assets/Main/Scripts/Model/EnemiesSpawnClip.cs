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
        /// <summary>
        /// 敵のスポーンテーブル
        /// 但し、クリップの作成において下記のルールに従う
        /// 1.昼トラックと夜トラックで全体の再生時間は統一させること
        /// 2.生成する時間間隔 % 全体の再生時間 == 0 となること（余り無しとすること）
        ///     a.余りがあっても良いが末尾のシークエンス到達 ⇒ 次のシークエンス再生のタイミングで敵が生成されない時間が僅かに生まれる
        /// </summary>
        [SerializeField] private EnemiesSpawnTable enemiesSpawnTable;
        /// <summary>スポーン位置の固定化フラグ　TRUEの場合はスポーン位置を固定化する</summary>
        [SerializeField] private bool isSpawnPositionLock = false;
        /// <summary>スポーン位置の固定化時の角度の変化量</summary>
        [SerializeField] private float changeDegree = 0f;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            // ここでEnemiesSpawnBehaviourに必要な設定を行う
            var playable = ScriptPlayable<EnemiesSpawnBehaviour>.Create(graph);
            var model = enemiesSpawnInLevel.Resolve(graph.GetResolver()).GetComponent<EnemiesSpawnModel>();
            playable.GetBehaviour().EnemiesSpawnModel = model;
            playable.GetBehaviour().EnemiesSpawnTable = enemiesSpawnTable;
            playable.GetBehaviour().isSpawnPositionLock = isSpawnPositionLock;
            playable.GetBehaviour().spawnRadius = changeDegree;

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
        /// <summary>スポーン位置の固定化フラグ　TRUEの場合はスポーン位置を固定化する</summary>
        public bool isSpawnPositionLock { get; set; }
        /// <summary>スポーン位置の固定化時の角度の変化量</summary>
        public float spawnRadius { get; set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            DoIsApplicationPlaying(() =>
            {
                // シークバーが一定時間に達したかどうかをチェックする
                if (0 <= playable.GetTime())
                {
                    EnemiesSpawnModel.SetEnemiesSpawnTable(EnemiesSpawnTable);
                    if (!EnemiesSpawnModel.isActiveAndEnabled)
                    {
                        EnemiesSpawnModel.gameObject.SetActive(true);
                        EnemiesSpawnModel.SetSpawnLock(isSpawnPositionLock, spawnRadius);
                    }
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
