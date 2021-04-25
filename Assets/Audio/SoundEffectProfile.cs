using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SoundEffectProfile", menuName = "Sound Effect Profile")]
public class SoundEffectProfile : ScriptableObject
{
    public AudioClip[] audioClips;
    public float volume = 1;
    public Vector2 pitchRange = Vector2.one;
    public static AudioManager am;

    public void Play()
    {
        Play(am.SFXSource, pitchRange.x, pitchRange.y);
    }

    public void Play(float pitch)
    {
        Play(am.SFXSource, pitch, pitch);
    }

    public void Play(AudioSource source)
    {
        Play(source, pitchRange.x, pitchRange.y);
    }

    public void Play(AudioSource source, float minPitch, float maxPitch)
    {
        source.pitch = Random.Range(minPitch, maxPitch);
        source.volume = volume;

        if (am != null)
            source.volume *= am.getScaleSFXVolume();

        source.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }


}
