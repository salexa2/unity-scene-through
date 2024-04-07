using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraTarget : MonoBehaviour
{

    public Collider playerCollider;
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Hello");
        IsVisible(cam);
    }

    private bool IsVisible(Camera c)
    {
        //Debug.Log("Hello");
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var objCollider = playerCollider.GetComponent<Collider>();

        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Debug.Log("I am inside the camera frustrum!");
            return true;
        }
        else
        {
            //Debug.Log("I am out of sight...");
            return false;
        }
    }
}
