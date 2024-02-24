using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cullingMask : MonoBehaviour
{
   private Camera camera;

    void Start()
    {
       camera = GetComponent<Camera>();
    }

    // This method is called when another collider enters the trigger
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "wall_coll")
        {
            Debug.Log("Collision started.");
            camera.cullingMask &= ~(1 << LayerMask.NameToLayer("invisbleLayer"));
        }
        
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name != "wall_coll")
        {
            Debug.Log("Collision exited.");
            camera.cullingMask |= 1 << LayerMask.NameToLayer("invisbleLayer");
        }
            
    }

}


