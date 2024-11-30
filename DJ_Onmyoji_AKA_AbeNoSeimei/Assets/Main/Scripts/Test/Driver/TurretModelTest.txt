using Main.Common;
using Main.Model;
using Main.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Test.Driver
{
    public class TurretModelTest : MonoBehaviour
    {
        [SerializeField] private WrapTurretModel wrapTurretModel;
        [SerializeField] private DanceTurretModel danceTurretModel;
        [SerializeField] private GraffitiTurretModel graffitiTurretModel;
        [SerializeField] private OnmyoTurretModel[] onmyoTurretModels;

        private void Update()
        {
            if (wrapTurretModel == null)
                wrapTurretModel = GameObject.FindObjectOfType<WrapTurretModel>();
            if (danceTurretModel == null)
                danceTurretModel = GameObject.FindObjectOfType<DanceTurretModel>();
            if (graffitiTurretModel == null)
                graffitiTurretModel = GameObject.FindObjectOfType<GraffitiTurretModel>();
            if (onmyoTurretModels == null ||
                onmyoTurretModels.Length < 1)
                onmyoTurretModels = GameObject.FindObjectsOfType<OnmyoTurretModel>();
        }

        private void OnGUI()
        {
            // ボタンの配置やサイズを決定（x, y, width, height）
            if (GUI.Button(new Rect(10, 10, 100, 50), "DemoTrue"))
            {
                wrapTurretModel.SetAutoInstanceMode(true);
                danceTurretModel.SetAutoInstanceMode(true);
                graffitiTurretModel.SetAutoInstanceMode(true);
                foreach (var model in onmyoTurretModels)
                    model.SetAutoInstanceMode(true);
            }

            if (GUI.Button(new Rect(10, 70, 100, 50), "DemoPause"))
            {
                wrapTurretModel.SetAutoInstanceMode(false);
                danceTurretModel.SetAutoInstanceMode(false);
                graffitiTurretModel.SetAutoInstanceMode(false);
                foreach (var model in onmyoTurretModels)
                    model.SetAutoInstanceMode(false);
            }

            if (GUI.Button(new Rect(10, 130, 100, 50), "Empty"))
            {
            }
        }
    }
}
