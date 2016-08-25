using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Util.Collections;

public class AudioSender: MonoBehaviour
{
    [Require]
    protected SoundsWriter spatialSounds;
    
    // Get clip from parameter.
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
            // Calculate how much time passed from last update. Translate that to number of samples using clip frequency.
            int samplesToSend = (int) (Time.deltaTime * clip.frequency);

            float[] newSamples = new float[samplesToSend * clip.channels];
            // Copy samplesToSend amount of samples, starting from last used offset.
            clip.GetData(newSamples, lastSampleOffset);

            // Send samples as SpatialOS sound state event.
            spatialSounds.Update.TriggerSamplesEvent(clip.channels, clip.frequency, new ReadOnlyList<float>(newSamples)).FinishAndSend();
            lastSampleOffset += samplesToSend;

            // Our radio really loves that clip! Let's loop it.
            if (lastSampleOffset >= clip.samples)
            {
                lastSampleOffset = 0;
            }
        }
    }
}
