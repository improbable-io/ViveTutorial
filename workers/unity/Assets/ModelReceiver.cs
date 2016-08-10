using Improbable.Player;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;


public class ModelReceiver : MonoBehaviour
{
    [Require]
    protected PlayerControlsReader PlayerControls;


    public Transform ModelLeftHandTransform;
    public Transform ModelRightHandTransform;
    public Transform ModelHeadTransform;


    void Update()
    {
        ModelLeftHandTransform.position = PlayerControls.LeftHandPosition.ToUnityVector();
        ModelRightHandTransform.position = PlayerControls.RightHandPosition.ToUnityVector();
        ModelHeadTransform.position = PlayerControls.HeadPosition.ToUnityVector();


        ModelLeftHandTransform.rotation = PlayerControls.LeftHandRotation.ToUnityQuaternion();
        ModelRightHandTransform.rotation = PlayerControls.RightHandRotation.ToUnityQuaternion();
        ModelHeadTransform.rotation = PlayerControls.HeadRotation.ToUnityQuaternion();
    }
}