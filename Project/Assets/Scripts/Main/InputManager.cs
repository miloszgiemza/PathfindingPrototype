using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance => instance;
    private static InputManager instance;

    public Vector3 CameraMovement => cameraMovement;
    public Vector3 CameraRotation => cameraRotation;
    public float CameraZoom => cameraZoom;

    public Vector3 SelectorHoverPosition => currentMousePosition;

    [SerializeField] private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private Vector3 cameraMovement = new Vector3(0, 0, 0);
    private Vector3 cameraRotation = new Vector3(0, 0, 0);

    private float cameraZoom = 0f;

    private Vector3 previousMousePosition = new Vector3(0, 0, 0);
    private Vector3 currentMousePosition = new Vector3(0, 0, 0);
    private bool firstFrame = true;

    private void Awake()
    {
        if (!ReferenceEquals(MainManager.Instance, null))
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        eventSystem = FindObjectOfType<EventSystem>(); 
    }

    private void Update()
    {
        UpdateCameraInput();
        UpdateTileSelectionInput();
    }

    public bool CheckIfOverUI()
    {
        bool overUI = false;

        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (!ReferenceEquals(result.gameObject.GetComponent<RectTransform>(), null)) overUI = true;
        }

        return overUI;
    }

    #region Camera

    private void UpdateCameraMovmentInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cameraMovement.z = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cameraMovement.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            cameraMovement.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            cameraMovement.x = +1;
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            cameraMovement.z = 0f;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            cameraMovement.x = 0f;
        }
    }

    private void UpdateCameraRotationInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            cameraRotation.y = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            cameraRotation.y = +1f;
        }

        if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            cameraRotation.y = 0f;
        }
    }

    private void UpdateCameraZoomInput()
    {
        float zoomSpeed = 1f;

        if (Input.mouseScrollDelta.y > 0)
        {
            cameraZoom -= zoomSpeed;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            cameraZoom += zoomSpeed;
        }
        if (Input.mouseScrollDelta.y == 0)
        {
            cameraZoom = 0f;
        }
    }

    private void UpdateCameraInput()
    {
        UpdateCameraMovmentInput();
        UpdateCameraRotationInput();
        UpdateCameraZoomInput();
    }
    #endregion

    #region Selector
    private void UpdateTileSelectorSelection()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!CheckIfOverUI())
            {
                GameEvents.OnClickedToSelectTile.Invoke(Input.mousePosition);
            }
        }
    }

    private void UpdateTileSelectionHover()
    {
        if (firstFrame)
        {
            firstFrame = false;
            previousMousePosition = Input.mousePosition;
        }
        else
        {
            currentMousePosition = Input.mousePosition;
            if (currentMousePosition != previousMousePosition)
            {
                GameEvents.OnHoverPositionChanged.Invoke(currentMousePosition);
            }
            previousMousePosition = currentMousePosition;
        }
    }

    private void UpdateTileSelectionInput()
    {
        UpdateTileSelectorSelection();
        UpdateTileSelectionHover();
    }
    #endregion
}
