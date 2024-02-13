using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Animator door_aniamtor;
    public bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            door_aniamtor.Play("Door_Idle");
        }
    }
    public void CompleteGoal()
    {
        door_aniamtor.Play("Door Open");
    }
}
