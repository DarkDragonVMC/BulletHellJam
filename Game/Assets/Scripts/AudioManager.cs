using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    public Slider musicSlider;
    public Slider soundSlider;

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

    public void updateGlobalVolume()
    {
        AudioSource[] sources = this.GetComponents<AudioSource>();

        foreach(AudioSource a in sources)
        {
            if (soundSlider.value == 0)
            {
                a.volume = 0;
                break;
            }
            if(globalVolume == 0)
            {
                a.volume = soundSlider.value;
                break;
            }
            a.volume = a.volume / globalVolume * soundSlider.value;
        }

        globalVolume = soundSlider.value;
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
    public void OnMusicChanged()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "Music");
        if (musicSlider.value == 0)
        {
            s.source.volume = 0;
            
        }
        if(musicVolume == 0)
        {
            s.source.volume = s.volume * musicSlider.value;
            return;
        }
        s.source.volume = s.source.volume / musicVolume * musicSlider.value;
        musicVolume = musicSlider.value;

    }


}
