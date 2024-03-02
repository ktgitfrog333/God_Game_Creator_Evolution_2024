using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// イメージ制御
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class ImagesGroup : MonoBehaviour, IImagesGroup
    {
        /// <summary>キャンバスグループ</summary>
        [SerializeField] private CanvasGroup canvasGroup;
        /// <summary>チェック状態か</summary>
        public IReactiveProperty<bool> IsChecked { get; private set; } = new BoolReactiveProperty();

        public bool SetDisabledByAlpha()
        {
            try
            {
                canvasGroup.alpha = 0f;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetEnabledByAlpha()
        {
            try
            {
                canvasGroup.alpha = 1f;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            IsChecked.Value = canvasGroup.alpha == 1f;
        }
    }

    /// <summary>
    /// イメージ制御
    /// インターフェース
    /// </summary>
    public interface IImagesGroup
    {
        /// <summary>
        /// アルファ値の調整により有効化
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetEnabledByAlpha();
        /// <summary>
        /// アルファ値の調整により無効化
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool SetDisabledByAlpha();
    }
}
