using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
   // Reference to the VideoPlayer component
  
   public VideoPlayer finalCutscenePlayer;

    // Start is called before the first frame update
    void Start()
    {
        finalCutscenePlayer.loopPointReached += OnFinalCutsceneEnd;
    

    }
      

    // Triggered when the final cutscene ends
    void OnFinalCutsceneEnd(VideoPlayer vp)
    {
        // Unsubscribe from the event to prevent multiple calls
        finalCutscenePlayer.loopPointReached -= OnFinalCutsceneEnd;

        // Load the main menu scene
        LoadingScreenManager.Instance.SwitchToScene(1);
    }
}
