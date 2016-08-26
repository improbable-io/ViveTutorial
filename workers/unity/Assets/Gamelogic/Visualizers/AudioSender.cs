using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using UnityEngine;
using System.Collections;
using Improbable.Util.Collections;

public class AudioSender: MonoBehaviour
{
    [Require]
    protected SoundsWriter spatialSounds;

    public AudioClip clip;
    private int lastSampleOffset;

    void Start()
    {
        lastSampleOffset = 0;
    }

    void FixedUpdate()
    {
        if (spatialSounds != null)
        {
            int realTimeSamples = (int) (Time.deltaTime * clip.frequency);
            Debug.Log(realTimeSamples + " " + Time.deltaTime);
            float[] newSamples = new float[realTimeSamples * clip.channels];
            clip.GetData(newSamples, lastSampleOffset);

            spatialSounds.Update.TriggerSamplesEvent(clip.channels, clip.frequency, new ReadOnlyList<float>(newSamples)).FinishAndSend();
            lastSampleOffset += realTimeSamples;

            if (lastSampleOffset >= clip.samples)
            {
                lastSampleOffset = 0;
            }
        }
    }
}
