using System.Collections;
using System.Collections.Generic;
using Main.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// クリア報酬のコンテンツ
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ClearRewardImageContents : MonoBehaviour, IClearRewardImageContents
    {
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;
        /// <summary>Mainのユーティリティ</summary>
        private MainViewUtility _mainViewUtility = new MainViewUtility();

        public bool SetSprite(Sprite sprite)
        {
            return _mainViewUtility.SetSpriteOfImage(image, sprite);
        }

        private void Reset()
        {
            image = GetComponent<Image>();
        }
    }

    /// <summary>
    /// クリア報酬のコンテンツ
    /// インターフェース
    /// </summary>
    public interface IClearRewardImageContents
    {
        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="sprite">スプライト</param>
        /// <returns>成功／失敗</returns>
        public bool SetSprite(Sprite sprite);
    }
}
