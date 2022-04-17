using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    [Range(0f,1f)]
    public float globalVolume;

    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null) instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * globalVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void updateGlobalVolume(float newGlobalVolume)
    {
        AudioSource[] sources = this.GetComponents<AudioSource>();

        foreach(AudioSource a in sources)
        {
            a.volume = a.volume / globalVolume * newGlobalVolume;
        }

        globalVolume = newGlobalVolume;
    }

    void Start()
    {
        Play("Music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Play();
        else Debug.LogWarning("Sound with name " + name + " not found!");
    }

}
