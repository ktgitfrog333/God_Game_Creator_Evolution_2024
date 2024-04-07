using System.Collections;
using System.Collections.Generic;
using Main.View;
using UnityEngine;

namespace Main.Test.Driver
{
    public class AnimatorViewTest : MonoBehaviour
    {
        [SerializeField] private AnimatorView animatorView;

        private void Reset()
        {
            animatorView = GameObject.Find("BodySprites").GetComponent<AnimatorView>();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(20,40,80,20), $"{ParametersOfAnim.DamageRight}"))
                if (!animatorView.SetTrigger(ParametersOfAnim.DamageRight))
                    Debug.LogError("SetTrigger");
            if (GUI.Button(new Rect(20,70,80,20), $"{ParametersOfAnim.DamageLoopRight}"))
                if (!animatorView.SetBool(ParametersOfAnim.DamageLoopRight, true))
                    Debug.LogError("SetBool");
            if (GUI.Button(new Rect(20,100,80,20), $"{ParametersOfAnim.DamageLeft}"))
                if (!animatorView.SetTrigger(ParametersOfAnim.DamageLeft))
                    Debug.LogError("SetTrigger");
            if (GUI.Button(new Rect(20,130,80,20), $"{ParametersOfAnim.DamageLoopLeft}"))
                if (!animatorView.SetBool(ParametersOfAnim.DamageLoopLeft, true))
                    Debug.LogError("SetBool");
            if (GUI.Button(new Rect(20,160,80,20), $"AllReset"))
            {
                if (!animatorView.SetBool(ParametersOfAnim.DamageLoopRight, false))
                    Debug.LogError("SetBool");
                if (!animatorView.SetBool(ParametersOfAnim.DamageLoopLeft, false))
                    Debug.LogError("SetBool");
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
