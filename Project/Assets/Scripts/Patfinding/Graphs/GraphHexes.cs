using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Base;
using System;

public class GraphHexes : BaseGraph
{
    public override List<Vector2> MovementDirections => new List<Vector2>()
    {
            new Vector2(0f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, -1f),
            new Vector2(-1f, -1f),
            new Vector2(-1f, 0f),
            new Vector2(-1f, 1f)
    };

    public List<Vector2> MovementDirectionsDisplacedRow => new List<Vector2>()
    {
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(1f, -1f),
            new Vector2(0f, -1f),
            new Vector2(-1f, 0f),
            new Vector2(0f, 1f)
    };

    public GraphHexes(Map.Field[,] mapData, int mapWidth, int mapHeight) : base(mapData, mapWidth, mapHeight)
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

        if (nodeY != 0 && nodeY % 2 != 0) rowDirections = MovementDirectionsDisplacedRow;
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

        if (nodePos.y != 0 && nodePos.y % 2 != 0) neighbours = MovementDirectionsDisplacedRow;
        else neighbours = MovementDirections;

        foreach (Vector2 direction in neighbours)
        {
            if (IsWithinBounds((int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y))) nodes[(int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y)].UpdateNeighbours(GetNeighbours((int)(nodePos.x + direction.x), (int)(nodePos.y + direction.y)));
        }
    }
}
