using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreenManager : MonoBehaviour
{

    public static LoadingScreenManager Instance;

   [ Header("bar")]
    public Slider ProgressBar;
    [Header("loading screen")]
    public GameObject loading_screen;
    [Header("Menu")]
    public GameObject mainmenu;
    [Header("cam")]
    public GameObject cam;
    [Header("maincam")]
    public GameObject mcam;


    // Start is called before the first frame update

    private void Awake()
    {

        if (Instance!= null && Instance!= this)
        {
        //    Destroy(this.gameObject);

        }
        else
        {

            Instance = this;
         //   DontDestroyOnLoad(this.gameObject);
        }

             
    }


    public void SwitchToScene(int id)
    { 
        loading_screen.SetActive(true);
        if(mainmenu != null)
        {
            mainmenu.SetActive(false);

        }
       
        cam.SetActive(true);
        mcam.SetActive(false);
        ProgressBar.value = 0;
        StartCoroutine(SwitchToSceneAsync(id));
    }
    void Start()
    {
        
    }

    

    IEnumerator SwitchToSceneAsync(int id)
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);
        while (!asyncLoad.isDone)
        {

            ProgressBar.value = asyncLoad.progress;
            yield return null;

        }
        yield return new WaitForSeconds(3f);
        loading_screen.SetActive(false);
        cam.SetActive(false);

    }
}
