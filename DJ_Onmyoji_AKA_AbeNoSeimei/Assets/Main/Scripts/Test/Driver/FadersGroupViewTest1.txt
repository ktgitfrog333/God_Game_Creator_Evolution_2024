using Main.View;
using Select.View;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class FadersGroupViewTest1 : MonoBehaviour
{
    [SerializeField] private FadersGroupView fadersGroupView;

    private void Reset()
    {
        fadersGroupView = GameObject.FindObjectOfType<FadersGroupView>();
    }

    private void OnGUI()
    {
        // ボタンの配置やサイズを決定（x, y, width, height）
        if (GUI.Button(new Rect(10, 10, 100, 50), "Demo1"))
        {
            Observable.FromCoroutine<bool>(observer => fadersGroupView.PlayMoveAnchorsHeight(observer))
                .Subscribe(_ => { })
                .AddTo(gameObject);
        }

        if (GUI.Button(new Rect(10, 70, 100, 50), "Demo2"))
        {
            Observable.FromCoroutine<bool>(observer => fadersGroupView.PlayMoveAnchorsBased(observer))
                .Subscribe(_ => { })
                .AddTo(gameObject);
        }

        if (GUI.Button(new Rect(10, 130, 100, 50), "Empty"))
        {
        }
    }
}
