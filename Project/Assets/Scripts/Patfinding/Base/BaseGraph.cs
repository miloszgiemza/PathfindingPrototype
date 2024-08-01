using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Base;

public abstract class BaseGraph
{
    public int XAxisLength => xAxisLength;
    public int YAxisLength => yAxisLength;
    public Node[,] Nodes => nodes;

    protected int xAxisLength;
    protected int yAxisLength;

    protected Node[,] nodes;

    public abstract List<Vector2> MovementDirections { get; }


    public BaseGraph(Map.Field[,] mapData, int mapWidth, int mapHeight)
    {
    }


    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < xAxisLength && y >= 0 && y < yAxisLength);
    }

    protected abstract List<Node> GetNeighbours(int nodeX, int nodeY);


    public abstract void UpdateGraph(Vector2 nodePos, Map.Field[,] mapData);

    public abstract float GetNodesDistance(Node startNode, Node targetNode);

    public void ResetGraphAfterPathfinding()
    {
        for (int x = 0; x < xAxisLength; x++)
        {
            for (int y = 0; y < yAxisLength; y++)
            {
                nodes[x, y].ResetNodeFromPathfindingData();
            }
        }
    }
}