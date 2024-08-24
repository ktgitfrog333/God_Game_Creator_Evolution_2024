using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;

namespace Main.Test.Driver
{
    public class PlayerModelTest : MonoBehaviour
    {
        private void OnEnable()
        {
            GameObject.Find("Player").GetComponent<PlayerModel>().SetIsDead(true);
        }
    }
}
