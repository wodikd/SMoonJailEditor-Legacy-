using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SX3Game
{
    public enum EGameNodeType
    {
        None, Bullet, Laser, Bomb
    }

    //public interface Node
    //{
    //    public void UpdateNode();
    //}

    public abstract class GameNode : MonoBehaviour
    {
        public bool targetPlayer;
        public bool randomPosition;
        public abstract EGameNodeType GetNodeType { get; }
        public abstract void UpdatePosition();
        public abstract void UpdateNode();

        private float time;
        public float Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }
    }
}
