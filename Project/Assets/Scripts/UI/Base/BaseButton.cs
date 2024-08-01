using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    protected Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(DoThisOnButtonClicked);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(DoThisOnButtonClicked);
    }

    protected abstract void DoThisOnButtonClicked();
}
