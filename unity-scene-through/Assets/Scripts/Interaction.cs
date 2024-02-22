using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject objectInteraction = null;
    public KeyCode quitKey = KeyCode.Q;
    public KeyCode interactionKey = KeyCode.E;
    public bool deathFlag = false;
    // Start is called before the first frame update


    public CinemachineVirtualCamera sideCam = null;
    public float x_position = 0;
    public float max_x_factor = 5;
    public float x_factor = 0;

    public CinemachineVirtualCamera topCam = null;
    public float y_position = 0;
    public float max_y_factor = 5;
    public float y_factor = 1;

    public Transform pivotPoint = null;
    void Start()
    {
        x_position = gameObject.transform.position.x;
        y_position = gameObject.transform.position.y;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (topCam != null && sideCam != null && topCam.isActiveAndEnabled)
            {
                topCam.enabled = false;
                sideCam.enabled = true;
            }
            else if (topCam != null && sideCam != null && sideCam.isActiveAndEnabled)
            {
                sideCam.enabled = false;
                topCam.enabled = true;
            }
        }
        if (sideCam != null && sideCam.isActiveAndEnabled)
        {
            float new_x = x_position - this.gameObject.transform.position.x;
            if (new_x != 0)
            {
                new_x = new_x + x_factor * ((x_position - this.gameObject.transform.position.x) / x_position);
            }
            if (sideCam.GetCinemachineComponent<CinemachineTrackedDolly>() != null)
            {
                sideCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset.x = new_x;
            }
        }
        if (topCam != null && topCam.isActiveAndEnabled)
        {
            Vector3 camTmp = topCam.gameObject.transform.position;
            Vector3 new_position = new Vector3(this.transform.position.x, camTmp.y, camTmp.z);
            topCam.gameObject.transform.position = Vector3.Lerp(camTmp, new_position, 4);
            float new_y = y_position - this.gameObject.transform.position.x;
            if (sideCam.GetCinemachineComponent<CinemachineTrackedDolly>() != null)
            {
                sideCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset.y = new_y;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(quitKey))
        {
            Debug.Log("Stop interaction");
            stopInteraction();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (deathFlag && collision.gameObject.tag.Equals("Death"))
        {
            Debug.Log("You Die");
        }
        /*if (collision.gameObject.tag.Equals("Rideable"))
        {
           objectInteraction = collision.gameObject;
            this.gameObject.transform.position = collision.contacts[0].point;
        }*/
    }
    private void OnCollisionStay(Collision collision)
    {
        /*(objectInteraction.gameObject.tag.Equals("Rideable"))
        {
            objectInteraction = collision.gameObject;
            this.gameObject.transform.position = collision.contacts[0].point;
        }*/
    }
    private void OnCollisionExit(Collision collision)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Rideable"))
        {
            this.transform.parent = other.transform;
            this.gameObject.GetComponent<PlayerMovement>().orientation = this.transform.parent;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Rideable"))
        {
            this.transform.parent = null;
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject != null)
        {
            Debug.Log("Object:" + other.gameObject.name);
            if (Input.GetKey(interactionKey))
            {
                if (other.gameObject.tag.Equals("Mirror")) //Interaction with mirror
                {
                    Debug.Log("Mirror is control");
                    interactionMirror(other.gameObject);
                    return;
                }
            }

        }
    }
    private void interactionMirror(GameObject obj)
    {
        objectInteraction = obj.gameObject;
        if (objectInteraction.GetComponent<Mirror>() != null)
        {
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            objectInteraction.GetComponent<Mirror>().enabled = true;
        }
    }
    private void stopInteraction()
    {
        if (objectInteraction == null)
        {
            this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            return;
        }
        if (objectInteraction.GetComponent<Mirror>() != null)
        {
            objectInteraction.GetComponent<Mirror>().enabled = false;
            this.gameObject.GetComponent<PlayerMovement>().enabled = true;
        }
    }
}
