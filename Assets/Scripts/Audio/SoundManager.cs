using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<Sound> sounds;
    public float fadeDuration = 10f;
    public bool debug;
    
    private Sound current;

    void Awake ()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.playOnAwake = false;
            s.source.loop = s.loop;
        }
    }

    public void Play ( string name )
    {
        if (debug) print ("AUDIO: launching: " + name);

        StopAllCoroutines();

        Sound sound = sounds.Find( sound => sound.name == name);
        StartCoroutine(TimedPlay(sound));
        StartCoroutine(Fade(sound, 0f, 1f, fadeDuration));
        if (current != null) StartCoroutine(Fade(current, 1f, 0f, fadeDuration));
        
        current = sound;
    }

    IEnumerator TimedPlay ( Sound sound )
    {
        sound.source.Play();
        yield return new WaitForSeconds(sound.clip.length - fadeDuration);
        OnClipDone();
    }

    void OnClipDone ()
    {
        print ("loop started.");
        Sound sound = sounds.Find( sound => sound.name == current.name + "_loop");
        sound.source.Play();
        StartCoroutine(Fade(sound, 0f, 1f, fadeDuration));
        StartCoroutine(Fade(current, 1f, 0f, fadeDuration));

        current = sound;
    }

    IEnumerator Fade (Sound sound, float start, float end, float duration)
    {
        if (debug) print("fade started.");

        sound.source.volume = start;

        float remaining = duration;
        bool active = true;

        while (active)
        {
            float t = 1 - remaining / duration; // t : 0 -> 1
            float v = Mathf.Lerp(start, end, t);

            sound.source.volume = v;

            remaining -= Time.deltaTime;
            if (remaining <= 0) active = false;
            yield return null;
        }

        sound.source.volume = end;

        if (debug) print("fade done.");
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;
    [Range (0f, 1f)] public float volume;
    public bool loop;
}

