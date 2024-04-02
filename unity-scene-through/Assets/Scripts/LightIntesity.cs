using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntesity : MonoBehaviour
{
    public Light myLight;
    [SerializeField] float remainingTime;
    float startTime;
    bool halftime = true;
    bool quartertime = true;
    bool finaltime = true;

    //Start up is called once at beginning
    void Start()
    {
        startTime = remainingTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        if (remainingTime < 0)
        {
            remainingTime = 0;
            myLight.intensity = 0.0f;
            //GameOver();

        }

        //Reduce Light strengh by half at half the time passing.
        if (remainingTime < (startTime * 0.5f) && halftime)
        {
            myLight.intensity = myLight.intensity * 0.5f;
            halftime = false;
        }

        //Reduce time by a fourth of current at quarter time left.
        if(remainingTime < (startTime * 0.25f) && quartertime)
        {
            //this can easily be edited by either changing the float multiplier or
            //setting a variable at start that remembers the orignal value of light intensity.
            myLight.intensity = myLight.intensity * 0.25f;
            quartertime = false;
        }

        //Reduce time by a tenth of current at tenth of time left.
        if (remainingTime < (startTime * 0.1f) && finaltime)
        {
            myLight.intensity = myLight.intensity * 0.1f;
            finaltime = false;
        }
    }
}
