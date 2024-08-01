using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAStar : BaseSearch
{
    public SearchAStar(BaseGraph graphValue, Node startNodeValue, Node endNodeValue) : base(graphValue, startNodeValue, endNodeValue) { }

    public override bool ShortestPathGuaranteed => true;

    public override List<Vector2> SearchAndReturnPath()
    {
        InitSearch();

        Node currentNode = new Node();

        while (frontierNodes.Count > 0 && currentNode != endNode)
        {
            currentNode = frontierNodes[0];
            frontierNodes.RemoveAt(0);

            if (!exploredNodes.Contains(currentNode)) exploredNodes.Add(currentNode);

            ExpandFrontier(currentNode);


            if (frontierNodes.Contains(endNode))
            {
                pathNodes = GetPathNodes(endNode);
            }

            iterations++;
        }


        List<Vector2> pathPositions = new List<Vector2>();

        foreach (Node node in pathNodes)
        {
            pathPositions.Add(node.Position);
        }

        return pathPositions;
    }

    protected override void ExpandFrontier(Node node)
    {
        for (int i = 0; i < node.Neighbours.Count; i++)
        {
            if (!exploredNodes.Contains(node.Neighbours[i]))
            {
                float distanceToNeighbour = graph.GetNodesDistance(node, node.Neighbours[i]) + node.Neighbours[i].MovementCost;
                float newDistanceTravelled = distanceToNeighbour + node.DistanceTravelled + node.MovementCost;

                if (float.IsPositiveInfinity(node.Neighbours[i].DistanceTravelled) || newDistanceTravelled < node.Neighbours[i].DistanceTravelled)
                {
                    node.Neighbours[i].SetPreviousNode(node);
                    node.Neighbours[i].SetDistanceTravelled(newDistanceTravelled);
                }

                if (!frontierNodes.Contains(node.Neighbours[i]))
                {
                    int predictedDistanceToGoal = (int)graph.GetNodesDistance(node.Neighbours[i], endNode);
                    node.Neighbours[i].SetPriority((int)node.Neighbours[i].DistanceTravelled + predictedDistanceToGoal);

                    frontierNodes.Add(node.Neighbours[i]);
                    frontierNodes = Node.SortListByPriority(frontierNodes); //symulacja kolejki priorytetowej 
                }
            }
        }
    }
}