using Improbable.Player;
using Improbable.Unity.Visualizer;
using UnityEngine;


public class CameraRigEnabler : MonoBehaviour
{
    [Require]
    protected LocalPlayerCheckWriter LocalPlayerChecker;

    public bool UseVR = true;
    public GameObject CameraRig;
    public GameObject NonVRCamera;

    void OnEnable()
    {
        if (UseVR)
        {
            if (CameraRig != null)
            {
                CameraRig.SetActive(true);
            }

        } else {
            if (NonVRCamera != null)
            {
                NonVRCamera.SetActive(true);
            }
        }
       
    }
}