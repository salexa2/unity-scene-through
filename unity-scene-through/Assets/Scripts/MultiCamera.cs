using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCamera : MonoBehaviour
{
    //original dolly camera
    public GameObject startCam;
    //camera we will be swapping to
    public GameObject newCam;

    void OnTriggerStay(Collider other)
    {
        Debug.Log("I'm inside the collider!");
        startCam.SetActive(false);
        newCam.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        newCam.SetActive(false);
        startCam.SetActive(true);
    }
}
