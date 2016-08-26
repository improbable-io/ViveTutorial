using Improbable.Sounds;
using Improbable.Unity.Visualizer;
using UnityEngine;
using System.Collections;
using Improbable.Util.Collections;

[RequireComponent(typeof(AudioSource))]
[EngineType(Improbable.Unity.EnginePlatform.Client)]
public class AudioReceiver : MonoBehaviour
{
    [Require]
    protected SoundsReader spatialSounds;

    private AudioSource src;
    private int lastSampleOffset;
    private const int FREQUENCY = 44100;
    private const int BUFFER_SAMPLES_COUNT = 100 * FREQUENCY;

    void Start()
    {
        src = GetComponent<AudioSource>();
        // Assuming the frequency is always FREQUENCY & only single channel.
        src.clip = AudioClip.Create("SpatialSound", BUFFER_SAMPLES_COUNT, 1, FREQUENCY, false);
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
        if (spatialSounds.IsAuthoritativeHere) { return; }

        float[] samplesArray = new float[sound.Samples.Count];
        sound.Samples.ToList().CopyTo(samplesArray, 0);
        Debug.Log("Received recording with samples: " + samplesArray.Length + " Offset: " + lastSampleOffset + " Currenct samples length: " + src.clip.samples);
        if (lastSampleOffset >= BUFFER_SAMPLES_COUNT)
        {
            lastSampleOffset = lastSampleOffset - BUFFER_SAMPLES_COUNT;
        }

        if(!src.isPlaying) {
            lastSampleOffset = 0;
        }

        src.clip.SetData(samplesArray, lastSampleOffset);

        if (!src.isPlaying)
        {
            src.PlayDelayed(0.1f);
        }

        lastSampleOffset += samplesArray.Length;
    }
}
