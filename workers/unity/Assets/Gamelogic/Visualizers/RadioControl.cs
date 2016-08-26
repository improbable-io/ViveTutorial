using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using Improbable.Unity.Common.Core.Math;
using UnityEngine;
using System.Collections;

public class RadioControl : MonoBehaviour {

    [Require]
    protected RadioControlWriter radioControl;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            radioControl.Update.TriggerTurnOn(this.transform.position.ToNativeVector()).FinishAndSend();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            radioControl.Update.TriggerTurnOff().FinishAndSend();
        }
    }
}
