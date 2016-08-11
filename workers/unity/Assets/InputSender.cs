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

    public GameObject LeftHand;
    public GameObject RightHand;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;


    void Update()
    {
        SteamVR_Controller.Device leftDevice = (LeftController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)LeftController.index) : null;
        SteamVR_Controller.Device rightDevice = (RightController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)RightController.index) : null;
        Vector3 headDirection = (HeadsetTransform == null) ? Vector3.zero : new Vector3(HeadsetTransform.forward.x, 0f, HeadsetTransform.forward.z).normalized;


        //Movement
        Vector3 leftInputDirection = (leftDevice == null) ? Vector3.zero : new Vector3(leftDevice.GetAxis().x, 0f, leftDevice.GetAxis().y);
        Vector3 rightInputDirection = (rightDevice == null) ? Vector3.zero : new Vector3(rightDevice.GetAxis().x, 0f, rightDevice.GetAxis().y);
        Vector3 inputDirection = (leftInputDirection + rightInputDirection).normalized;
        if (inputDirection.magnitude >= MinMovementMagnitude)
        {
            Vector3 movementDirection = Quaternion.LookRotation(headDirection) * inputDirection * MovementSpeed * Time.deltaTime;
            PlayerControls.Update.TriggerMoveEvent(movementDirection.ToNativeVector()).FinishAndSend();
        }

        //Picking up objects
        if (leftDevice != null && leftDevice.GetPress(triggerButton))
        {
            if (LeftHand.GetComponent<GrabItemsBehaviour>().touchedObject != null)
            {
                PlayerControls.Update.TriggerPickUpEvent(LeftHand.GetComponent<GrabItemsBehaviour>().touchedObject.EntityId(), "left").FinishAndSend();
            }
        }

        if (rightDevice != null && rightDevice.GetPress(triggerButton))
        {
            if (RightHand.GetComponent<GrabItemsBehaviour>().touchedObject != null)
            {
                PlayerControls.Update.TriggerPickUpEvent(RightHand.GetComponent<GrabItemsBehaviour>().touchedObject.EntityId(), "right").FinishAndSend();
            }
        }

        //Dropping objects
        if (leftDevice != null && leftDevice.GetPressUp(triggerButton))
        {
            if (LeftHand.GetComponent<FixedJoint>().connectedBody != null)
            {
                PlayerControls.Update.TriggerDropEvent(LeftHand.GetComponent<GrabItemsBehaviour>().touchedObject.EntityId(), "left").FinishAndSend();
            }
        }
        if (rightDevice != null && rightDevice.GetPressUp(triggerButton))
        {
            if (RightHand.GetComponent<FixedJoint>().connectedBody != null)
            {
                PlayerControls.Update.TriggerDropEvent(RightHand.GetComponent<GrabItemsBehaviour>().touchedObject.EntityId(), "right").FinishAndSend();
            }
        }

    }
}