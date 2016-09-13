using UnityEngine;

public class LocalPlayerModelSynchronizer : MonoBehaviour
{
    public Transform DeviceLeftHandTransform;
    public Transform DeviceRightHandTransform;
    public Transform DeviceHeadTransform;

    public Transform ModelLeftHandTransform;
    public Transform ModelRightHandTransform;
    public Transform ModelHeadTransform;

    void Update ()
    {
        ModelLeftHandTransform.position = DeviceLeftHandTransform.position;
        ModelRightHandTransform.position = DeviceRightHandTransform.position;
        ModelHeadTransform.position = DeviceHeadTransform.position;

        ModelLeftHandTransform.rotation = DeviceLeftHandTransform.rotation;
        ModelRightHandTransform.rotation = DeviceRightHandTransform.rotation;
        ModelHeadTransform.rotation = DeviceHeadTransform.rotation;
    }
}