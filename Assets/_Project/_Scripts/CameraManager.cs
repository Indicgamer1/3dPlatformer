using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField, Anywhere] InputReader input;
    [SerializeField, Anywhere] CinemachineFreeLook FreeLookCam;
    [SerializeField] Transform player;
    
    [Header("Settings")]
    [SerializeField, Range(0.5f, 3f)] float speedMultiplier;

    bool isRMBPressed;
    bool cameraMovementLock;
    float cameraXRotation;
    float cameraYRotation;

    Vector2 lastMousePosition;
    private void OnEnable()
    {
        input.Look += OnLook;
        input.EnableMouseControlCamera += OnEnableMouseControlCamera;
        input.DisableMouseControlCamera += OnDisableMouseControlCamera;
    }

    private void OnDisable()
    {
        input.Look -= OnLook;
        input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
        input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
    }

    private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (cameraMovementLock) return;

        if(isDeviceMouse && !isRMBPressed) return;

        //if Device is mouse then use fixedDeltaTime otherwise use deltatime
        float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

        //Set Camera axis value
        FreeLookCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
        FreeLookCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;
        cameraXRotation = FreeLookCam.m_XAxis.m_InputAxisValue;
        cameraYRotation = FreeLookCam.m_YAxis.m_InputAxisValue;
    }

    private void OnEnableMouseControlCamera()
    {
        isRMBPressed = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(DisableMouseForFrame());
    }

    private void OnDisableMouseControlCamera()
    {
        isRMBPressed = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //reset the camera axis to prevent jumping when re-enabling mouse control
        FreeLookCam.m_XAxis.m_InputAxisValue = 0f;
        FreeLookCam.m_YAxis.m_InputAxisValue = 0f;

    }

    private IEnumerator DisableMouseForFrame()
    {
        cameraMovementLock = true;

        yield return new WaitForEndOfFrame();

        cameraMovementLock = false;
    }
    
}
