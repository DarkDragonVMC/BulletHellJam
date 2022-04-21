using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    public static Slider musicSlider;
    public static Slider soundSlider;

    [Range(0f,1f)]
    public float globalVolume;

    public float musicVolume;

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

            if (s.name == "Music") s.source.volume = s.volume * musicVolume * globalVolume;
        }
    }

    public void updateGlobalVolume(float value)
    {
        AudioSource[] sources = this.GetComponents<AudioSource>();

        foreach(AudioSource a in sources)
        {
            if (value == 0)
            {
                a.volume = 0;
            } else if(globalVolume == 0)
            {
                if (a.clip.name == "Music") a.volume = value * musicVolume;
                else a.volume = value;
            }
            else a.volume = a.volume / globalVolume * value;
        }

        globalVolume = value;
    }

    
    void Start()
    {
        Play("Music");
    }
    private void Update()
    {

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Play();
        else Debug.LogWarning("Sound with name " + name + " not found!");
    }
    public void OnMusicChanged(float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == "Music");
        if (value == 0)
        {
            s.source.volume = 0;
            musicVolume = value;
            return;
        }
        if(musicVolume == 0)
        {
            s.source.volume = s.volume * value * globalVolume;
            musicVolume = value;
            return;
        }
        s.source.volume = s.source.volume / musicVolume * value;
        musicVolume = value;

    }
}
