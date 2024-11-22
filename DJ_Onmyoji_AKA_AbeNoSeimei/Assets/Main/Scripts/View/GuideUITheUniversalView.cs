using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 操作のガイド用UI
    /// ビュー
    /// </summary>
    public class GuideUITheUniversalView : MonoBehaviour
    {
        /// <summary>操作のガイド用UIの構造体</summary>
        [SerializeField] private GuideUITheUniversalStruct[] guideUITheUniversalStructs;
        /// <summary>シーケンス</summary>
        private Sequence _sequence;

        private void Reset()
        {
            var pages = GetComponentsInChildren<CanvasGroup>();
            List<GuideUITheUniversalStruct> structs = new List<GuideUITheUniversalStruct>();
            foreach (var page in pages.Select((p, i) => new { Content = p, Index = i }))
            {
                GuideUITheUniversalStruct @struct = new GuideUITheUniversalStruct()
                {
                    pageIndex = page.Index,
                    page = page.Content,
                    fadeTimeSec = .35f,
                    duration = 4.0f
                };
                structs.Add(@struct);
            }
            guideUITheUniversalStructs = structs.ToArray();
        }

        private void OnEnable()
        {
            if (_sequence != null)
            {
                _sequence.Restart();
            }
        }

        private void Start()
        {
            // ページが2枚以上ならループで切り替える
            if (guideUITheUniversalStructs != null &&
                1 < guideUITheUniversalStructs.Length)
            {
                // ChatGPT 4o
                // pageIndexの小さい順に並べ替え
                var sortedPages = guideUITheUniversalStructs.OrderBy(p => p.pageIndex).ToArray();
                // DOTweenのシーケンスを作成
                _sequence = DOTween.Sequence();
                foreach (var pageStruct in sortedPages)
                {
                    // フェードイン処理
                    _sequence.Append(pageStruct.page.DOFade(1.0f, pageStruct.fadeTimeSec)
                                .From(pageStruct.page.alpha = 0f))
                             .AppendInterval(pageStruct.duration)
                             .Append(pageStruct.page.DOFade(0.0f, pageStruct.fadeTimeSec));
                }
                // シーケンスをループさせる
                _sequence.SetLoops(-1, LoopType.Restart);

            }
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 操作のガイド用UI
    /// 構造体
    /// </summary>
    [System.Serializable]
    public struct GuideUITheUniversalStruct
    {
        /// <summary>ページ番号</summary>
        public int pageIndex;
        /// <summary>フェード時間</summary>
        public float fadeTimeSec;
        /// <summary>終了時間</summary>
        public float duration;
        /// <summary>ページ</summary>
        public CanvasGroup page;
    }
}
