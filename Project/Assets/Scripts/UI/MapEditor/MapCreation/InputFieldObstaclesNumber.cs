using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldObstaclesNumber : BaseInputFieldInteger
{
    protected override int MinValue => 0;

    protected override int MaxValue => currentMaxValue;

    private MapCreationUIController mapCreationUIController;

    int currentMaxValue = 100;

    protected override void Awake()
    {
        base.Awake();
        mapCreationUIController = GetComponentInParent<MapCreationUIController>();
        currentValue = 0;
    }

    public void UpdateMaxValueAndDisplayedValue()
    {
        currentMaxValue = mapCreationUIController.InputFieldMapSizeX.CurrentValue * mapCreationUIController.InputFieldMapSizeZ.CurrentValue;
        currentValue = Mathf.Clamp(currentValue, MinValue, currentMaxValue);
        inputField.text = currentValue.ToString();
    }
}
