using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{

    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !audio.isPlaying)
        {
            audio.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && audio.isPlaying)
        {
            audio.Pause();
        }
    }
}
