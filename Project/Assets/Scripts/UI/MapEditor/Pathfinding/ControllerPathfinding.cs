using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathfindingAlgorithmType
{
    AStar,
    Dijkstra
}

public class ControllerPathfinding : MonoBehaviour
{
    private DropdownPathfindingAlgorithm dropdownPathfindingAlgorithm;

    private void Awake()
    {
        dropdownPathfindingAlgorithm = GetComponentInChildren<DropdownPathfindingAlgorithm>();
    }

    public void RunPathfinding()
    {
        GameEvents.OnRunPathfinding.Invoke(dropdownPathfindingAlgorithm.Algorithm);
    }
}
