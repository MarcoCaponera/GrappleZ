using GrappleZ_Player;
using GrappleZ_Utility;
using UnityEngine;


public class PlayerLook : PlayerAbilityBase
{

    #region SerializedField
    [SerializeField]
    private float HorizontalRotationSens;
    [SerializeField]
    private float VerticalRotationSens;
    [SerializeField]
    private float MaxVerticalAngle;
    [SerializeField]
    private float MinVerticalAngle;
    [SerializeField]
    private Transform PlayerCameraTransform;
    #endregion

    #region ProtectedAttributes
    #endregion

    #region Override
    public override void OnInputDisabled()
    {
        isPrevented = true;
    }

    public override void OnInputEnabled()
    {
        isPrevented = false;
    }

    public override void StopAbility()
    {

    }
    #endregion

    #region Mono

    protected void FixedUpdate()
    {
        if (isPrevented) return;
        FillRotationFromInput();
        Rotation();
    }

    #endregion


    #region InternalMethods
    protected void FillRotationFromInput()
    {
        playerController.HorizontalRotation = InputManager.PlayerLook().x;
        playerController.VerticalCameraRotation = -InputManager.PlayerLook().y;
    }

    protected void Rotation()
    {
#if DEBUG
        if (playerController.HorizontalRotation != 0 || playerController.VerticalCameraRotation != 0)
        {
            playerController.OnCameraRotated?.Invoke();
        }
#endif
        Vector3 rotationVector = Vector3.zero;
        rotationVector.y = playerController.HorizontalRotation * HorizontalRotationSens;
        rotationVector.x = playerController.VerticalCameraRotation * VerticalRotationSens;
        float endYRotation = playerController.PlayerTransform.eulerAngles.y + rotationVector.y;
        float endXRotation = PlayerCameraTransform.eulerAngles.x + rotationVector.x;

        playerController.PlayerTransform.eulerAngles = new Vector3(
            playerController.PlayerTransform.eulerAngles.x,
            Mathf.Lerp(playerController.PlayerTransform.eulerAngles.y, endYRotation, Time.deltaTime),
            playerController.PlayerTransform.eulerAngles.z);

        PlayerCameraTransform.eulerAngles = new Vector3(
            Mathf.Lerp(PlayerCameraTransform.eulerAngles.x, endXRotation, Time.deltaTime),
            PlayerCameraTransform.eulerAngles.y,
            PlayerCameraTransform.eulerAngles.z);

        PlayerCameraTransform.localRotation = ClampRotationAroundAxis(
            PlayerCameraTransform.localRotation,
            MinVerticalAngle,
            MaxVerticalAngle);
    }

    private Quaternion ClampRotationAroundAxis(Quaternion q, float minAngle, float maxAngle)
    {

        q.x = q.x / q.w;
        q.y = q.y / q.w;
        q.z = q.z / q.w;
        q.w = 1;

        float angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angle = Mathf.Clamp(angle, minAngle, maxAngle);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);

        return q;
    }
    #endregion
}
