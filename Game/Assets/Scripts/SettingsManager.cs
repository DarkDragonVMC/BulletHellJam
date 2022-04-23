using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsManager : MonoBehaviour
{

    public Slider musicSlider;
    public Slider soundSlider;
    public Slider brightnessSlider;
    private AudioManager am;
    private Volume pp;

    private float brightness = 0f;

    private bool settingsLoaded;

    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        pp = GameObject.Find("Global Volume (Effects)").GetComponent<Volume>();

        //check if there is a safed value
        if(PlayerPrefs.HasKey("globalVolume")) am.updateGlobalVolume(PlayerPrefs.GetFloat("globalVolume"));
        musicSlider.value = am.musicVolume;

        if (PlayerPrefs.HasKey("musicVolume")) am.OnMusicChanged(PlayerPrefs.GetFloat("musicVolume"));
        soundSlider.value = am.globalVolume;

        if (PlayerPrefs.HasKey("brightness")) brightness = PlayerPrefs.GetFloat("brightness");
        brightnessSlider.value = brightness;
    }

    public static void loadSettings(Volume pp)
    {
        //change brightsness
        if(PlayerPrefs.HasKey("brightness"))
        {
            ColorAdjustments b = null;
            pp.profile.TryGet<ColorAdjustments>(out b);
            if (b == null) return;
            b.postExposure.Override(PlayerPrefs.GetFloat("brightness"));
        }

        //change global volume
        if(PlayerPrefs.HasKey("globalVolume")) FindObjectOfType<AudioManager>().updateGlobalVolume(PlayerPrefs.GetFloat("globalVolume"));

        //change music volum,e
        if (PlayerPrefs.HasKey("musicVolume")) FindObjectOfType<AudioManager>().OnMusicChanged(PlayerPrefs.GetFloat("musicVolume"));
    }

    public void changeGlobalVolume()
    {
        am.updateGlobalVolume(soundSlider.value);
        PlayerPrefs.SetFloat("globalVolume", soundSlider.value);
    }

    public void changeMusicVolume()
    {
        am.OnMusicChanged(musicSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void changeBrightness()
    {
        ColorAdjustments b = null;
        pp.profile.TryGet<ColorAdjustments>(out b);
        if (b == null) return;
        brightness = brightnessSlider.value;
        b.postExposure.Override(brightness);
        PlayerPrefs.SetFloat("brightness", brightness);
    }
}
