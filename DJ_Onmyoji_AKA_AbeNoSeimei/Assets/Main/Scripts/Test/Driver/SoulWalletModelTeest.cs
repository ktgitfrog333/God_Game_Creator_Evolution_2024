using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Main.Model;
using UniRx;
using UnityEngine;
using Universal.Utility;

namespace Main.Test.Driver
{
    public class SoulWalletModelTeest : MonoBehaviour
    {
        public IReactiveProperty<bool> IsUnLockUpdateOfSoulMoney { get; private set; } = new BoolReactiveProperty();
        private void Start()
        {
            // Debug.Log("start");
            DOVirtual.DelayedCall(1f, () => IsUnLockUpdateOfSoulMoney.Value = true)
                .SetUpdate(true);
            // StartCoroutine(ActionsAfterDelay(1f));
        }

        private IEnumerator ActionsAfterDelay(float delay)
        {
            Debug.Log("a");
            yield return new WaitForSeconds(delay);
            // IsUnLockUpdateOfSoulMoney.Value = true;
            Debug.Log("UnLockUpdateOfSoulMoney Done");
        }
        // private void OnEnable()
        // {
        //     var result = GetComponent<SoulWalletModel>().AddSoulMoney(1);
        //     Debug.Log(result);
        // }
    }
}
