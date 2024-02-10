#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;
using System.Linq;

namespace Universal.Other
{
    /// <summary>
    /// Unityエディタのメニューバーに新しい項目を追加し、その項目をクリックすると、
    /// コンソールにImageコンポーネントを持つすべてのオブジェクトの名前と階層を出力します。
    /// </summary>
    public class FindImagesInScene : EditorWindow
    {
        private const string NO_SPRITE = "No Sprite";
        private const string NO_COMPONENT = "No Component";

        [MenuItem("Tools/Find All Images in Scene")]
        private static void FindAllImages()
        {
            // StringBuilderを使用して、出力する文字列を構築します。
            StringBuilder stringBuilder = new StringBuilder();

            // シーン内のすべてのImageコンポーネントを検索します。
            Image[] images = FindObjectsOfType<Image>();
            SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();

            // Imageコンポーネントの情報を出力します。
            OutputPaths(images.Cast<Component>().ToArray(), stringBuilder);
            // SpriteRendererコンポーネントの情報を出力します。
            OutputPaths(spriteRenderers.Cast<Component>().ToArray(), stringBuilder);

            // コンソールに結果を出力します。
            Debug.Log(stringBuilder.ToString());
        }

        /// <summary>
        /// パスの出力
        /// </summary>
        /// <param name="components">コンポーネント配列</param>
        /// <param name="stringBuilder">メッセージ文字列</param>
        private static void OutputPaths(Component[] components, StringBuilder stringBuilder)
        {
            foreach (var component in components)
            {
                // オブジェクトのフルパスを取得します。
                string path = GetGameObjectPath(component.gameObject);
                string[] names =
                {
                    NO_SPRITE,
                    NO_COMPONENT,
                };
                // コンポーネントの型に応じてテクスチャ名を取得します。
                if (component is Image image)
                {
                    names[0] = image.sprite != null ? image.sprite.texture.name : NO_SPRITE;
                    names[1] = image.GetType().Name;
                }
                else if (component is SpriteRenderer spriteRenderer)
                {
                    names[0] = spriteRenderer.sprite != null ? spriteRenderer.sprite.texture.name : NO_SPRITE;
                    names[1] = spriteRenderer.GetType().Name;
                }
                // StringBuilderにオブジェクトの情報とテクスチャ名を追加します。
                stringBuilder.AppendLine($"{path}\t{string.Join("\t", names)}");
            }
        }

        /// <summary>
        /// オブジェクトのフルパスを取得するメソッド
        /// </summary>
        /// <param name="obj">ゲームオブジェクト</param>
        /// <returns>シーン（ヒエラルキー）内のオブジェクトパス</returns>
        private static string GetGameObjectPath(GameObject obj)
        {
            string path = obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = obj.name + "/" + path;
            }
            return path;
        }
    }    
}
#endif
