using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SX3Game;
using SX3Game.Editor;
using System.Threading;
using System.Threading.Tasks;

public class SaveLoadManager : MonoBehaviour
{
    public List<GameNode> gameNodeList;

    private char[] nodeSeparator = new char[] { ':' };
    private char dataSeparator = '/';

    private readonly WaitForEndOfFrame wait = new();

    public SaveLoadManager(List<GameNode> gameNodeList)
    {
        this.gameNodeList = gameNodeList;
    }

    public IEnumerator Save(string path)
    {
        Sort(path: path);

        StreamWriter streamWriter = new(path);
        System.Text.StringBuilder stringBuilder = new();

        List<Bullet> bulletList = new();
        List<Laser> laserList = new();
        List<Bomb> bombList = new();

        for (int i = 0; i < gameNodeList.Count; i++)
        {
            switch (gameNodeList[i].GetNodeType)
            {
                case EGameNodeType.None:
                    break;
                case EGameNodeType.Bullet:
                    SaveBullet(gameNodeList[i].GetComponent<Bullet>());
                    break;
                case EGameNodeType.Laser:
                    SaveLaser(gameNodeList[i].GetComponent<Laser>());
                    break;
                case EGameNodeType.Bomb:
                    SaveBomb(gameNodeList[i].GetComponent<Bomb>());
                    break;
                default:
                    break;
            }

            yield return wait;
        }

        streamWriter.Close();

        #region �Լ���
        void SaveBullet(Bullet bullet)
        {
            // 'type,time,startpos,angle,speed'
            stringBuilder
                .Append($"{bullet.GetNodeType}" + dataSeparator)
                .Append($"{bullet.Time}" + dataSeparator)
                .Append($"{bullet.StartPos.x}" + dataSeparator + $"{bullet.StartPos.y}" + dataSeparator)
                .Append($"{bullet.Angle}" + dataSeparator)
                .Append($"{bullet.Speed}")
                .Append(nodeSeparator)
                ;

            //streamWriter.WriteLine(stringBuilder);
            Write(stringBuilder);

            stringBuilder.Clear();
        }
        void SaveLaser(Laser laser)
        {
            stringBuilder.Append(nodeSeparator)
                .Append($"{laser.GetNodeType},")
                .Append($"{laser.Time},");

            Write(stringBuilder);

            stringBuilder.Clear();
        }
        void SaveBomb(Bomb bomb)
        {
            stringBuilder.Append(nodeSeparator)
                .Append($"{bomb.GetNodeType},")
                .Append($"{bomb.Time},");

            Write(stringBuilder);

            stringBuilder.Clear();
        }
        void Write(System.Text.StringBuilder nodeInfo)
        {
            streamWriter.Write(nodeInfo);
        }
        #endregion
    }

    public IEnumerator Load(string path)
    {
        System.Diagnostics.Stopwatch totalTimeStopwatch = new();
        System.Diagnostics.Stopwatch nodeSplitStopWatch = new();
        System.Diagnostics.Stopwatch convertNodeBuilderStopwatch = new();
        System.Diagnostics.Stopwatch nodeBuilderStopwatch = new();

        StreamReader streamReader = new(path);

        var nodeData = streamReader.ReadToEnd();
        
        var nodeList = new List<string>(nodeData.Split(separator: nodeSeparator));

        nodeList.RemoveAt(nodeList.Count - 1);

        totalTimeStopwatch.Start();

        for (int i = 0; i < nodeList.Count; i++)
        {
            nodeSplitStopWatch.Start();
            var node = nodeList[i].Split(separator: dataSeparator);
            nodeSplitStopWatch.Stop();

            switch (ConvertToNodeType(node[0]))
            {
                case EGameNodeType.None:
                    break;
                case EGameNodeType.Bullet:
                    convertNodeBuilderStopwatch.Start();
                    NodeBuilder builder = ConvertNodeBuilder(node);
                    convertNodeBuilderStopwatch.Stop();

                    nodeBuilderStopwatch.Start();
                    builder.BuildBullet();
                    nodeBuilderStopwatch.Stop();

                    break;
                case EGameNodeType.Laser:
                    break;
                case EGameNodeType.Bomb:
                    break;
                default:
                    break;
            }

            //Debug.Log($"{(((float)i + 1) / nodeList.Count) * 100,0:0.0}%");


        }
        totalTimeStopwatch.Stop();

        yield return wait;

        System.Text.StringBuilder stringBuilder = new();

        var totalTime = totalTimeStopwatch.ElapsedTicks;
        var nodeSplitTime = nodeSplitStopWatch.ElapsedTicks;
        var convertTime = convertNodeBuilderStopwatch.ElapsedTicks;
        var nodeBuilderTime = nodeBuilderStopwatch.ElapsedTicks;
        var convertAndBuiler = convertTime + nodeBuilderTime;

        stringBuilder.AppendLine($"totalTime:\n{totalTime}")
            .AppendLine($"ConvertNodeTime:\n{convertTime}({(float)convertTime / totalTime})")
            .AppendLine($"NodeSplitTime:\n{nodeSplitTime}({(float)nodeSplitTime / totalTime})")
            .AppendLine($"NodeBuilderTime:\n{nodeBuilderTime}({(float)nodeBuilderTime / totalTime})")
            .AppendLine($"Convert+Builder:\n{convertAndBuiler}({(float)convertAndBuiler / totalTime})")
            ;

        Debug.Log(stringBuilder);
    }
    // 'type,time,startpos,angle,speed'

    public void MakeBullet(float time, Vector2 startPos, float angle, float speed)
    {
        var bullet = Instantiate(GameManager.BulletPrefab).GetComponent<Bullet>();

        bullet.Set(
            time: time,
            startPos: startPos,
            angle: angle,
            speed: speed
            );
    }

    private NodeBuilder ConvertNodeBuilder(string[] nodeInfo)
    {
        NodeBuilder builder = new();

        switch (ConvertToNodeType(nodeInfo[0]))
        {
            case EGameNodeType.Bullet:

                var time = float.Parse(nodeInfo[1]);
                var startPos = new Vector2(
                    x: float.Parse(nodeInfo[2]), 
                    y: float.Parse(nodeInfo[3])
                    );
                var angle = float.Parse(nodeInfo[4]);
                var speed = float.Parse(nodeInfo[5]);

                builder.SetTime(time)
                    .SetStartPos(startPos)
                    .SetAngle(angle)
                    .SetSpeed(speed);

                break;
            case EGameNodeType.Laser:
                break;
            case EGameNodeType.Bomb:
                break;
            default:
                break;
        }

        return builder;
    }

    public EGameNodeType ConvertToNodeType(string nodeType) =>
        nodeType switch
        {
            "Bullet" => EGameNodeType.Bullet,
            "Laser" => EGameNodeType.Laser,
            "Bomb" => EGameNodeType.Bomb,
            _ => throw new System.Exception($"\"{nodeType}\" is not nodeType"),
        };

    public EGameNodeType ConvertToNodeType(GameNode nodeType) =>
        nodeType switch
        {
            Bullet => EGameNodeType.Bullet,
            Laser => EGameNodeType.Laser,
            Bomb => EGameNodeType.Bomb,
            _ => throw new System.Exception("is not nodeType"),
        };

    public void Sort(string path)
    {
        gameNodeList.Sort((a, b) =>
        {
            return a.Time > b.Time ? 1
            : a.Time == b.Time ? 0
            : -1;
        });
    }
}
