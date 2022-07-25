using UnityEngine;

namespace SX3Game
{
    class Laser : GameNode
    {
        public override void UpdatePosition()
        {

        }

        public override void UpdateNode()
        {

        }

        public override EGameNodeType GetNodeType
        {
            get
            {
                return EGameNodeType.Laser;
            }
        }
    }
}