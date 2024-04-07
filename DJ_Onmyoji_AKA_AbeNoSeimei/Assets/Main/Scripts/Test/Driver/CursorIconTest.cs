using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class CursorIconTest : MonoBehaviour
    {
        // [SerializeField] private Vector3 position;
        public IReactiveProperty<Vector3> Position { get; private set; } = new Vector3ReactiveProperty();
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Vector2 sizeDelta;
        public Vector3 SizeDelta => sizeDelta;

        private void Update()
        {
            Debug.Log($"objName:[{rectTransform.name}]\npos:[{rectTransform.position}]\nscale:[{rectTransform.sizeDelta}]");
            Position.Value = rectTransform.position;
            sizeDelta = rectTransform.sizeDelta;
        }
    }
}
