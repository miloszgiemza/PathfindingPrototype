using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapCreationUIController : MonoBehaviour
{
    public InputFieldMapSizeX InputFieldMapSizeX => inputFieldSizeX;
    public InputFieldMapSizeZ InputFieldMapSizeZ => inputFieldSizeZ;
    public InputFieldObstaclesNumber InputFieldObstaclesNumber => inputFieldObstaclesNumber;

    private Button buttonApply;

    private InputFieldMapSizeX inputFieldSizeX;
    private InputFieldMapSizeZ inputFieldSizeZ;
    private InputFieldObstaclesNumber inputFieldObstaclesNumber;
    private DropdownMapTilesType dropdownMapTilesType;

    private void Awake()
    {
        buttonApply = GetComponentInChildren<Button>();

        inputFieldSizeX = GetComponentInChildren<InputFieldMapSizeX>();
        inputFieldSizeZ = GetComponentInChildren<InputFieldMapSizeZ>();
        inputFieldObstaclesNumber = GetComponentInChildren<InputFieldObstaclesNumber>();
        dropdownMapTilesType = GetComponentInChildren<DropdownMapTilesType>();
    }

    private void OnEnable()
    {
        buttonApply.onClick.AddListener(SendInputForMapGeneration);
    }

    private void OnDisable()
    {
        buttonApply.onClick.RemoveListener(SendInputForMapGeneration);
    }

    public void SendInputForMapGeneration()
    {
        GameEvents.OnCreateNewMap.Invoke(inputFieldSizeX.CurrentValue, inputFieldSizeZ.CurrentValue, inputFieldObstaclesNumber.CurrentValue, dropdownMapTilesType.TilesType);
    }
}
