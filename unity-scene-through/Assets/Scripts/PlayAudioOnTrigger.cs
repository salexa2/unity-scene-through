using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{

    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !audio.isPlaying && !audio.mute)
        {
            audio.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && audio.isPlaying)
        {
            if(transform.name != "Forklift")
            {
                audio.Pause();
            }
            if(transform.name == "Forklift")
            {
                audio.mute = true;
            }
            if(transform.name == "Key Collider Sound")
            {
                audio.mute = true;
                gameObject.SetActive(false);
            }
        }
    }
}
