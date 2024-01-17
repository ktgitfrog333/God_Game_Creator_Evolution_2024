using System.Collections;
using System.Collections.Generic;
using Main.Model;
using UnityEngine;

namespace Main.Test.Driver
{
    public class SoulWalletModelTeest : MonoBehaviour
    {
        private void OnEnable()
        {
            var result = GetComponent<SoulWalletModel>().AddSoulMoney(1);
            Debug.Log(result);
        }
    }
}
