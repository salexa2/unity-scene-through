using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject objectInteraction = null;
    public KeyCode quitKey = KeyCode.Q;
    public KeyCode interactionKey = KeyCode.E;
    public bool deathFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
        objectInteraction = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject != null)
        {
            Debug.Log("Object:" + other.gameObject.name);
            if (Input.GetKeyDown(interactionKey))
            {
                if (other.gameObject.tag.Equals("Mirror")) //Interaction with mirror
                {
                    Debug.Log("Mirror is control");
                    interactionMirror(other.gameObject);
                    return;
                }
            }
            else if(Input.GetKeyDown(quitKey))
            {
                Debug.Log("Stop interaction");
                stopInteraction();
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
        else
        {
            objectInteraction = null;
        }
    }
    private void stopInteraction()
    {
        if(objectInteraction == null)
        {
            return;
        }
        if (objectInteraction.GetComponent<Mirror>() != null)
        {
            objectInteraction.GetComponent<Mirror>().enabled = false;
            objectInteraction = null;
            this.gameObject.GetComponent<PlayerMovement>().enabled = true;
        }
    }
}
