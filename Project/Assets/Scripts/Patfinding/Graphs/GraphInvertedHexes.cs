using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphInvertedHexes : BaseGraph
{
    public override List<Vector2> MovementDirections => new List<Vector2>
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(1, -1),
        new Vector2(0, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 0)
    };

    public List<Vector2> MovementDirectionsDisplacedColumns => new List<Vector2>()
    {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(-1, 1)
    };

    public GraphInvertedHexes(Map.Field[,] mapData, int mapWidth, int mapHeight) : base(mapData, mapWidth, mapHeight)
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

    protected override List<Node> GetNeighbours(int nodeX, int nodeY)
    {
        List<Node> neighbourNodes = new List<Node>();

        List<Vector2> rowDirections = new List<Vector2>();

        if (nodeX != 0 && nodeX % 2 != 0) rowDirections = MovementDirectionsDisplacedColumns;
        else rowDirections = MovementDirections;

        foreach (Vector2 dir in rowDirections)
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

    public override float GetNodesDistance(Node startNode, Node targetNode)
    {
        int distanceX = (int)Mathf.Abs(startNode.Position.x - targetNode.Position.x);
        int distanceY = (int)Mathf.Abs(startNode.Position.y - targetNode.Position.y);

        return (distanceX + distanceY);
    }

    public override void UpdateGraph(Vector2 nodePos, Map.Field[,] mapData)
    {
        nodes[(int)nodePos.x, (int)nodePos.y].UpdateNode((int)nodePos.x, (int)nodePos.y, mapData[(int)nodePos.x, (int)nodePos.y].traversable, mapData[(int)nodePos.x, (int)nodePos.y].cost);

        List<Vector2> neighbours = new List<Vector2>();

        if (nodePos.x != 0 && nodePos.x % 2 != 0) neighbours = MovementDirectionsDisplacedColumns;
        else neighbours = MovementDirections;

        foreach (Vector2 direction in neighbours)
        {
            if (IsWithinBounds((int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y))) nodes[(int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y)].UpdateNeighbours(GetNeighbours((int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y)));
        }
    }
}

