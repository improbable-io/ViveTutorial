using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using UnityEngine;
using System.Collections;
using Improbable.Util.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioSender: MonoBehaviour
{
    [Require]
    protected SoundsWriter spatialSounds;

    private AudioSource src;
    private int lastSampleOffset;

    void Start()
    {
        src = GetComponent<AudioSource>();
        lastSampleOffset = 0;
    }

    void FixedUpdate()
    {
        if (spatialSounds != null)
        {
            int realTimeSamples = (int) (Time.deltaTime * src.clip.frequency);
            Debug.Log(realTimeSamples + " " + Time.deltaTime);
            float[] newSamples = new float[realTimeSamples * src.clip.channels];
            src.clip.GetData(newSamples, lastSampleOffset);
            spatialSounds.Update.TriggerSamplesEvent(src.clip.channels, src.clip.frequency, new ReadOnlyList<float>(newSamples)).FinishAndSend();
            lastSampleOffset += realTimeSamples;
        }
    }
}
