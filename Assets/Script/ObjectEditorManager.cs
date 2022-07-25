using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SX3Game
{
    namespace Editor
    {
        public enum ListAddMode
        {
            beginning = 0,
            addition = 1
        }

        public interface INodeEditor
        {
            void UpdateNodeInfo();
        }

        [System.Serializable]
        public class ObjectEditorManager : MonoBehaviour
        {
            private static List<GameNode> nodeList = new List<GameNode>();
            private static INodeEditor bulletEditor;
            private static INodeEditor laserEditor;

            private void Awake()
            {
                bulletEditor = GameManager.FindObjectOfType<BulletEditor>();
            }

            public static void AddNodeToList(GameNode gameNode, ListAddMode addMode)
            {
                if (nodeList.Count == 0)
                {
                    nodeList.Add(gameNode);

                    UpdateNodeInfo(gameNode.GetNodeType);
                }
                else if (nodeList[0].GetNodeType == gameNode.GetNodeType)
                {
                    switch (addMode)
                    {
                        case ListAddMode.beginning:
                            nodeList.Clear();
                            nodeList.Add(gameNode);
                            break;
                        case ListAddMode.addition:
                            nodeList.Add(gameNode);
                            break;
                        default:
                            break;
                    }

                    UpdateNodeInfo(gameNode.GetNodeType);
                }
                else
                {
                    Debug.Log($"type of list[0] is {nodeList[0].GetNodeType} but you tried to add {gameNode.GetNodeType}");
                }

                
            }

            public static void UpdateNodeInfo()
            {
                switch (nodeList[0].GetNodeType)
                {
                    case EGameNodeType.None:
                        break;
                    case EGameNodeType.Bullet:
                        bulletEditor.UpdateNodeInfo();
                        break;
                    case EGameNodeType.Laser:
                        break;
                    case EGameNodeType.Bomb:
                        break;
                    default:
                        break;
                }
            }

            public static void UpdateNodeInfo(EGameNodeType nodeType)
            {
                switch (nodeType)
                {
                    case EGameNodeType.None:
                        break;
                    case EGameNodeType.Bullet:
                        bulletEditor.UpdateNodeInfo();
                        break;
                    case EGameNodeType.Laser:
                        break;
                    case EGameNodeType.Bomb:
                        break;
                    default:
                        break;
                }
            }

            public static List<GameNode> NodeList
            {
                get
                {
                    return nodeList;
                }
            }
        }
    }
}