using System.Collections;
using System.Collections.Generic;
using Main.Audio;
using Main.Common;
using Main.Utility;
using UnityEngine;
using UnityEngine.UI;
using Universal.Common;

namespace Main.View
{
    /// <summary>
    /// ペンダグラムターンテーブル
    /// プレゼンタから伝達された入力を元に出力を行う
    /// Imageコンポーネントへ入力操作を行う
    /// ビュー
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class PentagramTurnTableCommonView : MonoBehaviour
    {
        /// <summary>ターンテーブル</summary>
        [SerializeField] protected Image image;
        /// <summary>オーディオオーナー</summary>
        private AudioOwner _audioOwner;
        /// <summary>オーディオオーナー</summary>
        protected AudioOwner AudioOwner
        {
            get
            {
                if (_audioOwner != null)
                    return _audioOwner;
                else
                {
                    _audioOwner = MainGameManager.Instance.AudioOwner;
                    return _audioOwner;
                }
            }
        }

        /// <summary>回転制御においてスティック入力感度の補正値小さいほど鈍く回転して、大きいほど素早く回転する。</summary>
        [SerializeField, Range(.5f, 10f)] protected float angleCorrectionValue;
        /// <summary>回転制御においてスティック入力感度の補正値小さいほど鈍く回転して、大きいほど素早く回転する。</summary>
        public float AngleCorrectionValue
        {
            get { return angleCorrectionValue; }
            set { angleCorrectionValue = value; }
        }
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        protected Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>スプライト</summary>
        [SerializeField] protected Sprite[] sprites;
        /// <summary>ユーティリティ</summary>
        protected MainViewUtility _utility = new MainViewUtility();
        protected virtual void Reset()
        {
            image = GetComponent<Image>();
        }

        protected virtual void Start()
        {
            var adminDataSingleton = AdminDataSingleton.Instance != null ?
                AdminDataSingleton.Instance :
                new GameObject(Universal.Common.ConstGameObjectNames.GAMEOBJECT_NAME_ADMINDATA_SINGLETON).AddComponent<AdminDataSingleton>()
                    .GetComponent<AdminDataSingleton>();
            angleCorrectionValue = adminDataSingleton.AdminBean.pentagramTurnTableView.angleCorrectionValue;
        }
    }
}
