using UnityEngine;

namespace SX3Game
{
    class Bomb : GameNode
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
                return EGameNodeType.Bomb;
            }
        }
    }
}