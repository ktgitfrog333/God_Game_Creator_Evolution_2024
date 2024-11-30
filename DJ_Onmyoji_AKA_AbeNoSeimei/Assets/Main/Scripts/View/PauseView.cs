using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// ビュー
    /// ポーズ画面
    /// </summary>
    public class PauseView : MonoBehaviour, IPauseView
    {
        /// <summary>閉じるまでの時間</summary>
        [SerializeField] private float closedTime = .5f;
        /// <summary>操作の許可、禁止</summary>
        private bool _isControllEnabled = true;
        /// <summary>操作の許可、禁止</summary>
        public bool IsControllEnabled => _isControllEnabled;

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public IEnumerator PlayCloseAnimation(System.IObserver<bool> observer)
        {
            DOVirtual.DelayedCall(closedTime, () =>
            {
                observer.OnNext(true);
            });
            yield return null;
        }

        public bool SetControllEnabled(bool isControllEnabled)
        {
            try
            {
                _isControllEnabled = isControllEnabled;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }

    /// <summary>
    /// ポーズ画面
    /// インターフェース
    /// </summary>
    public interface IPauseView
    {
        /// <summary>
        /// 操作の許可、禁止を切り替える
        /// </summary>
        /// <param name="isControllEnabled">操作の許可、禁止</param>
        /// <returns>成功／失敗</returns>
        public bool SetControllEnabled(bool isControllEnabled);
    }
}
