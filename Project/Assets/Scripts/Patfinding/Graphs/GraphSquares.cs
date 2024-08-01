using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSquares : BaseGraph
{
    public override List<Vector2> MovementDirections => new List<Vector2>
        {
            //first non-diagonal not to walk in strange diagonal patterns with the samne movement cost
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(0, -1),
            new Vector2(-1, 0),

            new Vector2(1, 1),
            new Vector2(1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, 1)
        };

    public GraphSquares(Map.Field[,] mapData, int mapWidth, int mapHeight) : base(mapData, mapWidth, mapHeight)
    {
        xAxisLength = mapWidth;
        yAxisLength = mapHeight;

        nodes = new Node[xAxisLength, yAxisLength];

        for (int y = 0; y < yAxisLength; y++)
        {
            for (int x = 0; x < xAxisLength; x++)
            {
                Node newNode = new Node();
                newNode.Initialize(x, y, mapData[x, y].traversable, mapData[x, y].cost);
                nodes[x, y] = newNode;
            }
        }

        for (int y = 0; y < yAxisLength; y++)
        {
            for (int x = 0; x < xAxisLength; x++)
            {
                if (nodes[x, y].Traversable)
                {
                    nodes[x, y].UpdateNeighbours(GetNeighbours(x, y));
                }
            }
        }
    }

    public override float GetNodesDistance(Node startNode, Node targetNode)
    {
        int distanceX = (int)Mathf.Abs(startNode.Position.x - targetNode.Position.x);
        int distanceY = (int)Mathf.Abs(startNode.Position.y - targetNode.Position.y);

        int min = Mathf.Min(distanceX, distanceY);
        int max = Mathf.Max(distanceX, distanceY);

        int diagonalPathLength = min;
        int straightPathLength = max - min;

        return (diagonalPathLength + straightPathLength);
    }

    public override void UpdateGraph(Vector2 nodePos, Map.Field[,] mapData)
    {
        nodes[(int)nodePos.x, (int)nodePos.y].UpdateNode((int)nodePos.x, (int)nodePos.y, mapData[(int)nodePos.x, (int)nodePos.y].traversable, mapData[(int)nodePos.x, (int)nodePos.y].cost);

        foreach (Vector2 direction in MovementDirections)
        {
            if(IsWithinBounds((int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y)))nodes[(int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y)].UpdateNeighbours(GetNeighbours((int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y)));
        }
    }

    protected override List<Node> GetNeighbours(int nodeX, int nodeY)
    {
        List<Node> neighbourNodes = new List<Node>();

        foreach (Vector2 dir in MovementDirections)
        {
            int neighboursX = nodeX + (int)dir.x;
            int neighboursY = nodeY + (int)dir.y;

            if (IsWithinBounds(neighboursX, neighboursY) && nodes[neighboursX, neighboursY].Traversable)
            {
                neighbourNodes.Add(nodes[neighboursX, neighboursY]);
            }
        }

        return neighbourNodes;
    }
}