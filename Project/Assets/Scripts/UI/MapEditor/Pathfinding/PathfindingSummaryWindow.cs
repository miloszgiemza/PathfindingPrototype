using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PathfindingSummaryWindow : MonoBehaviour
{
    [SerializeField] private GameObject window;
    [SerializeField] private TextMeshProUGUI iterationsNumber;
    [SerializeField] private TextMeshProUGUI ifShortestPathGuaranteedInfo;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        window.SetActive(false);
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void GenerateSummaryWindow(int iterationsValue, bool shortestPathValue, bool success)
    {
        window.SetActive(true);
        iterationsNumber.text = "Iterations: " + iterationsValue.ToString() + ";";

        if (shortestPathValue)
        {
            ifShortestPathGuaranteedInfo.text = "Shortest Path Guaranteed: Yes;";
            ifShortestPathGuaranteedInfo.color = Color.green;
        }
        else
        {
            ifShortestPathGuaranteedInfo.text = "Shortest Path Guaranteed: No;";
            ifShortestPathGuaranteedInfo.color = Color.red;
        }
        if(!success)
        {
            ifShortestPathGuaranteedInfo.text = "Path beetwen choosen points does not exist;";
            ifShortestPathGuaranteedInfo.color = Color.red;
        }
    }

    public void HideWindow()
    {
        window.SetActive(false);
    }

    private void SubscribeEvents()
    {
        GameEvents.OnDisplayPathfindingSummary += GenerateSummaryWindow;
    }

    private void UnsubscribeEvents()
    {
        GameEvents.OnDisplayPathfindingSummary -= GenerateSummaryWindow;
    }
}
