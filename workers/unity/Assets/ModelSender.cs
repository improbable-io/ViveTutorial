using Improbable.Player;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class ModelSender : MonoBehaviour {
    [Require]
    protected PlayerControlsWriter PlayerControls;

    public Transform DeviceLeftHandTransform;
    public Transform DeviceRightHandTransform;
    public Transform DeviceHeadTransform;

    void Update()
    {
        if (DeviceLeftHandTransform.gameObject.activeSelf)
        {
            PlayerControls.Update
                .LeftHandPosition(DeviceLeftHandTransform.position.ToNativeVector())
                .LeftHandRotation(DeviceLeftHandTransform.rotation.eulerAngles.ToNativeVector())
                .FinishAndSend();
        }
        if (DeviceRightHandTransform.gameObject.activeSelf)
        {
            PlayerControls.Update
                .RightHandPosition(DeviceRightHandTransform.position.ToNativeVector())
                .RightHandRotation(DeviceRightHandTransform.rotation.eulerAngles.ToNativeVector())
                .FinishAndSend();
        }
        if (DeviceHeadTransform.gameObject.activeSelf)
        {
            PlayerControls.Update
                .HeadPosition(DeviceHeadTransform.position.ToNativeVector())
                .HeadRotation(DeviceHeadTransform.rotation.eulerAngles.ToNativeVector())
                .FinishAndSend();
        }
    }
}
