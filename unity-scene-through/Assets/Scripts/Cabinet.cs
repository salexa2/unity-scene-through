using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    public bool isNotLock = false;
    [SerializeField] protected Vector3 openDirection = new Vector3(-1.0f, 0.0f, 0.0f);
    [SerializeField] protected float howFarOpen = 0;
    [SerializeField] protected float speedCabinet = 0.1f;
    public AudioSource audio;
    private bool stopClip = false;

    private Vector3 currentposition;
    private float current = 0;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        currentposition = transform.position;
        isNotLock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNotLock)
        {
            if (current < howFarOpen)
            {
                current += (speedCabinet * Time.deltaTime);
                transform.position = currentposition + (openDirection * current);
            }
            else
            {
                current = howFarOpen;
                transform.position = currentposition + (openDirection * current);
            }
            if(stopClip == false)
            {
                audio.Play();
                stopClip = true;
            }
        }
        else
        {
            if(current > 0)
            {
                current -= (speedCabinet * Time.deltaTime);
                transform.position = currentposition + (openDirection * current);
            }
            else
            {
                current = 0;
                transform.position = currentposition + (openDirection * current);
            }
        }

    }
}
