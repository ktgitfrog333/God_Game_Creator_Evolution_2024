using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Model;
using Main.View;
using UniRx;
using Universal.Template;
using System.Linq;

namespace Main.Common
{
    /// <summary>
    /// プレゼンタの共通処理
    /// </summary>
    public class MainPresenterCommon : IMainPresenterCommon
    {
        public bool IsFinalLevel()
        {
            return false;
        }
    }

    /// <summary>
    /// プレゼンタの共通処理
    /// インターフェース
    /// </summary>
    public interface IMainPresenterCommon
    {
        /// <summary>
        /// 最終ステージである
        /// または、各エリアの最終ステージかつシナリオ未読である
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool IsFinalLevel();
    }
}
