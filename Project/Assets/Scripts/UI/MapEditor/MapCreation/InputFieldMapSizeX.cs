using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldMapSizeX : BaseInputFieldMapSize
{
    private MapCreationUIController mapCreationUIController;

    protected override void Awake()
    {
        base.Awake();
        mapCreationUIController = GetComponentInParent<MapCreationUIController>();
    }

    protected override void OnNewInputDoThis(TMP_InputField change)
    {
        base.OnNewInputDoThis(change);
        mapCreationUIController.InputFieldObstaclesNumber.UpdateMaxValueAndDisplayedValue();
    }
}
