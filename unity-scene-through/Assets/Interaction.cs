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

    public CinemachineVirtualCamera cam = null;
    public float x_position = 0;
    public float x_factor = 1;
    void Start()
    {
        x_position = gameObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(quitKey))
        {
            Debug.Log("Stop interaction");
            stopInteraction();
        }
        if(cam != null)
        {
            float new_x = x_position - this.gameObject.transform.position.x;
            if (new_x != 0)
            {
                new_x = new_x + x_factor* ((x_position - this.gameObject.transform.position.x) / x_position);
            }
            if (cam.GetCinemachineComponent<CinemachineTrackedDolly>() != null)
            {
                cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset.x = new_x;
            }
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
        if(objectInteraction == null)
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
