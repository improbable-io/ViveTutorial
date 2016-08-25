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
    private const int FREQUENCY = 44100;

    void Start()
    {
        src = GetComponent<AudioSource>();
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
        AudioClip received = AudioClip.Create("SpatialRecording", samplesArray.Length, 1, FREQUENCY, false);
        received.SetData(samplesArray, 0);
        src.PlayOneShot(received);
    }
}
