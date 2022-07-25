using UnityEngine;

namespace SX3Game
{
    public class Bullet : GameNode
    {
        private float speed;
        private float angle;
        private Vector2 startPos;

        private new Rigidbody2D rigidbody;

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }
        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }
        public Vector2 StartPos
        {
            get
            {
                return startPos;
            }
            set
            {
                startPos = value;
            }
        }

        public Bullet(float time, Vector2 startPos, float angle, float speed)
        {
            Set(time, startPos, angle, speed);
        }

        public void Awake()
        {
            //((IGameNode)this).UpdateNode();
            GameManager.GameNodeList.Add(this);

            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Start()
        {
            //Set(transform.position, Tool.DirToAngle((Vector2.zero - (Vector2)transform.position).normalized), 0.1f, GameManager.GameTime);
        }

        public void Set(float time, Vector2 startPos, float angle, float speed)
        {
            this.Time = time;
            this.startPos = startPos;
            this.angle = angle;
            this.speed = speed;

            UpdateNode();
        }

        public override void UpdateNode()
        {
            UpdatePosition();
            transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
        }
        public override void UpdatePosition()
        {
            rigidbody.MovePosition(startPos + ((GameManager.GameTime - Time) * -speed * GameManager.mapInfo.BPM) * (Vector2)transform.right);
        }

        //void IGameNode.UpdateNode()
        //{
        //    UpdateNode();
        //}

        //void IGameNode.UpdatePosition()
        //{
        //    UpdatePosition();
        //}

        

        //public void Set(Vector2 startPos)
        //{
        //    this.startPos = startPos;
        //    UpdateNode();
        //}

        //public void Set(Vector2 startPos, float angle)
        //{
        //    this.startPos = startPos;
        //    this.angle = angle;

        //    UpdateNode();
        //}

        //public void Set(Vector2 startPos, float angle, float speed)
        //{
        //    this.startPos = startPos;
        //    this.angle = angle;
        //    this.speed = speed;

        //    UpdateNode();
        //}

        //public NodeType GetNodeType // It works
        //{
        //    get
        //    {
        //        return NodeType.Bullet;
        //    }
        //}

        public override EGameNodeType GetNodeType
        {
            get
            {
                return EGameNodeType.Bullet;
            }
        }
    }
}
