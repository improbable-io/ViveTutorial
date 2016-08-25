using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[EngineType(Improbable.Unity.EnginePlatform.Client)]
public class AudioReceiver : MonoBehaviour
{
    [Require]
    protected SoundsReader spatialSounds;

    private AudioSource src;
    private int lastSampleOffset;

    void Start()
    {
        src = GetComponent<AudioSource>();
        lastSampleOffset = 0;
    }

    void OnEnable()
    {
        spatialSounds.SamplesEvent += ReceiveSoundSamples;
    }

    void OnDisable()
    {
        spatialSounds.SamplesEvent -= ReceiveSoundSamples;
    }

    void ReceiveSoundSamples(SamplesData sound)
    {
        // We don't want to play sound which we produced.
        if (spatialSounds.IsAuthoritativeHere) { return; }

        if (src.clip == null)
        {
            // Assuming that further events will have the same frequency and number of channels!
            // Let's create a buffer for 100 seconds. If filled, we will overwrite it from beginning.
            src.clip = AudioClip.Create("SpatialSound", 100 * sound.Frequency, sound.Channels, sound.Frequency, false);
        }

        if (!src.isPlaying)
        {
            // If audio source stopped playing because of lack of samples we need to make sure that we are setting samples from the beginning. 
            // Every `src.Play()` starts playing from the beginning!
            lastSampleOffset = 0;

        } 
        else if (lastSampleOffset >= src.clip.samples)
        {
            // If we last time overflow the clip buffer we need to find accurate point, because `SetData` overwrote data from the beginning.
            lastSampleOffset = lastSampleOffset - src.clip.samples;
        }

        // Inject data into clip in run-time (it does not matter if audio is played or not).
        src.clip.SetData(readOnlyListToArray(sound.Samples), lastSampleOffset);

        if (!src.isPlaying)
        {
            // We need to delay playing by 100ms in order to have better quality of sound. That's mitigates the latencies.
            src.PlayDelayed(0.1f);
        }

        lastSampleOffset += sound.Samples.Count;
    }

    float[] readOnlyListToArray(Improbable.Util.Collections.IReadOnlyList<float> readOnlyList)
    {
        float[] array = new float[readOnlyList.Count];
        readOnlyList.ToList().CopyTo(array, 0);

        return array;
    }
}
