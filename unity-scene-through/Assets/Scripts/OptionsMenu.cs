using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
    }

    public void setVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        Debug.Log("Volume Working!");
    }

    public void setQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
