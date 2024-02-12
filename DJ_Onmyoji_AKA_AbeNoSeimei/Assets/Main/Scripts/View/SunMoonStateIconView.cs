using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 陰陽（昼夜）のアイコン
    /// </summary>
    public class SunMoonStateIconView : MonoBehaviour, ISunMoonStateIconView
    {
        /// <summary>陰陽（昼夜）の状態によるアイコン角度</summary>
        [SerializeField] private IconRotateByState iconRotateByState;
        /// <summary>ピボットパネルのビュー</summary>
        [SerializeField] private PivotView pivotView;

        public Vector3 SetRotate(float onmyoStateValue)
        {
            return pivotView.SetRotateOfIconRotateByState(onmyoStateValue, iconRotateByState);
        }

        private void Reset()
        {
            iconRotateByState.daytime = new Vector3(0f, 0f, -65f);
            iconRotateByState.dayAndNight = Vector3.zero;
            iconRotateByState.night = new Vector3(0f, 0f, 65f);
            pivotView = GetComponentInChildren<PivotView>();
        }
    }

    /// <summary>
    /// 陰陽（昼夜）の状態によるアイコン角度
    /// </summary>
    [System.Serializable]
    public struct IconRotateByState
    {
        /// <summary>昼</summary>
        public Vector3 daytime;
        /// <summary>昼夜</summary>
        public Vector3 dayAndNight;
        /// <summary>夜</summary>
        public Vector3 night;
    }

    /// <summary>
    /// 陰陽（昼夜）のアイコン
    /// インタフェース
    /// </summary>
    public interface ISunMoonStateIconView
    {
        /// <summary>
        /// 角度をセット
        /// </summary>
        /// <param name="onmyoStateValue">陰陽（昼夜）の状態</param>
        /// <returns>変更後の角度</returns>
        public Vector3 SetRotate(float onmyoStateValue);
    }
}
