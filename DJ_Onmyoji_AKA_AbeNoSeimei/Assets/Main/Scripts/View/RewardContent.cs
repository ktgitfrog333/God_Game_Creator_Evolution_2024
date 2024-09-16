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
        /// <summary>フェードイメージ</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>スケールサイズの種類</summary>
        [SerializeField] private float[] scaleSizes = { 1f, 1.05f};
        /// <summary>式神タイプイメージ</summary>
        [SerializeField] private Sprite[] shikigamiTypeImages;
        /// <summary>レアイメージ</summary>
        [SerializeField] private Sprite[] rareImages;
        /// <summary>タイプアイコン</summary>
        [SerializeField] private Sprite[] typeIcons;
        /// <summary>名前末尾の文言</summary>
        [SerializeField] private string[] nameSuffix = { "-召喚", "-強化", "-強化" };
        /// <summary>スケール</summary>
        private Vector3? _scale;
        /// <summary>スケール</summary>
        public Vector3? Scale => _scale != null ? _scale : _scale = (Transform as RectTransform).localScale;

        public bool Check(bool isCheck)
        {
            try
            {
                if (isCheck)
                    return imagesGroup.SetEnabledByAlpha();
                else
                {
                    if (!imagesGroup.SetDisabledByAlpha())
                        throw new System.Exception("SetDisabledByAlpha");
                    if (!fadeImageView.SetFade(EnumFadeState.Close))
                        throw new System.Exception("SetFade");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayScalingAnimation(bool isScaleUp)
        {
            try
            {
                var size = scaleSizes[isScaleUp ? 1 : 0];
                var rect = Transform as RectTransform;
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
                if (!clearRewardImageContents[0].SetSprite(shikigamiTypeImages[(int)rewardContentProp.shikigamiType]))
                    throw new System.Exception("SetSprite");
                if (!clearRewardImageContents[1].SetSprite(rareImages[(int)rewardContentProp.rareType]))
                    throw new System.Exception("SetSprite");
                if (!clearRewardImageContents[2].SetSprite(rewardContentProp.image))
                    throw new System.Exception("SetSprite");
                if (!clearRewardImageContents[3].SetSprite(typeIcons[(int)rewardContentProp.rewardType]))
                    throw new System.Exception("SetSprite");
                if (!clearRewardTextContents.SetName($"{rewardContentProp.name}{nameSuffix[(int)rewardContentProp.rewardType]}"))
                    throw new System.Exception("SetName");
                if (!clearRewardTMPContents.SetSoulMoney(rewardContentProp.soulMoney))
                    throw new System.Exception("SetSoulMoney");
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool Disabled()
        {
            try
            {
                if (!imagesGroup.SetEnabledByAlpha())
                    throw new System.Exception("SetEnabledByAlpha");
                if (!fadeImageView.SetFade(EnumFadeState.Open))
                    throw new System.Exception("SetFade");
                
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
            fadeImageView = GetComponentInChildren<FadeImageView>();
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
        /// <summary>
        /// チェック無効
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool Disabled();
    }
}
