using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AllPurposeMirror : MonoBehaviour
{
    [Header("Mirror Properties")]
    public GameObject[] rayNum = new GameObject[0]; // Plug-in starting position for multiple rays when reflect off.
    public bool isThereMultipleRay = false; //Boolean to check if the mirror will cast multiple rays.
    public GameObject mirrorWheel = null; //Mirror wheel for turning the wheel if any.
    public GameObject mirrorFrame = null; //Mirror frame that hold the mirror if any.
    
    public Vector3[] directions;
    public KeyCode[] buttons;


    //Speed of the moving the mirror.
    public float mirrorSpeed = 0.01f; 
    public float currentAngle = 0;
    public float maxSpeed = 0.6f;
    public float acceleration = 0.05f;
    public float decceleration = 0.01f;

    public Camera topCam = null;


    // Start is called before the first frame update
    void Start()
    {

    }
    private void Update()
    {

        /*
         * Left Arrow to rotate the frame the hold the mirror to the left
         * and accerelation speed. PS. Modified if needed. 
         */
        bool isMove = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (Input.GetKey(buttons[i]))
            {
                mirrorSpeed += acceleration;
                if (mirrorSpeed > maxSpeed)
                {
                    mirrorSpeed = maxSpeed;
                }

                if (mirrorFrame != null)
                {
                    mirrorFrame.transform.Rotate(directions[i], mirrorSpeed);
                }
                else
                {
                    this.gameObject.transform.Rotate(directions[i], mirrorSpeed);
                }
                if (mirrorWheel != null)
                {
                    mirrorWheel.transform.Rotate(directions[i], -1 * mirrorSpeed);
                }
            }
        }
        if (!isMove)
        {
            /*Decceleration the speed*/
            mirrorSpeed -= decceleration;
            if (mirrorSpeed < 0)
            {
                mirrorSpeed = 0;
            }
        }
    }

    //Method for creating more ray with the 
    public GameObject[] NewCast(GameObject obj)
    {
        //Check if the mirror create more ray.
        if (!isThereMultipleRay) { return null; }
        if(rayNum.Length == 0) { return null; }
        int index_size = 0;
        //Destory the orignal ray list in power script/
        for(int i = 0; i < obj.GetComponent<Power>().raycreate.Length; i++)
        {
            Destroy(obj.GetComponent<Power>().raycreate[i]);
            obj.GetComponent<Power>().raycreate[i] = null;
        }

        for (int i = 0; i < rayNum.Length; i++)
        {
            if (rayNum[i] != null)
            {
                index_size ++;
            }
        }
        //Create a need ray list for the power script.
        GameObject[] ret = new GameObject[index_size];
        int j = 0;
        //Make a copy of the rays and add to the ray list.
        for (int i = 0; i < rayNum.Length; i++)
        {
            if (rayNum[i] != null && j < index_size)
            {
                GameObject temp = Instantiate(obj) as GameObject;
                if(temp.GetComponent<Power>() != null)
                {
                    //Set the new ray properites.
                    temp.GetComponent<Power>().start_position = rayNum[i];
                    temp.GetComponent<Power>().direciton = transform.forward * Mathf.Cos(rayNum[i].transform.rotation.y);
                    temp.GetComponent<Power>().direciton = transform.right * Mathf.Sin(rayNum[i].transform.rotation.y);
                    temp.GetComponent<Power>().line_renderer.positionCount = 2;
                    temp.GetComponent<Power>().bounces = 0;
                    temp.GetComponent<Power>().line_renderer.SetPosition(0, temp.GetComponent<Power>().start_position.transform.position);
                    temp.GetComponent<Power>().castRay(rayNum[i].transform.position, temp.GetComponent<Power>().direciton);
                }
                ret[j] = temp;
                j++;
            }
        }
        return ret;
    }
}