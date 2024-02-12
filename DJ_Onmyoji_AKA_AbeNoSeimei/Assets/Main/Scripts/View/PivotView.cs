using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// ピボットパネル
    /// ビュー
    /// </summary>
    public class PivotView : MonoBehaviour, IPivotView
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        private Transform Transform => _transform != null ? _transform : _transform = transform;

        public Vector3 SetRotateOfIconRotateByState(float onmyoStateValue, IconRotateByState iconRotateByState)
        {
            if (onmyoStateValue < -1f ||
            1f < onmyoStateValue)
                throw new System.ArgumentOutOfRangeException("onmyoStateValue", "onmyoStateValue must be in the range of -1 to 1.");

            Vector3 result;
            if (onmyoStateValue == -1f)
                result = iconRotateByState.night;
            else if (onmyoStateValue < 0f)
                result = Vector3.Lerp(iconRotateByState.night, iconRotateByState.dayAndNight, onmyoStateValue + 1);
            else if (onmyoStateValue == 0f)
                result = iconRotateByState.dayAndNight;
            else // 0 < onmyoStateValue <= 1
                result = Vector3.Lerp(iconRotateByState.dayAndNight, iconRotateByState.daytime, onmyoStateValue);
            Transform.eulerAngles = result;

            return result;
        }
    }

    /// <summary>
    /// ピボットパネル
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IPivotView
    {
        /// <summary>
        /// 角度をセット
        /// </summary>
        /// <param name="onmyoStateValue">陰陽（昼夜）の状態</param>
        /// <param name="iconRotateByState">陰陽（昼夜）の状態によるアイコン角度</param>
        /// <returns>変更後の角度</returns>
        public Vector3 SetRotateOfIconRotateByState(float onmyoStateValue, IconRotateByState iconRotateByState);
    }
}
