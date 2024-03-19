using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The script for player to interaction with other if they trigger a collider of the object.
/// </summary>
public class Interaction : MonoBehaviour
{
    //Interaction Properties
    public GameObject objectInteraction = null; //Store the current object the player is interacting with. 
    public KeyCode quitKey = KeyCode.Q;  //Key for quitting an interaction.
    public KeyCode interactionKey = KeyCode.E; //Key for interaction with object.
    public bool deathFlag = false; // Set if you want to dies.
    //public bool canSeeTopDown = false;

    
    protected bool youDie = true; // Set if you want to more death.

    //Swing Mechanic
    /*public Transform swingpositon = null;
    protected bool isSwing = false;
    public Vector3 vineVelocityWhenGrabbed;
    public float swingForce = 10f;*/

    

    //Camera Properites.
    public CinemachineVirtualCamera sideCam = null;
    public float x_position = 0;
    public float max_x_factor = 5;
    public float x_factor = 0;
    public Camera sideCamera = null;


    // Start is called before the first frame update
    void Start()
    {
        x_position = gameObject.transform.position.x;
        if(sideCam != null)
        {
            sideCam.gameObject.SetActive(true);
            if (sideCam != null)
            {
                sideCamera.gameObject.SetActive(true);
                sideCam.gameObject.SetActive(true);
            }
        }
        
    }

    private void FixedUpdate()
    {

        if (sideCam != null && sideCam.isActiveAndEnabled)
        {
            float new_x = x_position - this.gameObject.transform.position.x;
            Debug.Log("new_x 2 " + new_x);
            if (sideCam.GetCinemachineComponent<CinemachineTrackedDolly>() != null)
            {
                sideCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset.x = new_x;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //You player the quit key to stop interaction between the player and object.
        if (Input.GetKey(quitKey))
        {
            stopInteraction();
        }
        //Swing Mechanic to stop swinging. 
        /*if (isSwing)
        {
            this.transform.position = swingpositon.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSwing = false;
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(swingpositon.GetComponent<Rigidbody>().velocity.x, swingpositon.GetComponent<Rigidbody>().velocity.y + swingForce,
                    swingpositon.GetComponent<Rigidbody>().velocity.z);
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }*/
    }

    /*
     * Enter the Trigger Collider.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Rideable")) //Interaction with Rideable
        {
            this.transform.parent = other.transform;
            this.gameObject.GetComponent<PlayerMovement>().orientation = this.transform.parent;

        }
        /*if (other.gameObject.tag == "Swingable") //Interaction with Swingable
        {
            other.GetComponent<Rigidbody>().velocity = vineVelocityWhenGrabbed;
            isSwing = true;
            swingpositon = other.transform;
        }*/
        if (objectInteraction != null) return;
        if (other.gameObject.tag.Equals("Key"))
        {
            objectInteraction = other.gameObject;
            other.gameObject.SetActive(false);
        }
    }
    /*
     * Exit the Trigger Collider.
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Rideable")) //Interaction with Rideable
        {
            this.transform.parent = null;
        }
        if (other.gameObject.tag.Equals("Mirror")) //Interaction with mirror
        {
            stopInteraction();
        }
    }
    /*
     * Stay Trigger
     */
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject != null)
        {
            if (objectInteraction != null)
            {
                return;
            }
            //Debug.Log("Object:" + other.gameObject.name);
            if (other.gameObject.tag.Equals("Mirror")) //Interaction with mirror
            {
                interactionMirror(other.gameObject);
                return;
            }
            

        }
    }
    //Method for interaction with a mirror.
    private void interactionMirror(GameObject obj)
    {
        //Press the interteraction key to the active mirror script. 
        if (Input.GetKey(interactionKey))
        {
            
            objectInteraction = obj.gameObject;
            if (objectInteraction.GetComponent<Mirror>() != null)
            {
                this.gameObject.GetComponent<PlayerMovement>().enabled = false;
                objectInteraction.GetComponent<Mirror>().enabled = true;
                //obj.gameObject.GetComponent<Mirror>().ChangeDirection();
                Debug.Log("Mirror Click");
                if (obj.GetComponent<Mirror>().topCam != null && sideCam != null && sideCam.gameObject.activeSelf)
                {
                    Debug.Log("Click Click Click");
                    obj.GetComponent<Mirror>().topCam.gameObject.SetActive(true);
                    sideCam.gameObject.SetActive(false);
                    sideCamera.gameObject.SetActive(false);
                }
            }
        }
        
    }

    //Stop all interaction between player and object.
    private void stopInteraction()
    {
        //Give control back thte player movement script.
        if (objectInteraction == null)
        {
            if (!sideCam.gameObject.activeSelf)
            {
                sideCam.gameObject.SetActive(true);
                sideCamera.gameObject.SetActive(true);
            }
            this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            return;
        }
        //Stop mirror interaction.
        if (objectInteraction.GetComponent<Mirror>() != null)
        {
            if (objectInteraction.GetComponent<Mirror>().topCam != null && sideCam != null && !sideCam.gameObject.activeSelf)
            {
                objectInteraction.GetComponent<Mirror>().topCam.gameObject.SetActive(false);
                sideCam.gameObject.SetActive(true);
                sideCamera.gameObject.SetActive(true);
            }
            objectInteraction.GetComponent<Mirror>().enabled = false;
            objectInteraction = null;
            if(objectInteraction == null)
            {
                Debug.Log("Hello Move");
                this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            }
            
            
        }
        //Add more stop interaction code below
        //----------------------------------------------------------
    }
}
