using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    /*
     * AUDIO MANAGER
     * 
     * Unlike Music Theme, it's not needed for him to be a singleton
     * Furthermore, simplify sounds interfacing with unity's scene (ie buttons, events trigger etc...)
     */

    public Sound[] sounds;


    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.aSource = gameObject.AddComponent<AudioSource>();
            s.aSource.clip = s.clip;
            s.aSource.volume = s.volume;
            s.aSource.pitch = s.pitch;
            s.aSource.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.aSource.PlayOneShot(s.clip);
    }

    public void Play(string name, float randomRangeMin, float randomRangeMax)
    {
        //Making sure pitch range can not overflow
        if (randomRangeMin < 0.1f) randomRangeMin = .1f;
        if (randomRangeMax < 3f) randomRangeMin = 3f;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.aSource.pitch= UnityEngine.Random.Range(randomRangeMin, randomRangeMax);
        s.aSource.PlayOneShot(s.clip);
    }

    public void Stop(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.aSource.Stop();
    }
}
