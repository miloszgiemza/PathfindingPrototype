using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer virtualCameraTransposser;

    private float movmentSpeed = 290f;
    Vector3 moveVector = new Vector3(0, 0, 0);

    private float rotationSpeed = 40f;

    private float maxCameraYOffset = 742.336f;
    private float minCameraYOffset = 2f;

    private float zoomSpeed = 650f;

    private void Awake()
    {
        virtualCameraTransposser = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        moveVector = transform.forward * InputManager.Instance.CameraMovement.z + transform.right * InputManager.Instance.CameraMovement.x;
        transform.position += moveVector * movmentSpeed * Time.deltaTime;

        transform.eulerAngles += InputManager.Instance.CameraRotation * rotationSpeed * Time.deltaTime;

        virtualCameraTransposser.m_FollowOffset.y = Mathf.Clamp(virtualCameraTransposser.m_FollowOffset.y + InputManager.Instance.CameraZoom * zoomSpeed * Time.deltaTime, minCameraYOffset, maxCameraYOffset);
    }
}
