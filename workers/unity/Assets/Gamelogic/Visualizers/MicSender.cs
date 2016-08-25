using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using UnityEngine;
using System.Collections;
using Improbable.Util.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicSender : MonoBehaviour
{
    [Require]
    protected SoundsWriter spatialSounds;

    private AudioSource src;
    private int lastSampleOffset;
    private int finishtOffset;

    private const int FREQUENCY = 44100;

    void Start()
    {
        src = GetComponent<AudioSource>();
        lastSampleOffset = 0;
        finishtOffset = -1;
    }

    void FixedUpdate()
    {
        if (spatialSounds != null)
        {
            int offset = finishtOffset == -1 ? Microphone.GetPosition(null) : finishtOffset;
            int diff = offset - lastSampleOffset;

            if (diff < 0)
            {
                diff = (src.clip.samples - lastSampleOffset) + offset;
            }

            if (diff > 0)
            {
                float[] newSamples = new float[diff * src.clip.channels];
                src.clip.GetData(newSamples, lastSampleOffset);
                spatialSounds.Update.TriggerSamplesEvent(src.clip.channels, src.clip.frequency, new ReadOnlyList<float>(newSamples)).FinishAndSend();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // Start Arguments: DeviceName, Loop, LengthSec, Frequency.
            src.clip = Microphone.Start(null, true, 10, FREQUENCY);
            finishtOffset = -1;

            StartCoroutine(ShowMessage("Started Recording", 2));
        }

        if (Input.GetKeyUp(KeyCode.F2))
        {
            finishtOffset = Microphone.GetPosition(null);
            Microphone.End(null);

            StartCoroutine(ShowMessage("Ended Recording", 2));
        }
    }
}
