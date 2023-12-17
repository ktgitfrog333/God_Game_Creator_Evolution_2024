using System.Collections;
using System.Collections.Generic;
using Main.Audio;
using UnityEngine;

public class ChangeSpeedDemo : MonoBehaviour
{
    [SerializeField] private AudioOwner audioOwner;
    [SerializeField,Range(-1f, 1f)] private float pbSpeed = 0f;
    [SerializeField] private bool invert;

    private void Reset()
    {
        audioOwner = GetComponent<AudioOwner>();
    }

    private void Update()
    {
        var details = new BgmConfDetails
        {
            InputValue = pbSpeed,
            Invert = invert
        };
        if (!audioOwner.ChangeSpeed(ClipToPlay.se_scratch_1, details))
            Debug.LogError("ChangeSpeed");
    }
}
