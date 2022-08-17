using UnityEngine;
using System;
using System.Collections;

namespace SMoonJail
{
    class Laser : GameNode
    {
        public int delayBeat;
        public int durationBeat;

        public float beatGap;

        private float endTime;
        private float actionTime;
        private float durationTime;

        private bool isValueUpdate = false;

        private float EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                isValueUpdate = false;

                endTime = value;
            }
        }
        private float ActionTime
        {
            get
            {
                return actionTime;
            }
            set
            {
                isValueUpdate = false;

                actionTime = value;
            }
        }

        [HideInInspector]
        public Area area1;
        [HideInInspector]
        public Area area2;

        private void Awake()
        {
            area1 = transform.Find("Area1").GetComponent<Area>();
            area2 = transform.Find("Area2").GetComponent<Area>();
        }

        protected override void Start()
        {
            base.Start();

            beatGap = Editor.MusicTool.GetBeatGap;
        }

        public override void UpdatePosition()
        {
            var gameTime = GameManager.GameTime;

            if (!IsAction)
            {
                area2.T = 0;

                return;
            }
            var runTime = gameTime - Time;

            var gauge = runTime / ActionTime;

            if (gauge < 1)
            {
                area2.T = gauge;
            }
            else
            {
                var temp = 1 - ((runTime / durationTime) - 1);
                area1.T = temp;
                area2.T = temp;
            }

            //gauge = gauge > 1 ? 1 : gauge; 

            //area2.T = gauge;

            #region Debug
            //GameTool.Debugger.Log(gauge);

            //System.Text.StringBuilder stringBuilder = new();
            //stringBuilder
            //    .AppendLine("runTime: " + runTime)
            //    .AppendLine("gameTime: " + gameTime)
            //    .AppendLine("delayTime: " + delayTime)
            //    .AppendLine("endTime: " + (Time + delayTime))
            //    ;

            //GameTool.Debugger.Log(stringBuilder);
            #endregion
        }

        public override void UpdateValue()
        {
            EndTime = ((delayBeat + durationBeat) * Editor.MusicTool.GetBeatGap);
            ActionTime = delayBeat * Editor.MusicTool.GetBeatGap;
            durationTime = EndTime - ActionTime;

            area1.T = 1;
            area2.T = 0;

            isValueUpdate = true;
        }

        public bool IsAction
        {
            get
            {
                if (!isValueUpdate)
                {
                    UpdateValue();
                }

                var gameTime = GameManager.GameTime;
                

                if (gameTime < Time || gameTime > EndTime + Time)
                {
                    return false;
                }

                return true;
            }
        }

        public override GameNodeType GetNodeType
        {
            get
            {
                return GameNodeType.Laser;
            }
        }
    }
}