using Main.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// レベル背景（昼／夜）用
    /// ビュー
    /// </summary>
    public class FadeLevelBackgroundView : FadeImageView
    {
        /// <summary>レベル背景プロパティ</summary>
        [SerializeField] private FadeLevelBackgroundProp[] fadeLevelBackgroundProps;

        private void Start()
        {
            MainCommonUtility mainCommonUtility = new MainCommonUtility();
            var userBean = mainCommonUtility.UserDataSingleton.UserBean;
            var sourceImage = fadeLevelBackgroundProps.Where(q => q.stageId == userBean.sceneId)
                .Select(q => q.sprite)
                .ToArray();
            if (sourceImage.Length < 1 ||
                1 < sourceImage.Length)
                Debug.LogError($"不正なステージID: {userBean.sceneId}");
            else
                image.sprite = sourceImage[0];
        }
    }

    /// <summary>
    /// レベル背景
    /// プロパティ
    /// </summary>
    [System.Serializable]
    public struct FadeLevelBackgroundProp
    {
        /// <summary>ステージID</summary>
        public int stageId;
        /// <summary>スプライト</summary>
        public Sprite sprite;
    }
}
