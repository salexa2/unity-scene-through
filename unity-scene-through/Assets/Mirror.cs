using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public GameObject[] rayNum = new GameObject[0]; // Plug-in starting position.
    public bool isThereMultipleRay = false;
    public GameObject mirrorWheel = null;
    public GameObject mirrorFrame = null;
    public float mirrorSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            
            if (mirrorFrame != null)
            {
                mirrorFrame.transform.Rotate(Vector3.up, mirrorSpeed);
            }
            else
            {
                Debug.Log("It working left arrow");
                this.gameObject.transform.Rotate(Vector3.up, mirrorSpeed);
            }
            if (mirrorWheel != null)
            {
                mirrorWheel.transform.Rotate(Vector3.up, -1 * mirrorSpeed);
            }
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            
            if (mirrorFrame != null)
            {
                mirrorFrame.transform.Rotate(Vector3.up, -1 * mirrorSpeed);
            }
            else
            {
                this.gameObject.transform.Rotate(Vector3.up, -1 * mirrorSpeed);
            }
            if (mirrorWheel != null)
            {
                mirrorWheel.transform.Rotate(Vector3.up, mirrorSpeed);
            }
        }
    }

    public GameObject[] NewCast(GameObject obj)
    {
        if (!isThereMultipleRay) { return null; }
        if(rayNum.Length == 0) { return null; }
        int index_size = 0;
        Debug.Log("Problem 1");
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
        Debug.Log("Problem 2");
        GameObject[] ret = new GameObject[index_size];
        int j = 0;
        for (int i = 0; i < rayNum.Length; i++)
        {
            if (rayNum[i] != null && j < index_size)
            {
                GameObject temp = Instantiate(obj) as GameObject;
                if(temp.GetComponent<Power>() != null)
                {
                    Debug.Log("Problem with loop");
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