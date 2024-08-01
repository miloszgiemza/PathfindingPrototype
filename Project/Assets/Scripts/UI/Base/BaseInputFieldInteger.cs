using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

public abstract class BaseInputFieldInteger : BaseInputField
{

    public int CurrentValue => currentValue;

    protected int currentValue = 10;
    protected abstract int MinValue { get; }
    protected abstract int MaxValue { get; }

    protected override void OnNewInputDoThis(TMP_InputField change)
    {
        if(Int32.TryParse(change.text, out currentValue))
        {
            currentValue = Mathf.Clamp(currentValue, MinValue, MaxValue);
            if (Convert.ToInt32(inputField.text) > MaxValue || Convert.ToInt32(inputField.text) < MinValue) inputField.text = currentValue.ToString();
        }
    }
}
