using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// チュートリアル
    /// ビュー
    /// </summary>
    public class TutorialView : MonoBehaviour
    {
        /// <summary>FungusのSayDialogを管理のビュー</summary>
        [SerializeField] private GuideMessageView guideMessageView;
        /// <summary>FungusのSayDialogを管理のビュー</summary>
        public GuideMessageView GuideMessageView => guideMessageView;
        /// <summary>ターンテーブル操作のガイド用UIのビュー</summary>
        [SerializeField] private GuideUITheTurntableView guideUITheTurntableView;
        /// <summary>ターンテーブル操作のガイド用UIのビュー</summary>
        public GuideUITheTurntableView GuideUITheTurntableView => guideUITheTurntableView;
        /// <summary>イコライザー操作のガイド用UIのビュー</summary>
        [SerializeField] private GuideUITheEqualizerView guideUITheEqualizerView;
        /// <summary>イコライザー操作のガイド用UIのビュー</summary>
        public GuideUITheEqualizerView GuideUITheEqualizerView => guideUITheEqualizerView;
        /// <summary>基本UIの説明（イコライザーゲージ）用UIのビュー</summary>
        [SerializeField] private GuideUITheEqualizerGageView guideUITheEqualizerGageView;
        /// <summary>基本UIの説明（イコライザーゲージ）用UIのビュー</summary>
        public GuideUITheEqualizerGageView GuideUITheEqualizerGageView => guideUITheEqualizerGageView;
        /// <summary>基本UIの説明（DJエネルギー）用UIのビュー</summary>
        [SerializeField] private GuideUITheDJEnergyView guideUITheDJEnergyView;
        /// <summary>基本UIの説明（DJエネルギー）用UIのビュー</summary>
        public GuideUITheDJEnergyView GuideUITheDJEnergyView => guideUITheDJEnergyView;
        /// <summary>基本UIの説明（プレイヤーHP）用UIのビュー</summary>
        [SerializeField] private GuideUIThePlayerHPView guideUIThePlayerHPView;
        /// <summary>基本UIの説明（プレイヤーHP）用UIのビュー</summary>
        public GuideUIThePlayerHPView GuideUIThePlayerHPView => guideUIThePlayerHPView;
        /// <summary>基本UIの説明（昼夜ゲージ）用UIのビュー</summary>
        [SerializeField] private GuideUITheClearCountdownTimerCircleView guideUITheClearCountdownTimerCircleView;
        /// <summary>基本UIの説明（昼夜ゲージ）用UIのビュー</summary>
        public GuideUITheClearCountdownTimerCircleView GuideUITheClearCountdownTimerCircleView => guideUITheClearCountdownTimerCircleView;
        /// <summary>基本UIの説明（魂(経験値)）用UIのビュー</summary>
        [SerializeField] private GuideUITheClearRewardTextContentsView guideUITheClearRewardTextContentsView;
        /// <summary>基本UIの説明（魂(経験値)）用UIのビュー</summary>
        public GuideUITheClearRewardTextContentsView GuideUITheClearRewardTextContentsView => guideUITheClearRewardTextContentsView;

        private void Reset()
        {
            // TODO: アタッチ
        }
    }
}
