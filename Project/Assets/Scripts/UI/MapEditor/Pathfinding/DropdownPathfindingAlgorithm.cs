using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownPathfindingAlgorithm : BaseDropdownList
{
    protected override List<string> Options => new List<string> { "A*", "Dijkstra" };

    public PathfindingAlgorithmType Algorithm => algorithm;

    private PathfindingAlgorithmType algorithm = PathfindingAlgorithmType.AStar;

    protected override void HandleDropdown(TMP_Dropdown change)
    {
        switch(change.value)
        {
            case 0:
                algorithm = PathfindingAlgorithmType.AStar;
                break;
            case 1:
                algorithm = PathfindingAlgorithmType.Dijkstra;
                break;
        }
    }
}
