using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Universal.Common;
using System.Linq;
using Universal.Bean;

namespace Universal.Accessory
{
    /// <summary>
    /// リソースアクセス
    /// タイトル用
    /// </summary>
    public class ResourcesAccessory
    {
        /// <summary>
        /// JSONファイルの拡張子
        /// </summary>
        private readonly string EXTENSION_JSON = ".json";
        /// <summary>
        /// エンコーディング
        /// </summary>
        private readonly string ENCODING = "UTF-8";

        /// <summary>
        /// 初期処理
        /// </summary>
        public void Initialize()
        {
            // リソース管理ディレクトリが存在しない場合は作成
            if (!Directory.Exists(GetHomePath()))
            {
                Directory.CreateDirectory(GetHomePath());
            }
            if (!File.Exists($"{GetHomePath()}{ConstResorcesNames.USER_DATA}{EXTENSION_JSON}"))
            {
                using (File.Create($"{GetHomePath()}{ConstResorcesNames.USER_DATA}{EXTENSION_JSON}")) { }
                if (!SaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA, new UserBean(EnumLoadMode.Default)))
                    Debug.LogError("ユーザデータをJSONファイルへ保存の失敗");
            }
            if (!File.Exists($"{GetHomePath()}{ConstResorcesNames.ADMIN_DATA}{EXTENSION_JSON}"))
            {
                using (File.Create($"{GetHomePath()}{ConstResorcesNames.ADMIN_DATA}{EXTENSION_JSON}")) { }
                if (!SaveDatasJsonOfAdminBean(ConstResorcesNames.ADMIN_DATA, new AdminBean()))
                    Debug.LogError("管理者データをJSONファイルへ保存の失敗");
            }
        }

        /// <summary>
        /// ホームディレクトリを取得
        /// </summary>
        /// <returns>ホームディレクトリ</returns>
        private string GetHomePath()
        {
            var path = "";
#if UNITY_EDITOR
            path = ConstResorcesNames.HOMEPATH_UNITYEDITOR;
#elif UNITY_STANDALONE
                path = ConstResorcesNames.HOMEPATH_BUILD;
#endif
            return path;
        }

        /// <summary>
        /// JSONデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースJSONファイル名</param>
        /// <param name="enumLoadMode">ロードモード</param>
        /// <returns>ユーザー情報</returns>
        public UserBean LoadSaveDatasJsonOfUserBean(string resourcesLoadName, EnumLoadMode enumLoadMode=EnumLoadMode.Continue)
        {
            try
            {
                var path = GetHomePath();
                switch (enumLoadMode)
                {
                    case EnumLoadMode.Continue:
                        // 設定内容を保存
                        using (var sr = new StreamReader($"{path}{resourcesLoadName}{EXTENSION_JSON}", Encoding.GetEncoding(ENCODING)))
                            return new UserBean(JsonUtility.FromJson<UserBean>(sr.ReadToEnd()));
                    case EnumLoadMode.Default:
                        return new UserBean(enumLoadMode);
                    case EnumLoadMode.All:
                        return new UserBean(enumLoadMode);
                    default:
                        throw new System.Exception("例外エラー");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public AdminBean LoadSaveDatasJsonOfAdminBean(string resourcesLoadName)
        {
            try
            {
                var path = GetHomePath();
                using (var sr = new StreamReader($"{path}{resourcesLoadName}{EXTENSION_JSON}", Encoding.GetEncoding(ENCODING)))
                    return new AdminBean(JsonUtility.FromJson<AdminBean>(sr.ReadToEnd()));
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// ユーザデータをJSONファイルへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="UserBean">ユーザー情報を保持するクラス</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasJsonOfUserBean(string resourcesLoadName, UserBean userBean)
        {
            try
            {
                var path = GetHomePath();
                // 設定内容を保存
                using (var sw = new StreamWriter($"{path}{resourcesLoadName}{EXTENSION_JSON}", false, Encoding.GetEncoding(ENCODING)))
                {
                    var json = JsonUtility.ToJson(userBean);
                    sw.WriteLine(json);
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public bool SaveDatasJsonOfAdminBean(string resourcesLoadName, AdminBean adminBean)
        {
            try
            {
                var path = GetHomePath();
                // 設定内容を保存
                using (var sw = new StreamWriter($"{path}{resourcesLoadName}{EXTENSION_JSON}", false, Encoding.GetEncoding(ENCODING)))
                {
                    var json = JsonUtility.ToJson(adminBean);
                    sw.WriteLine(json);
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// シーンの状態を更新
        /// </summary>
        /// <param name="continues">続行するユーザーデータ</param>
        /// <param name="defaults">デフォルトのユーザーデータ</param>
        /// <returns>更新されたユーザーデータ</returns>
        public UserBean UpdateSceneStates(UserBean continues, UserBean defaults)
        {
            try
            {
                continues.sceneId = defaults.sceneId;
                continues.state = defaults.state;

                return continues;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// オーディオと振動の設定を更新
        /// </summary>
        /// <param name="continues">続行するユーザーデータ</param>
        /// <param name="defaults">デフォルトのユーザーデータ</param>
        /// <returns>更新されたユーザーデータ</returns>
        public UserBean UpdateAudioAndVibration(UserBean continues, UserBean defaults)
        {
            try
            {
                continues.audioVolumeIndex = defaults.audioVolumeIndex;
                continues.bgmVolumeIndex = defaults.bgmVolumeIndex;
                continues.seVolumeIndex = defaults.seVolumeIndex;
                continues.vibrationEnableIndex = defaults.vibrationEnableIndex;

                return continues;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }
}
