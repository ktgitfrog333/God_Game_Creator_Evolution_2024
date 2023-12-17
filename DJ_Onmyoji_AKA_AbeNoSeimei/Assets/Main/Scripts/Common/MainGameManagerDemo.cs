using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Main.Presenter;

namespace Main.Common
{
    public class MainGameManagerDemo : MonoBehaviour
    {
        [SerializeField] private MainPresenterDemo mainPresenterDemo;
        private void Start()
        {
            MainGameManager manager = GetComponent<MainGameManager>();
            var only = new BoolReactiveProperty();
            manager.UpdateAsObservable()
                .Subscribe(_ => 
                {
                    if (!only.Value)
                    {
                        only.Value = true;
                        if (mainPresenterDemo.isActiveAndEnabled)
                            mainPresenterDemo.OnStart();
                    }
                });
        }
    }
}
