using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Main.Common;
using UniRx;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// クリア報酬のコンテンツ
    /// </summary>
    public class RewardContent : MonoBehaviour, IRewardContent
    {
        /// <summary>テキスト表示クリア報酬のコンテンツ</summary>
        [SerializeField] private ClearRewardTextContents clearRewardTextContents;
        /// <summary>イメージ表示クリア報酬のコンテンツ</summary>
        [SerializeField] private ClearRewardImageContents[] clearRewardImageContents;
        /// <summary>テキスト表示クリア報酬のコンテンツ</summary>
        [SerializeField] private ClearRewardTMPContents clearRewardTMPContents;
        /// <summary>イメージ制御</summary>
        [SerializeField] private ImagesGroup imagesGroup;
        /// <summary>チェック状態か</summary>
        public IReactiveProperty<bool> IsChecked => imagesGroup.IsChecked;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>スケールサイズの種類</summary>
        [SerializeField] private float[] scaleSizes = { 1f, 1.05f};
        /// <summary>タイプアイコン</summary>
        [SerializeField] private Sprite[] typeIcons;
        /// <summary>名前末尾の文言</summary>
        [SerializeField] private string[] nameSuffix = { "-召喚", "-強化" };
        private Vector2? _sizeDelta;
        public Vector2? SizeDelta => _sizeDelta != null ? _sizeDelta : _sizeDelta = (Transform as RectTransform).sizeDelta;
        private Vector3? _scale;
        public Vector3? Scale => _scale != null ? _scale : _scale = (Transform as RectTransform).localScale;

        public bool Check(bool isCheck)
        {
            return isCheck ? imagesGroup.SetEnabledByAlpha() : imagesGroup.SetDisabledByAlpha();
        }

        public bool PlayScalingAnimation(bool isScaleUp)
        {
            try
            {
                var size = scaleSizes[isScaleUp ? 1 : 0];
                var rect = Transform as RectTransform;
                // rect.sizeDelta = SizeDelta.Value * size;
                rect.localScale = Scale.Value * size;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetContents(RewardContentProp rewardContentProp)
        {
            try
            {
                if (!clearRewardImageContents[0].SetSprite(rewardContentProp.image))
                    throw new System.Exception("SetSprite");
                if (!clearRewardImageContents[1].SetSprite(typeIcons[(int)rewardContentProp.rewardType]))
                    throw new System.Exception("SetSprite");
                if (!clearRewardTextContents.SetName($"{rewardContentProp.name}{nameSuffix[(int)rewardContentProp.rewardType]}"))
                    throw new System.Exception("SetName");
                if (!clearRewardTMPContents.SetSoulMoney(rewardContentProp.soulMoney))
                    throw new System.Exception("SetSoulMoney");
                if (!imagesGroup.SetEnabledByAlpha())
                    throw new System.Exception("SetEnabledByAlpha");
                
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
            clearRewardTextContents = GetComponentInChildren<ClearRewardTextContents>();
            clearRewardImageContents = GetComponentsInChildren<ClearRewardImageContents>();
            clearRewardTMPContents = GetComponentInChildren<ClearRewardTMPContents>();
            imagesGroup = GetComponentInChildren<ImagesGroup>();
        }

        private void Start()
        {
            // 最初からチェック状態にはしないため外す
            imagesGroup.SetDisabledByAlpha();
        }
    }

    /// <summary>
    /// クリア報酬のコンテンツ
    /// インターフェース
    /// </summary>
    public interface IRewardContent
    {
        /// <summary>
        /// コンテンツをセット
        /// </summary>
        /// <param name="rewardContentProp">リワード情報</param>
        /// <returns>成功／失敗</returns>
        public bool SetContents(RewardContentProp rewardContentProp);
        /// <summary>
        /// スケールを変化させるアニメーションを再生
        /// </summary>
        /// <param name="isScaleUp">大きくするか</>
        /// <returns>成功／失敗</returns>
        public bool PlayScalingAnimation(bool isScaleUp);
        /// <summary>
        /// チェック状態の可否
        /// </summary>
        /// <param name="isCheck">チェック有効</param>
        /// <returns>成功／失敗</returns>
        public bool Check(bool isCheck);
    }
}
