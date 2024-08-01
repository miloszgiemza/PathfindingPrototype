using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public enum ErrorMessageType
{
    ChooseStartAndEndOfPath,
    NoMapCreated
}

public class ErrorMessagesController : MonoBehaviour
{
    [SerializeField] private GameObject errorMessageWindow;
    [SerializeField] private TextMeshProUGUI errorText;

    private Dictionary<ErrorMessageType, string> messagesTexts = new Dictionary<ErrorMessageType, string>() 
    { { ErrorMessageType.ChooseStartAndEndOfPath, "TO RUN PATHFINDING CHOOSE PATH'S START AND END TILES"}, { ErrorMessageType.NoMapCreated, "YOU NEED TO CREATE A MAP TO RUN PATHFINDING"} };

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void HideErrorMessage()
    {
        errorMessageWindow.SetActive(false);
    }

    private void ShowErrorMessage(ErrorMessageType errorMessageType)
    {
        errorMessageWindow.SetActive(true);

        switch(errorMessageType)
        {
            case ErrorMessageType.ChooseStartAndEndOfPath:
                errorText.text = messagesTexts[ErrorMessageType.ChooseStartAndEndOfPath];
                break;
            case ErrorMessageType.NoMapCreated:
                errorText.text = messagesTexts[ErrorMessageType.NoMapCreated];
                break;
        }
    }

    private void SubscribeEvents()
    {
        GameEvents.OnShowErrorMessage += ShowErrorMessage;
        GameEvents.OnHideErrorMessage += HideErrorMessage;
    }

    private void UnsubscribeEvents()
    {
        GameEvents.OnShowErrorMessage += ShowErrorMessage;
        GameEvents.OnHideErrorMessage += HideErrorMessage;
    }
}
