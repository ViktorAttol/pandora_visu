using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<Sound> sounds;
    public float fadeDuration;
    public bool debug;

    private Sound current;
    private Sound next;

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

    // Start is called before the first frame update
    void Start()
    {
        current = sounds[0];
        next = sounds[1];
    }

    // Update is called once per frame
    void Update ()
    {
        
    }

    void Next ()
    {

    }

    IEnumerator Play (Sound sound)
    {
        sound.source.Play();
        yield return new WaitForSeconds(sound.clip.length - fadeDuration);
        OnClipDone();
    }

    void OnClipDone ()
    {
        
    }

    
    IEnumerator Crossfade (Sound from, Sound to, float duration)
    {
        if (debug) print("crossfade started.");

        to.source.volume = 0f;
        StartCoroutine(Play(to));

        float remaining = duration;
        bool active = true;

        while (active)
        {
            float t = 1 - remaining / duration; // t : 0 -> 1

            from.source.volume = 1f - t;
            to.source.volume = t;

            remaining -= Time.deltaTime;
            if (remaining <= 0) active = false;
            yield return null;
        }

        to.source.volume = 1f;
        from.source.volume = 0f;
        from.source.Stop();

        if (debug) print("crossfade done.");
    }

    IEnumerator Fade (Sound sound, float start, float end, float duration)
    {
        if (debug) print("fade started.");

        sound.source.volume = start;
        StartCoroutine(Play(sound));

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


