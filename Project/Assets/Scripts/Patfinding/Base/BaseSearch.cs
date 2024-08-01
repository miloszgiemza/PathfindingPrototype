using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Base;

public abstract class BaseSearch
{
    public List<Vector2> Path => path;

    protected Node startNode;
    protected Node endNode;
    protected BaseGraph graph;

    protected List<Node> frontierNodes; //jako kolejka

    protected List<Node> exploredNodes;
    protected List<Node> pathNodes;

    protected List<Vector2> path;

    public abstract bool ShortestPathGuaranteed { get; }
    protected int iterations;

    public BaseSearch(BaseGraph graphValue, Node startNodeValue, Node endNodeValue)
    {
        startNode = startNodeValue;
        startNode.SetDistanceTravelled(0);

        endNode = endNodeValue;
        graph = graphValue;
    }

    protected void InitSearch()
    {

        frontierNodes = new List<Node>();
        frontierNodes.Add(startNode);
        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();
        path = new List<Vector2>();

        startNode.SetDistanceTravelled(0);
    }

    public abstract List<Vector2> SearchAndReturnPath();
    protected abstract void ExpandFrontier(Node node);

    protected List<Node> GetPathNodes(Node endNode)
    {
        //powrót od end noda po zapamiêtanych previous nodach - œcie¿ka
        List<Node> path = new List<Node>();

        path.Add(endNode);

        Node currentNode = endNode.PreviousNode;

        while (!ReferenceEquals(currentNode, null))
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    public void DisplayPathfindingSummary(List<Vector2> pathValue)
    {
        bool pathExists = true;
        if(pathValue.Count < 1 || (pathValue[pathValue.Count - 1].x != endNode.Position.x || pathValue[pathValue.Count - 1].y != endNode.Position.y)) pathExists = false; ;

        GameEvents.OnHideErrorMessage.Invoke();
        GameEvents.OnDisplayPathfindingSummary.Invoke(iterations, ShortestPathGuaranteed, pathExists);
    }
}