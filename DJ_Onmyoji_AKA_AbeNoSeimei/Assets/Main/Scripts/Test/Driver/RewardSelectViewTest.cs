using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Main.Common;
using Main.View;
using UniRx;
using UnityEngine;

namespace Main.Test.Driver
{
    public class RewardSelectViewTest : MonoBehaviour
    {
        /// <summary>魂のお金</summary>
        public IReactiveProperty<int> SoulMoney { get; private set; } = new IntReactiveProperty();
        [SerializeField] private int soulMoney;
        [SerializeField] private RewardContentProp[] rewardContentProps;
        public RewardContentProp[] RewardContentProps => rewardContentProps;
        [SerializeField] private ClearRewardType clearRewardType;
        public ClearRewardType ClearRewardType => clearRewardType;
        [SerializeField] private int isScaleUpIndex;
        public IReactiveProperty<int> IsScaleUpIndex { get; private set; } = new IntReactiveProperty();
        [SerializeField] private int isScaleDownIndex;
        public IReactiveProperty<int> IsScaleDownIndex { get; private set; } = new IntReactiveProperty();
        [SerializeField] private bool upOrDown;
        [SerializeField] private bool isCheck;
        [SerializeField] private int checkIndex;
        public IReactiveProperty<int> CheckIndex { get; private set;} = new IntReactiveProperty();
        [SerializeField] private int unCheckIndex;
        public IReactiveProperty<int> UnCheckIndex { get; private set;} = new IntReactiveProperty();

        // Start is called before the first frame update
        void Start()
        {
            transform.DOScale(Vector3.one * 100, 10f);
        }

        // Update is called once per frame
        void Update()
        {
            SoulMoney.Value = soulMoney;
            if (upOrDown)
                IsScaleUpIndex.Value = isScaleUpIndex;
            else
                IsScaleDownIndex.Value = isScaleDownIndex;
            if (isCheck)
                CheckIndex.Value = checkIndex;
            else
                UnCheckIndex.Value = unCheckIndex;
        }
    }
}
