using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCameraHandler : MonoBehaviour
{
    [SerializeField] private GameObject topDownCamera;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private Button switchButton;

    private bool isThirdPersonCamera = true;

    private void Start()
    {
        switchButton.onClick.RemoveAllListeners();
        switchButton.onClick.AddListener(() =>
        {
            isThirdPersonCamera = !isThirdPersonCamera;

            thirdPersonCamera.SetActive(isThirdPersonCamera);
            topDownCamera.SetActive(!isThirdPersonCamera);

            CameraEvent.NotifyOnSwitch(isThirdPersonCamera);
        });
    }
}

public static class CameraEvent
{
    public static Action<bool> OnSwitch;

    public static void NotifyOnSwitch(bool isThirdPerson)
    {
        OnSwitch?.Invoke(isThirdPerson);
    }
}
