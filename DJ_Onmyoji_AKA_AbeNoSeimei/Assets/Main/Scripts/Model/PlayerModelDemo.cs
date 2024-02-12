using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    public class PlayerModelDemo : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerModel;
        private void Reset()
        {
            playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();
        }
        public void Case_0()
        {
            playerModel.SetIsDead(true);
        }
    }
}
