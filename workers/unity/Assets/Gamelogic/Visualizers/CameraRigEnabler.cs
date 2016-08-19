using Improbable.Player;
using Improbable.Unity.Visualizer;
using UnityEngine;


public class CameraRigEnabler : MonoBehaviour
{
    [Require]
    protected LocalPlayerCheckWriter LocalPlayerChecker;
    public GameObject CameraRig;


    void OnEnable()
    {
        if (CameraRig != null)
        {
            CameraRig.SetActive(true);
        }
    }
}