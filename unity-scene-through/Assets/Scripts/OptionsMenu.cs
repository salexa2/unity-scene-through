using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    //public Dropdown resolutionDropdown;
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        //gather all the resolutions possible for the PC we are currently on.
        resolutions = Screen.resolutions;


        resolutionDropdown.ClearOptions();

        //List<string> options = new List<string>();
        TMP_Dropdown.OptionDataList options = new TMP_Dropdown.OptionDataList();

        // Loop through each resolution and add it to the options list
        /*for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);
        }*/



        foreach (Resolution resolution in resolutions)
        {
            string optionText = $"{resolution.width} x {resolution.height}";
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(optionText);
            options.options.Add(option);
        }

        //resolutionDropdown.AddOptions(options);
        // Assign the list of resolution options to the dropdown
        resolutionDropdown.options = options.options;

        // Find the index of the current screen resolution in the resolutions array
        int currentIndex = GetCurrentResolutionIndex(resolutions);

        // Set the dropdown value to the index of the current resolution
        resolutionDropdown.value = currentIndex;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    int GetCurrentResolutionIndex(Resolution[] resolutions)
    {
        // Get the current screen resolution
        Resolution currentResolution = Screen.currentResolution;

        // Find the index of the current resolution in the resolutions array
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == currentResolution.width &&
                resolutions[i].height == currentResolution.height)
            {
                return i;
            }
        }

        // If the current resolution is not found, return -1 or handle the case accordingly
        return -1;
    }
}
