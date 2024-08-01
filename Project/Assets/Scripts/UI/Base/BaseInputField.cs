using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public abstract class BaseInputField : MonoBehaviour
{
    protected TMP_InputField inputField;

    protected virtual void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    protected virtual void OnEnable()
    {
        inputField.onValueChanged.AddListener(delegate { OnNewInputDoThis(inputField); });
    }

    protected virtual void OnDisable()
    {
        inputField.onValueChanged.RemoveListener(delegate { OnNewInputDoThis(inputField); });
    }

    protected abstract void OnNewInputDoThis(TMP_InputField change);
}
