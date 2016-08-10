using Improbable.Player;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class InputSender : MonoBehaviour
{
    [Require]
    protected PlayerControlsWriter PlayerControls;


    public float MovementSpeed = 4f;
    public float MinMovementMagnitude = 0.1f;
    public Transform HeadsetTransform;
    public SteamVR_TrackedObject LeftController;
    public SteamVR_TrackedObject RightController;


    void Update()
    {
        SteamVR_Controller.Device leftDevice = (LeftController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)LeftController.index) : null;
        SteamVR_Controller.Device rightDevice = (RightController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)RightController.index) : null;
        Vector3 headDirection = (HeadsetTransform == null) ? Vector3.zero : new Vector3(HeadsetTransform.forward.x, 0f, HeadsetTransform.forward.z).normalized;
        Vector3 leftInputDirection = (leftDevice == null) ? Vector3.zero : new Vector3(leftDevice.GetAxis().x, 0f, leftDevice.GetAxis().y);
        Vector3 rightInputDirection = (rightDevice == null) ? Vector3.zero : new Vector3(rightDevice.GetAxis().x, 0f, rightDevice.GetAxis().y);
        Vector3 inputDirection = (leftInputDirection + rightInputDirection).normalized;
        if (inputDirection.magnitude >= MinMovementMagnitude)
        {
            Vector3 movementDirection = Quaternion.LookRotation(headDirection) * inputDirection * MovementSpeed * Time.deltaTime;
            PlayerControls.Update.TriggerMoveEvent(movementDirection.ToNativeVector()).FinishAndSend();
        }
    }
}