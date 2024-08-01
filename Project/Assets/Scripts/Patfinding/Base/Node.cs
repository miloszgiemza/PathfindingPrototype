using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Base;

public class Node
{
    public Vector2 Position => position;
    public bool Traversable => traversable;
    public float MovementCost => movementCost;
    public List<Node> Neighbours => neighbours;
    public Node PreviousNode => previousNode;
    public float DistanceTravelled => distanceTravelled;


    protected Vector2 position = new Vector2();
    protected bool traversable = true;
    protected float movementCost = 0f;

    protected List<Node> neighbours = new List<Node>();
    protected Node previousNode = null;
    protected float distanceTravelled = Mathf.Infinity;

    public void Initialize(int x, int y, bool traversableValue, float movementCostValue)
    {
        position = new Vector2(x, y);
        traversable = traversableValue;
        movementCost = movementCostValue;
    }

    public void UpdateNode(int x, int y, bool traversableValue, float movementCostValue)
    {
        //Does not null or update neighbours
        position = new Vector2(x, y);
        traversable = traversableValue;
        movementCost = movementCostValue;
        previousNode = null;
        distanceTravelled = Mathf.Infinity;
    }

    public void ResetNodeFromPathfindingData()
    {
        //Does not null or update neighbours
        previousNode = null;
        distanceTravelled = Mathf.Infinity;
    }

    public void UpdateNeighbours(List<Node> neighboursValue)
    {
        neighbours.Clear();
        neighbours = null;
        neighbours = neighboursValue;
    }

    public void SetDistanceTravelled(float distanceTravelledValue)
    {
        distanceTravelled = distanceTravelledValue;
    }

    public void SetPreviousNode(Node node)
    {
        previousNode = node;
    }

    public static List<Node> SortListByDistanceFromStartAscending(List<Node> nodes)
    {
        List<Node> unsortedNodes = nodes;
        List<Node> sortedNodes = new List<Node>();

        while (unsortedNodes.Count > 0)
        {
            Node min = unsortedNodes[0];
            int minIndex = 0;

            for (int i = 0; i < unsortedNodes.Count; i++)
            {
                if (unsortedNodes[i].DistanceTravelled < min.DistanceTravelled)
                {
                    min = unsortedNodes[i];
                    minIndex = i;
                }
            }

            sortedNodes.Add(min);
            unsortedNodes.RemoveAt(minIndex);
        }

        return sortedNodes;
    }

    #region A*PriorityQueue
    public int Priority => priority;

    private int priority;

    public void SetPriority(int priorityValue)
    {
        priority = priorityValue;
    }

    public static List<Node> SortListByPriority(List<Node> nodes)
    {
        List<Node> unsortedNodes = nodes;
        List<Node> sortedNodes = new List<Node>();

        while (unsortedNodes.Count > 0)
        {
            Node min = unsortedNodes[0];
            int minIndex = 0;

            for (int i = 0; i < unsortedNodes.Count; i++)
            {
                if (unsortedNodes[i].priority < min.priority)
                {
                    min = unsortedNodes[i];
                    minIndex = i;
                }
            }

            sortedNodes.Add(min);
            unsortedNodes.RemoveAt(minIndex);
        }

        return sortedNodes;
    }
    #endregion
}

