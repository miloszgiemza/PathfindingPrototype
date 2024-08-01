using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldMovementCost : BaseInputFieldInteger
{
    protected override int MinValue => 0;

    protected override int MaxValue => 20;

    protected override void Awake()
    {
        base.Awake();
        currentValue = 0;
    }

    public void SetText(string textValue)
    {
        inputField.text = textValue;
    }

    public void SetInactive()
    {
        inputField.text = "Not Applicable";
        inputField.interactable = false;
        //inputField.text = "0";
        currentValue = 0;
    }

    public void SetActive()
    {
        inputField.text = currentValue.ToString();
        inputField.interactable = true;
    }
}
