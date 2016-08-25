using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using Improbable.Unity.Common.Core.Math;
using UnityEngine;

public class RadioControl : MonoBehaviour {

    [Require]
    protected RadioControlWriter radioControl;

    public SteamVR_TrackedObject LeftController;
    public SteamVR_TrackedObject RightController;

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private bool isRadioOn = false;

    void Update()
    {
        SteamVR_Controller.Device leftDevice = (LeftController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)LeftController.index) : null;
        SteamVR_Controller.Device rightDevice = (RightController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)RightController.index) : null;

        if ((leftDevice != null && leftDevice.GetPressDown(gripButton)) || (rightDevice != null && rightDevice.GetPressDown(gripButton)))
        {
            if (!isRadioOn)
            {
                radioControl.Update.TriggerTurnOn(this.transform.position.ToNativeVector()).FinishAndSend();
                isRadioOn = true;
            } else {
                radioControl.Update.TriggerTurnOff().FinishAndSend();
                isRadioOn = false;
            }
        }
    }
}
