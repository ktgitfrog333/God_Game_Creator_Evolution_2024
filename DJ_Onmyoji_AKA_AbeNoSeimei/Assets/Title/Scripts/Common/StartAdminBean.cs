using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Title.Common
{
    public class StartAdminBean : MonoBehaviour
    {
        [SerializeField] private string openURLPath = "https://ktgitfrog333.github.io/adminpage/dist/";
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "TitleScene";

        private void Start()
        {
            Application.OpenURL(openURLPath);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
