using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

   public void Play() //Play Button Script
    {
        //SceneManager.LoadScene(1); //To load Scene by Index within the Build Settings.
        //SceneManager.LoadScene("NEWLEVEL1SCENE"); //To load Scene by the Scene's Actual Name
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Loads the scene by adding from the zero index of the build settings.
    }

    public void Quit() //Quit Button Script
    {
        Application.Quit();
        Debug.Log("Player has Quit the Game!");
    }
}
