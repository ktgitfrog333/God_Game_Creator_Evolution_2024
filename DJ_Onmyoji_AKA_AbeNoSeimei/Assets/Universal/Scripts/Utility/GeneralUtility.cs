using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Utility
{
    /// <summary>
    /// 汎用ユーティリティ
    /// </summary>
    public static class GeneralUtility
    {
        /// <summary>
        /// 遅延時間後にメソッドを実行
        /// コルーチンのためStartCoroutineまたはFromCoroutineを使用すること
        /// </summary>
        /// <param name="delay">遅延時間</param>
        /// <param name="action">メソッド</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator ActionsAfterDelay(float delay, System.Action action)
        {
            if (Time.timeScale == 0f)
                Debug.LogError("timeScaleが0の場合はコルーチンが実行されません");
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}
