using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using System;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Slider loadbar;

    private float loadbarRefreshRate = 0.016f;
    private PoolerStage currentLoadingScreenStage = PoolerStage.GeneratingSquares;

    private Dictionary<PoolerStage, string> stageTexts = new Dictionary<PoolerStage, string>() 
    { 
        { PoolerStage.GeneratingSquares, "Please, wait. Generating square tilemap environment in progress..."}, 
        {PoolerStage.GeneratingHexes,  "Please, wait. Generating hex tilemap environment in progress..."},
        {PoolerStage.GeneratingInvertedHexes, "Please, wait. Generating inverted hex tilemap environment in progress..." },
        { PoolerStage.Finished, "Almost complited..."}
    };

    private void Start()
    {
        loadbar.maxValue = 3;
        loadbar.value = 0.4f;
        StartCoroutine(RunLoadingScreen());
    }

    private IEnumerator RunLoadingScreen()
    {
        while (PoolerProgress.Progress!=PoolerStage.Finished)
        {
            if(currentLoadingScreenStage != PoolerProgress.Progress)
            {
                currentLoadingScreenStage = PoolerProgress.Progress;

                switch (currentLoadingScreenStage)
                {
                    case PoolerStage.GeneratingSquares:
                        textField.text = stageTexts[currentLoadingScreenStage];
                        loadbar.value = 0f;
                        break;
                    case PoolerStage.GeneratingHexes:
                        textField.text = stageTexts[currentLoadingScreenStage];
                        loadbar.value = 2f;
                        break;
                    case PoolerStage.GeneratingInvertedHexes:
                        textField.text = stageTexts[currentLoadingScreenStage];
                        loadbar.value = 3f;
                        break;
                    case PoolerStage.Finished:
                        textField.text = stageTexts[currentLoadingScreenStage];
                        break;
                }
            }

            yield return new WaitForSeconds(loadbarRefreshRate);
        }
        gameObject.SetActive(false);
    }
}
