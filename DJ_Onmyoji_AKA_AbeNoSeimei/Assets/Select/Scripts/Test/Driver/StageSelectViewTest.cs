using System.Collections;
using System.Collections.Generic;
using Select.Common;
using Select.View;
using UnityEngine;

namespace Select.Test.Driver
{
    public class StageSelectViewTest : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] private EnumEventCommand enumEventCommand;

        private void OnEnable()
        {
            StageSelectView stageSelectView = GameObject.Find("StageSelect").GetComponent<StageSelectView>();
            //stageSelectView.RenderLineStageContetsBetweenTargetPoints(index, enumEventCommand);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            // StageSelectView stageSelectView = GameObject.Find("StageSelect").GetComponent<StageSelectView>();
            // ((IStageSelectViewTest)stageSelectView).RenderLineFromPointBetweenToPoint(anchoredPositionFrom.position, anchoredPositionTo.position);
        }
    }

    public interface IStageSelectViewTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anchoredPositionFrom"></param>
        /// <param name="anchoredPositionTo"></param>
        /// <returns></returns>
        /// <see cref="Select.View.StageSelectView.RenderLineFromPointBetweenToPoint(Vector2, Vector2)"/>
        public bool RenderLineFromPointBetweenToPoint(Vector2 anchoredPositionFrom, Vector2 anchoredPositionTo);
    }
}
