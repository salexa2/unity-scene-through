using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator TestElevator;
    public bool t_elevator = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Period))
        {
            t_elevator = !t_elevator;
        }
        if (t_elevator)
        {
            TestElevator.Play("Door Open");
        }
        else
        {
            TestElevator.Play("Door_Idle");
        }
    }
}
