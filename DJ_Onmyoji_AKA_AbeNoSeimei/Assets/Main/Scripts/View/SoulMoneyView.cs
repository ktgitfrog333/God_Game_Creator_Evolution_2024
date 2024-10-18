using System.Collections;
using System.Collections.Generic;
using Main.Model;
using Main.Utility;
using UniRx;
using UnityEngine;
using Universal.Utility;
using DG.Tweening;

namespace Main.View
{
    /// <summary>
    /// 魂の経験値
    /// ビュー
    /// </summary>
    public class SoulMoneyView : MonoBehaviour, ISoulMoneyView
    {
        /// <summary>アニメーション終了時間</summary>
        [SerializeField] private float[] durations = { 1.5f, 1.5f };
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>トランスフォーム</summary>
        public Transform Transform => _transform != null ? _transform : _transform = transform;
        /// <summary>ボディスプライトのビュー</summary>
        [SerializeField] private BodySpriteView bodySpriteView;
        /// <summary>獲得済みか</summary>
        public IReactiveProperty<bool> IsGeted { get; private set; } = new BoolReactiveProperty();
        /// <summary>敵のプロパティ</summary>
        private EnemiesProp _enemiesProp;
        /// <summary>敵のプロパティ</summary>
        public EnemiesProp EnemiesProp => _enemiesProp;
        /// <summary>TargetのPosition</summary>
        [SerializeField] private Vector3 _targetPosition;
        /// <summary>魂の財布、獲得したソウルの管理のモデル</summary>
        private SoulWalletModel soulWalletModel;

        private void Reset()
        {
            bodySpriteView = GetComponentInChildren<BodySpriteView>();
        }

        private void OnEnable()
        {
            IsGeted.Value = false;
            Vector3 newPosition = Transform.position + Random.onUnitSphere * 1.5f;
            transform.DOMove(newPosition, 0.5f).SetEase(Ease.OutQuint).OnComplete(FirstMoveComplete);
        }

        private void FirstMoveComplete()
        {
            transform.DOMove(_targetPosition, 2f).SetEase(Ease.InQuad).OnComplete(MoveComplete);
        }

        private void MoveComplete()
        {
            // 到着後の処理
            IsGeted.Value = true;
            gameObject.SetActive(false);

            soulWalletModel.AddSoulMoney(_enemiesProp.soulMoneyPoint);
        }

        private void Start()
        {
            gameObject.SetActive(false);
            soulWalletModel = GameObject.Find("SoulWallet").GetComponent<SoulWalletModel>();
        }

        public bool SetEnemiesProp(EnemiesProp enemiesProp)
        {
            try
            {
                _enemiesProp = enemiesProp;

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
    /// 魂の経験値
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface ISoulMoneyView
    {
        /// <summary>
        /// 敵のプロパティをセット
        /// </summary>
        /// <param name="enemiesProp">敵のプロパティ</param>
        /// <returns>成功／失敗</returns>
        public bool SetEnemiesProp(EnemiesProp enemiesProp);
    }
}
