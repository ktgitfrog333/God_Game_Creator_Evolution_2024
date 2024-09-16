using Effect.Model;
using Effect.Utility;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Main.View
{
    /// <summary>
    /// 敵
    /// ビュー
    /// </summary>
    public class EnemyView : MonoBehaviour, IEnemyView
    {
        /// <summary>ボディスプライトのビュー</summary>
        [SerializeField] private BodySpriteView bodySpriteView;
        /// <summary>終了時間</summary>
        [SerializeField] float[] durations = { 1.25f, 1.25f };
        /// <summary>スケールのパターン</summary>
        [SerializeField] float[] scales = { 1.1f, .9f };
        /// <summary>アニメータが存在するか</summary>
        public bool IsFoundAnimator => animatorView != null;
        /// <summary>アニメータのビュー</summary>
        [SerializeField] private AnimatorView animatorView;
        /// <summary>エフェクトプールプレハブ</summary>
        [SerializeField] private Transform effectsPoolPrefab;
        /// <summary>エフェクトユーティリティ</summary>
        private EffectUtility _effectUtility = new EffectUtility();
        /// <summary>エフェクトプールモデル</summary>
        private EffectsPoolModel _effectsPoolModel = new EffectsPoolModel();
        /// <summary>敵のヒットエフェクト</summary>
        private Transform _hitEffect;
        /// <summary>敵がやられた時のエフェクト</summary>
        private Transform _enemyDownEffect;
        /// <summary>HPバースプライト</summary>
        [SerializeField] private SpriteRenderer hpSprite;
        /// <summary>HPバーゲージスプライト</summary>
        [SerializeField] private SpriteRenderer hpSpriteGauge;
        /// <summary>フェードアウト時間</summary>
        [SerializeField] private float fadeDuration = 2.0f;
        /// <summary>フェードアウト用コルーチン</summary>
        private Coroutine currentCoroutine;
        /// <summary>スプライトカラー</summary>
        private Color spriteColor;

        private void Reset()
        {
            bodySpriteView = GetComponentInChildren<BodySpriteView>();
            if (GetComponentInChildren<AnimatorView>() != null)
                animatorView = GetComponentInChildren<AnimatorView>();
        }

        private void OnEnable()
        {
            if (bodySpriteView != null)
                if (!bodySpriteView.PlayScalingLoopAnimation(durations, scales))
                    Debug.LogError("PlayScalingLoopAnimation");

            if (hpSprite != null && hpSpriteGauge != null)
            {
                hpSprite.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
                hpSprite.transform.localPosition = new Vector3(0f, -0.5f, 0f);
                spriteColor = hpSprite.color;
                spriteColor.a = 0f;
                hpSprite.color = spriteColor;
                hpSpriteGauge.color = spriteColor;
            }
        }

        private void Start()
        {
            // エフェクトプールからエフェクトを取得して再生させる
            _effectsPoolModel = _effectUtility.FindOrInstantiateForGetEffectsPoolModel(effectsPoolPrefab);
            _effectsPoolModel.IsCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    _hitEffect = _effectsPoolModel.GetHitEffect();
                    _enemyDownEffect = _effectsPoolModel.GetEnemyDownEffect();
                });
        }

        public bool PlayWalkingAnimation(float moveSpeed)
        {
            return animatorView.SetFloat(ParametersOfAnim.MoveSpeed, moveSpeed);
        }

        public bool PlayHitEffect()
        {
            try
            {
                if (_hitEffect == null)
                    return true;

                var particleSystems = _hitEffect.GetComponentsInChildren<ParticleSystem>();
                _hitEffect.position = transform.position;
                _hitEffect.gameObject.SetActive(true);
                foreach (var particleSystem in particleSystems)
                    particleSystem.Play();
                Observable.FromCoroutine(() => _effectsPoolModel.WaitForAllParticlesToStop(particleSystems))
                    .Subscribe(_ => _hitEffect.gameObject.SetActive(false))
                    .AddTo(gameObject);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayEnemyDownEffect()
        {
            try
            {
                if (_enemyDownEffect == null)
                    return true;

                var particleSystems = _enemyDownEffect.GetComponentsInChildren<ParticleSystem>();
                _enemyDownEffect.position = transform.position;
                _enemyDownEffect.gameObject.SetActive(true);
                foreach (var particleSystem in particleSystems)
                    particleSystem.Play();
                Observable.FromCoroutine(() => _effectsPoolModel.WaitForAllParticlesToStop(particleSystems))
                    .Subscribe(_ => _enemyDownEffect.gameObject.SetActive(false))
                    .AddTo(gameObject);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void SetHpBar(float hp, float maxHp)
        {
            float hpBar = hp / maxHp * 0.2f;

            if (hpSprite != null && hpSpriteGauge != null)
            {
                float originalWidth = hpSprite.bounds.size.x;
                hpSprite.transform.localScale = new Vector3(hpBar, 0.2f, 1.0f);
                float newWidth = hpSprite.bounds.size.x;
                float widthDifference = originalWidth - newWidth;
                hpSprite.transform.position -= new Vector3(widthDifference / 2, 0, 0);

                spriteColor = hpSprite.color;
                spriteColor.a = 1.0f;
                hpSprite.color = spriteColor;
                hpSpriteGauge.color = spriteColor;

                // すでにコルーチンが実行中なら停止
                if (currentCoroutine != null)
                    StopCoroutine(currentCoroutine);

                currentCoroutine = StartCoroutine(FadeOut());
            }
        }

        private IEnumerator FadeOut()
        {
            // 現在の色を取得
            spriteColor = hpSprite.color;

            // フェードアウトにかける時間に基づいて1秒あたりどれくらいアルファ値を減らすかを計算
            float fadeAmountPerFrame = Time.deltaTime / fadeDuration;

            while (spriteColor.a > 0)
            {
                // アルファ値を徐々に減らす
                spriteColor.a -= fadeAmountPerFrame;
                hpSprite.color = spriteColor;
                hpSpriteGauge.color = spriteColor;

                // 次のフレームまで待機
                yield return null;
            }

            // 完全に透明にする
            spriteColor.a = 0;
            hpSprite.color = spriteColor;
        }
    }

    /// <summary>
    /// 敵
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IEnemyView : IMobView
    {
        /// <summary>
        /// ヒットエフェクトを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayHitEffect();
        /// <summary>
        /// 敵がやられた時のエフェクトを再生
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool PlayEnemyDownEffect();
    }

    /// <summary>
    /// モブ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IMobView
    {
        /// <summary>
        /// 歩くアニメーションを再生
        /// </summary>
        /// <param name="moveSpeed">移動速度</param>
        /// <returns>成功／失敗</returns>
        public bool PlayWalkingAnimation(float moveSpeed);
    }
}
