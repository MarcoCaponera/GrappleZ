using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCamera : MonoBehaviour
{
    public Camera playerCamera;
    public Camera weaponCamera;
    public LayerMask playerCameraLayerMask;
    public LayerMask weaponCameraLayerMask;

    private void Awake()
    {
        SetWeaponCamera();
    }

    private void SetWeaponCamera()
    {
        playerCamera.cullingMask = playerCameraLayerMask;
        weaponCamera.cullingMask = weaponCameraLayerMask;
        weaponCamera.clearFlags = CameraClearFlags.Depth;
        playerCamera.clearFlags = CameraClearFlags.Skybox;
        weaponCamera.depth = 0.1f;
    }
}
