using Fungus;
using Main.Common;
using Main.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    /// <summary>
    /// FungusのSayDialogを管理
    /// ビュー
    /// </summary>
    public class GuideMessageView : MonoBehaviour, IGuideMessageView, IButtonEventTriggerModel
    {
        /// <summary>FungusのSayDialog</summary>
        [SerializeField] private SayDialog sayDialog;
        /// <summary>FungusのSayDialogを管理する構造体</summary>
        [SerializeField]
        private GuideMessageStruct[] guideMessageStructs = new GuideMessageStruct[]
        {
            new GuideMessageStruct()
            {
                missionID = MissionID.MI0001,
                sentenceTemplate = "ターンテーブルを回して、全ての敵を倒そう！！！\n" +
                "【 $killedEnemyCount ／ $killedEnemyCountMax 】"
            },
            new GuideMessageStruct()
            {
                missionID = MissionID.MI0002,
                sentenceTemplate = "イコライザーを左右に回して、式神の強さを調整して、\n" +
                "各色のオーラを纏った敵を全て倒そう！！！\n" +
                "【 $killedEnemyCount ／ $killedEnemyCountMax 】"
            },
        };

        private void Reset()
        {
            sayDialog = GetComponent<SayDialog>();
        }

        public bool SetButtonEnabled(bool enabled)
        {
            try
            {
                sayDialog.SetInteractableOfContinueButton(enabled);

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SetEventTriggerEnabled(bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateSentence(MissionID missionID, int killedEnemyCount, int killedEnemyCountMax)
        {
            try
            {
                var guideMessageStruct = guideMessageStructs.FirstOrDefault(g => g.missionID.Equals(missionID));
                string sentence = guideMessageStruct.sentenceTemplate;
                // 置換処理
                sentence = sentence.Replace("$killedEnemyCount", killedEnemyCount.ToString("D").ToFullWidth());
                sentence = sentence.Replace("$killedEnemyCountMax", killedEnemyCountMax.ToString("D").ToFullWidth());


                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    /// <summary>
    /// FungusのSayDialogを管理
    /// 構造体
    /// </summary>
    public struct GuideMessageStruct
    {
        /// <summary>ミッションID</summary>
        public MissionID missionID;
        /// <summary>ミッション中の進捗状況を記述する文章テンプレート</summary>
        public string sentenceTemplate;
    }

    /// <summary>
    /// FungusのSayDialogを管理
    /// インターフェース
    /// </summary>
    public interface IGuideMessageView
    {
        /// <summary>
        /// ミッション用に表示文言を変更する
        /// </summary>
        /// <param name="missionID">ミッションID</param>
        /// <param name="killedEnemyCount">敵の撃破数（子）の管理情報</param>
        /// <param name="killedEnemyCountMax">敵の撃破数（親）の管理情報</param>
        /// <returns>成功／失敗</returns>
        public bool UpdateSentence(MissionID missionID, int killedEnemyCount, int killedEnemyCountMax);
    }

    // 全角変換を行う拡張メソッド
    public static class StringExtensions
    {
        public static string ToFullWidth(this string input)
        {
            return string.Concat(input.Select(c => c <= '\u007E' && c >= '\u0021' ? (char)(c + '\uFEE0') : c));
        }
    }
}
