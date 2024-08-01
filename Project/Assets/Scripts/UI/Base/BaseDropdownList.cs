using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public abstract class BaseDropdownList : MonoBehaviour
{
    protected abstract List<string> Options { get; }

    protected TMP_Dropdown dropdown;

    protected virtual void Awake()
    {
        dropdown = GetComponent<TMPro.TMP_Dropdown>();
    }

    protected virtual void OnEnable()
    {
        dropdown.onValueChanged.AddListener(delegate { HandleDropdown(dropdown); });
    }

    protected virtual void Start()
    {
        dropdown.AddOptions(Options);
    }

    protected virtual void OnDisable()
    {
       dropdown.onValueChanged.RemoveListener(delegate { HandleDropdown(dropdown); });
    }

    protected abstract void HandleDropdown(TMP_Dropdown change);
}
