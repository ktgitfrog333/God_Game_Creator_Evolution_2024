using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 敵
    /// ビュー
    /// </summary>
    public class EnemyView : MonoBehaviour
    {
        /// <summary>ボディスプライトのビュー</summary>
        [SerializeField] private BodySpriteView bodySpriteView;
        /// <summary>終了時間</summary>
        [SerializeField] float[] durations = { 1.25f, 1.25f };
        /// <summary>スケールのパターン</summary>
        [SerializeField] float[] scales = { 1.1f, .9f };

        private void Reset()
        {
            bodySpriteView = GetComponentInChildren<BodySpriteView>();
        }

        private void OnEnable()
        {
            if (bodySpriteView != null)
                if (!bodySpriteView.PlayScalingLoopAnimation(durations, scales))
                    Debug.LogError("PlayScalingLoopAnimation");
        }
    }
}
