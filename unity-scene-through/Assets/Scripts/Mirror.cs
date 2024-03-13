using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [Header("Mirror Properties")]
    public GameObject[] rayNum = new GameObject[0]; // Plug-in starting position for multiple rays when reflect off.
    public bool isThereMultipleRay = false; //Boolean to check if the mirror will cast multiple rays.
    public GameObject mirrorWheel = null; //Mirror wheel for turning the wheel if any.
    public GameObject mirrorFrame = null; //Mirror frame that hold the mirror if any.
    
    public Vector3 direction = Vector3.up;


    //Speed of the moving the mirror.
    public float mirrorSpeed = 0.01f; 
    public float currentAngle = 0;
    public float maxSpeed = 0.6f;
    public float acceleration = 0.05f;
    public float decceleration = 0.01f;

    //Mirror Camera when it being interaction.
    public Camera topCam = null;
    // Direction of previous set angle.
    /*public Quaternion[] mirrorDirection = new Quaternion[0];
    public int mirrorIndex = 0;
    public bool isTurn = false;
    public bool wheelisTurn = false;*/


    // Start is called before the first frame update
    void Start()
    {
        //No Initial at the start.
        /*if(mirrorDirection.Length != 0)
        {
            //this.gameObject.transform.Rotate(transform.up, mirrorDirection[mirrorIndex].y);
            //mirrorFrame.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation,mirrorDirection[mirrorIndex], 3);
            //isTurn = false;
            //wheelisTurn = false;
        }
        if (mirrorFrame != null)
        {
            currentAngle = mirrorFrame.transform.eulerAngles.y;
        }
        else
        {
            currentAngle = this.transform.transform.eulerAngles.y;
        }*/

    }
    private void Update()
    {
        /*Quaternion mirrorAngle = mirrorFrame.transform.rotation;
        if (mirrorFrame != null)
        {
            currentAngle = mirrorFrame.transform.eulerAngles.y;
        }
        else
        {
            currentAngle = this.transform.transform.eulerAngles.y;
        }
        */
        /*
         * Left Arrow to rotate the frame the hold the mirror to the left
         * and accerelation speed. PS. Modified if needed. 
         */
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mirrorSpeed += acceleration;
            if (mirrorSpeed > maxSpeed)
            {
                mirrorSpeed = maxSpeed;
            }

            if (mirrorFrame != null)
            {
                mirrorFrame.transform.Rotate(direction, mirrorSpeed);
            }
            else
            {
                this.gameObject.transform.Rotate(direction, mirrorSpeed);
            }
            if (mirrorWheel != null)
            {
                mirrorWheel.transform.Rotate(direction, -1 * mirrorSpeed);
            }
        }
        /*
         * Right Arrow to rotate the frame the hold the mirror to the Right
         * and accerelation speed. PS. Modified if needed. 
         */
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mirrorSpeed += acceleration;
            if (mirrorSpeed > maxSpeed)
            {
                mirrorSpeed = maxSpeed;
            }
            if (mirrorFrame != null)
            {
                mirrorFrame.transform.Rotate(direction, -1 * mirrorSpeed);
            }
            else
            {
                this.gameObject.transform.Rotate(direction, -1 * mirrorSpeed);
            }
            if (mirrorWheel != null)
            {
                mirrorWheel.transform.Rotate(direction, mirrorSpeed);
            }
        }
        else
        {
            /*Decceleration the speed*/
            mirrorSpeed -= decceleration;
            if(mirrorSpeed < 0)
            {
                mirrorSpeed = 0;
            }
        }
        /*if (mirrorFrame.transform.rotation != mirrorDirection[mirrorIndex])
        {
            mirrorFrame.transform.rotation = mirrorDirection[mirrorIndex];
        }
        else
        {
            wheelisTurn = false;
        }*/
    }
    /*public void ChangeDirection()
    {
        if (mirrorDirection.Length != 0 && !isTurn && !wheelisTurn)
        {
            mirrorIndex++;
            if(mirrorIndex >= mirrorDirection.Length)
            {
                mirrorIndex = 0;
            }
            wheelisTurn = true;
            isTurn = true;
        }
    }*/

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