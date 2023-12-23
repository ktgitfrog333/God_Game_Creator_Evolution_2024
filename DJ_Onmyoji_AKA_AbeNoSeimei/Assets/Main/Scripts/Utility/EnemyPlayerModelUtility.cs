using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Main.Utility
{
    /// <summary>
    /// 敵とプレイヤーのユーティリティ
    /// </summary>
    public class EnemyPlayerModelUtility : IEnemyPlayerModelUtility
    {
        public bool IsCompareTagAndUpdateReactiveFlag(Collider2D other, string[] tags, IReactiveProperty<bool> isHit)
        {
            return 0 < tags.Where(q => other.CompareTag(q))
            .Select(q => q)
            .ToArray()
            .Length &&
            !isHit.Value;
        }
    }

    /// <summary>
    /// 敵とプレイヤーのユーティリティ
    /// インターフェース
    /// </summary>
    public interface IEnemyPlayerModelUtility
    {
        /// <summary>
        /// 対象がタグ内に含まれている場合はフラグを更新
        /// </summary>
        /// <param name="other">衝突した対象</param>
        /// <param name="tags">対象タグ</param>
        /// <param name="isHit">ヒットフラグ</param>
        /// <returns></returns>
        public bool IsCompareTagAndUpdateReactiveFlag(Collider2D other, string[] tags, IReactiveProperty<bool> isHit);
    }
}
