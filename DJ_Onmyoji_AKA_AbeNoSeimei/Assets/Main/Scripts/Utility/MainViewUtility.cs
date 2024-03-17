using System.Collections;
using System.Collections.Generic;
using Main.Common;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Main.Utility
{
    /// <summary>
    /// Mainのユーティリティ
    /// ビュー
    /// </summary>
    public class MainViewUtility : IMainViewUtility
    {
        /// <summary>デフォルト表示</summary>
        private readonly float DEFAULT = 1f;
        /// <summary>ループアニメーションSeqence管理</summary>
        private struct LoopAnimations
        {
            /// <summary>スケーリングするDOTweenアニメーション再生</summary>
            public Sequence scaling;
        }
        /// <summary>ループアニメーションSeqence管理</summary>
        private LoopAnimations _loopAnimations = new LoopAnimations();

        public IEnumerator PlayFadeAnimation<T>(System.IObserver<bool> observer, EnumFadeState state, float duration, T component) where T : Component
        {
            // componentがImageかSpriteRendererかをチェック
            if (component is Image image)
            {
                // Imageの場合の処理
                image.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 0f : 1f, duration)
                    .SetUpdate(true)
                    .OnComplete(() => observer.OnNext(true));
            }
            else if (component is SpriteRenderer spriteRenderer)
            {
                // SpriteRendererの場合の処理
                spriteRenderer.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 1f : 0f, duration)
                    .From(state.Equals(EnumFadeState.Open) ? 0f : 1f)
                    .SetUpdate(true)
                    .OnComplete(() => observer.OnNext(true));
            }
            else
            {
                throw new System.ArgumentException("Unsupported component type. Only Image and SpriteRenderer are supported.", nameof(component));
            }

            yield return null;
        }

        public bool SetFade<T>(EnumFadeState state, T component) where T : Component
        {
            try
            {
                // componentがImageかSpriteRendererかをチェック
                if (component is Image image)
                {
                    // Imageの場合の処理
                    image.color = new Color(image.color.r, image.color.g, image.color.b, state.Equals(EnumFadeState.Open) ? 0f : 1f);
                }
                else if (component is SpriteRenderer spriteRenderer)
                {
                    // TODO:SpriteRendererの場合の処理を実装
                    throw new System.NotImplementedException();
                }
                else
                {
                    throw new System.ArgumentException("Unsupported component type. Only Image and SpriteRenderer are supported.", nameof(component));
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool PlayScalingLoopAnimation(float[] durations, float[] scales, Transform transform)
        {
            try
            {
                if (_loopAnimations.scaling == null)
                {
                    transform.localScale = Vector3.one * scales[1];
                    _loopAnimations.scaling = DOTween.Sequence()
                        .Append(transform.DOScale(scales[0], durations[0]))
                        .Append(transform.DOScale(scales[1], durations[1]))
                        .SetLoops(-1, LoopType.Restart);
                    _loopAnimations.scaling.Play();
                }
                else
                {
                    transform.localScale = Vector3.one * scales[1];
                    _loopAnimations.scaling.Restart();
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null)
        {
            try
            {
                if (maskAngle < 0f || 1f < maskAngle)
                    throw new System.Exception("不正な値セット:0fから1fの値を設定して下さい");
                else if (0f < maskAngle && transform == null)
                    throw new System.Exception("不正な値セット:maskAngle設定時はtransformも設定が必要です");

                float baseAmount = GetRate(timeSec, limitTimeSecMax);
                float calc = System.Math.Max(0f, maskAngle);
                image.fillAmount = baseAmount * (1f - calc);
                if (transform != null)
                    transform.eulerAngles = new Vector3(0f, 0f, 360f) * (calc * .5f);
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public IEnumerator PlayFillAmountAndColorOfImage(System.IObserver<bool> observer, float[] durations, Color32 dangerousColor, Image image, float timeSec, float limitTimeSecMax, float maskAngle = 0, Transform transform = null)
        {
            try
            {
                if (maskAngle < 0f || 1f < maskAngle)
                    throw new System.Exception("不正な値セット:0fから1fの値を設定して下さい");
                else if (0f < maskAngle && transform == null)
                    throw new System.Exception("不正な値セット:maskAngle設定時はtransformも設定が必要です");

                float baseAmount = GetRate(timeSec, limitTimeSecMax);
                float calc = System.Math.Max(0f, maskAngle);
                image.DOFillAmount(baseAmount * (1f - calc), durations[0])
                    .OnComplete(() => observer.OnNext(true));
                image.DOColor(dangerousColor, durations[1])
                    .From(Color.white)
                    .SetLoops(-1, LoopType.Yoyo);

                if (transform != null)
                    transform.eulerAngles = new Vector3(0f, 0f, 360f) * (calc * .5f);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                observer.OnError(e);
            }

            yield return null;
        }

        public bool SetAnchorOfImage(RectTransform rectTransform, float timeSec, float limitTimeSecMax, Vector2 anchorPosMin, Vector2 anchorPosMax)
        {
            try
            {
                var basePos = GetRate(timeSec, limitTimeSecMax);
                var movePos = new Vector2(rectTransform.anchoredPosition.x, (1f - basePos) * (anchorPosMax.y + anchorPosMin.y));
                rectTransform.anchoredPosition = movePos;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// 割合を取得
        /// </summary>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <returns>割合</returns>
        private float GetRate(float timeSec, float limitTimeSecMax)
        {
            return limitTimeSecMax == 0f ? DEFAULT : (timeSec / limitTimeSecMax);
        }

        public bool SetColorOfImage(float onmyoStateValue, Image image, Color32[] colors)
        {
            try
            {
                Color32 colorToSet;
                if (onmyoStateValue <= -1)
                {
                    colorToSet = colors[1];
                }
                else if (onmyoStateValue >= 1)
                {
                    colorToSet = colors[0];
                }
                else
                {
                    float blend = (onmyoStateValue + 1) / 2; // -1から1の値を0から1に変換
                    colorToSet = Color32.Lerp(colors[1], colors[0], blend);
                }
                image.color = colorToSet;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSpriteIndex(Image image, float timeSec, float limitTimeSecMax, Sprite[] sprites)
        {
            try
            {
                var baseIdx = GetRate(timeSec, limitTimeSecMax);
                var length = sprites.Length;
                var idx = Mathf.CeilToInt((1 - baseIdx) * (length - 1));
                if (!SetSprite(image, sprites[idx]))
                    throw new System.Exception("SetSprite");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSpriteOfImage(Image image, Sprite sprite)
        {
            return SetSprite(image, sprite);
        }

        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="image">イメージ</param>
        /// <param name="sprite">スプライト</param>
        /// <returns>成功／失敗</returns>
        private bool SetSprite(Image image, Sprite sprite)
        {
            try
            {
                image.sprite = sprite;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetSoulMoneyOfText(Text text, int soulMoney, string defaultFormat)
        {
            return SetContentOfText(text, $"{soulMoney}", defaultFormat);
        }

        public bool SetNameOfText(Text text, string name, string defaultFormat)
        {
            return SetContentOfText(text, name, defaultFormat);
        }

        /// <summary>
        /// 値をテキストへセット
        /// </summary>
        /// <param name="textComponent">テキスト系コンポーネント</param>
        /// <param name="value">値</param>
        /// <param name="defaultFormat">デフォルトフォーマット</param>
        /// <returns>成功／失敗</returns>
        private bool SetContentOfText<T>(T textComponent, string value, string defaultFormat)
        {
            try
            {
                if (textComponent is Text uiText)
                {
                    uiText.text = value;
                }
                else if (textComponent is TextMeshProUGUI tmpText)
                {
                    tmpText.text = value;
                }
                else
                {
                    throw new System.ArgumentException("Unsupported text component type.");
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                if (textComponent is Text uiText)
                {
                    uiText.text = defaultFormat;
                }
                else if (textComponent is TextMeshProUGUI tmpText)
                {
                    tmpText.text = defaultFormat;
                }

                return false;
            }
        }

        public bool SetNameOfText(TextMeshProUGUI text, string name, string defaultFormat)
        {
            return SetContentOfText(text, name, defaultFormat);
        }

        public bool SetSoulMoneyOfText(TextMeshProUGUI textMeshProUGUI, int soulMoney, string defaultFormat)
        {
            return SetContentOfText(textMeshProUGUI, $"{soulMoney}", defaultFormat);
        }

        public IEnumerator PlayBackSpinAnimation(System.IObserver<bool> observer, float duration, int backSpinCount, Transform transform)
        {
            transform.DORotate(new Vector3(0, 0, 360 * backSpinCount), duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => observer.OnNext(true));

            yield return null;
        }
    }

    /// <summary>
    /// Mainのユーティリティ
    /// ビュー
    /// インターフェース
    /// </summary>
    public interface IMainViewUtility
    {
        /// <summary>
        /// ImageのFillAmountをセットする
        /// </summary>
        /// <param name="image">イメージ</param>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <param name="maskAngle">マスクする角度の割合（0f~1f）</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool SetFillAmountOfImage(Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null);
        /// <summary>
        /// ImageのFillAmountのアニメーションを再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="image">イメージ</param>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <param name="maskAngle">マスクする角度の割合（0f~1f）</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFillAmountAndColorOfImage(System.IObserver<bool> observer, float[] durations, Color32 dangerousColor, Image image, float timeSec, float limitTimeSecMax, float maskAngle=0f, Transform transform=null);
        /// <summary>
        /// Imageのアンカーをセットする
        /// </summary>
        /// <param name="rectTransform">Rectトランスフォーム</param>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <param name="anchorPosMin">アンカー位置（最小）</param>
        /// <param name="anchorPosMax">アンカー位置（最大）</param>
        /// <returns>成功／失敗</returns>
        public bool SetAnchorOfImage(RectTransform rectTransform, float timeSec, float limitTimeSecMax, Vector2 anchorPosMin, Vector2 anchorPosMax);
        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="image">イメージ</param>
        /// <param name="timeSec">タイマー</param>
        /// <param name="limitTimeSecMax">制限時間（秒）</param>
        /// <param name="sprites">スプライト</param>
        /// <returns>成功／失敗</returns>
        public bool SetSpriteIndex(Image image, float timeSec, float limitTimeSecMax, Sprite[] sprites);
        /// <summary>
        /// スプライトをセット
        /// </summary>
        /// <param name="image">イメージ</param>
        /// <param name="sprite">スプライト</param>
        /// <returns>成功／失敗</returns>
        public bool SetSpriteOfImage(Image image, Sprite sprite);
        /// <summary>
        /// ImageのColor32をセットする
        /// </summary>
        /// <param name="onmyoStateValue">陰陽（昼夜）の状態</param>
        /// <param name="image">イメージ</param>
        /// <param name="colors">カラー</param>
        /// <returns>成功／失敗</returns>
        public bool SetColorOfImage(float onmyoStateValue, Image image, Color32[] colors);
        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="state">ステータス</param>
        /// <param name="duration">終了時間</param>
        /// <param name="component">コンポーネント</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayFadeAnimation<T>(System.IObserver<bool> observer, EnumFadeState state, float duration, T component) where T : Component;
        /// <summary>
        /// 逆回転させるDOTweenアニメーション再生
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <param name="duration">終了時間</param>
        /// <param name="backSpinCount">逆回転の回転数</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayBackSpinAnimation(System.IObserver<bool> observer, float duration, int backSpinCount, Transform transform);
        /// <summary>
        /// フェードステータスをセット
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <param name="component">コンポーネント</param>
        /// <returns>成功／失敗</returns>
        public bool SetFade<T>(EnumFadeState state, T component) where T : Component;
        /// <summary>
        /// スケーリングするDOTweenアニメーション再生
        /// </summary>
        /// <param name="durations">終了時間</param>
        /// <param name="scales">スケールのパターン</param>
        /// <param name="transform">トランスフォーム</param>
        /// <returns>成功／失敗</returns>
        public bool PlayScalingLoopAnimation(float[] durations, float[] scales, Transform transform);
        /// <summary>
        /// 魂のお金をセット
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="soulMoney">魂のお金</param>
        /// <param name="defaultFormat">テキストのデフォルトフォーマット</param>
        /// <returns>成功／失敗</returns>
        public bool SetSoulMoneyOfText(Text text, int soulMoney, string defaultFormat);
        /// <summary>
        /// 魂のお金をセット
        /// </summary>
        /// <param name="textMeshProUGUI">テキスト</param>
        /// <param name="soulMoney">魂のお金</param>
        /// <param name="defaultFormat">テキストのデフォルトフォーマット</param>
        /// <returns>成功／失敗</returns>
        public bool SetSoulMoneyOfText(TextMeshProUGUI textMeshProUGUI, int soulMoney, string defaultFormat);
        /// <summary>
        /// 名前をセット
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="name">名前</param>
        /// <param name="defaultFormat">テキストのデフォルトフォーマット</param>
        /// <returns>成功／失敗</returns>
        public bool SetNameOfText(Text text, string name, string defaultFormat);
        /// <summary>
        /// 名前をセット
        /// </summary>
        /// <param name="text">テキスト</param>
        /// <param name="name">名前</param>
        /// <param name="defaultFormat">テキストのデフォルトフォーマット</param>
        /// <returns>成功／失敗</returns>
        public bool SetNameOfText(TextMeshProUGUI text, string name, string defaultFormat);
    }
}
