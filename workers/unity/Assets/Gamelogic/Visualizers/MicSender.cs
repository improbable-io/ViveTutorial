using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using UnityEngine;
using System.Collections;
using Improbable.Util.Collections;

public class MicSender : MonoBehaviour
{
    [Require]
    protected SoundsWriter spatialSounds;

    private AudioClip clip;
    private int lastSampleOffset;
    private int finishOffset;
    private const int FREQUENCY = 44100;

    public SteamVR_TrackedObject LeftController;
    public SteamVR_TrackedObject RightController;

    private Valve.VR.EVRButtonId appButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    private bool isMicOn = false;

    void Start()
    {
        clip = null;
        lastSampleOffset = 0;
        finishOffset = -1;
    }

   

    void Update()
    {
        SteamVR_Controller.Device leftDevice = (LeftController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)LeftController.index) : null;
        SteamVR_Controller.Device rightDevice = (RightController.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)RightController.index) : null;

        if ((leftDevice != null && leftDevice.GetPressDown(appButton)) || (rightDevice != null && rightDevice.GetPressDown(appButton)))
        {
            if (!isMicOn)
            {
                // Start Arguments: DeviceName, Loop, LengthSec, Frequency.
                // NOTE: We could record in lower frequency to decrease the latencies. For now let's stick to 44100 Hz.
                clip = Microphone.Start(null, true, 10, FREQUENCY);
                finishOffset = -1;
                isMicOn = true;

                StartCoroutine(ShowMessage("Started recording", 2));
            }
            else
            {
                finishOffset = Microphone.GetPosition(null);
                Microphone.End(null);
                isMicOn = false;

                StartCoroutine(ShowMessage("Ended recording", 2));
            }
        }
    }

    void FixedUpdate()
    {
        if (spatialSounds != null)
        {
            int offset = isMicOn ? Microphone.GetPosition(null) : finishOffset;

            // We need to calculate diff of last seen samples and current microphone position. That allows us to detect how many `samples` player's microphone fetched.
            int diff = offset - lastSampleOffset;

            if (diff < 0)
            {
                // If diff < 0, than it means that lastSampleOffset overflew the microphone buffer.
                diff = (clip.samples - lastSampleOffset) + offset;
            }

            if (diff > 0)
            {
                float[] newSamples = new float[diff * clip.channels];
                
                // Get data from Microphone buffer.
                clip.GetData(newSamples, lastSampleOffset);
                
                // Send samples.
                spatialSounds.Update.TriggerSamplesEvent(clip.channels, clip.frequency, new ReadOnlyList<float>(newSamples)).FinishAndSend();
                lastSampleOffset = offset;
            }
        }
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        text = message;
        showNotification = true;
        yield return new WaitForSeconds(delay);
        showNotification = false;
    }

    private string text = "";
    private bool showNotification = false;

    void OnGUI()
    {
        if (showNotification)
            GUI.Label(new Rect(0, 300, 200, 100), text);
    }
}
