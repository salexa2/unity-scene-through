using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSetScript : MonoBehaviour
{

    [SerializeField] ClockTimer timer1;
    [SerializeField] int seconds;

    // Start is called before the first frame update
    void Start()
    {
        timer1.SetDuration(seconds).Begin();
    }

    
}
