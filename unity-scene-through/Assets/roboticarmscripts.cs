using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class roboticarmscripts : MonoBehaviour
{
    
    private Animator animator;
    private float timer = 0.0f;
    // Start is called before the first frame update
    private bool flag = true; //isdown is false; 

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Check if it's time to move the arm down
        if (timer >=3.0f)
        {
            Debug.Log("[roboticarmscript.c]ARM SHOULD MOVE\n ");
            MoveArmDown();
            timer = 0.0f;
        }
    }

    void MoveArmDown()
    {
        

        if(flag == true)
        {
            animator.SetBool("isDown", flag);
            flag = false;
        }
        else
        {

            animator.SetBool("isDown", flag);
                flag = true; 
        }
       
    }
}
