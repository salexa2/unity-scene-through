using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    public GameObject objectInteraction = null;
    public KeyCode quitKey = KeyCode.Q;
    public KeyCode interactionKey = KeyCode.E;
    public bool deathFlag = false;
    public bool hasJump = true;
    //public bool canSeeTopDown = false;


    protected bool youDie = true;

    public Transform swingpositon = null;
    protected bool isSwing = false;
    public Vector3 vineVelocityWhenGrabbed;
    public float swingForce = 10f;

    public CinemachineVirtualCamera sideCam = null;
    public float x_position = 0;
    public float max_x_factor = 5;
    public float x_factor = 0;

    public Camera topCam = null;
    public Camera sideCamera = null;


    // Start is called before the first frame update
    void Start()
    {
        x_position = gameObject.transform.position.x;
        if(sideCam != null)
        {
            sideCam.gameObject.SetActive(true);
            if(topCam != null)
            {
                topCam.gameObject.SetActive(false);
            }
            if (sideCam != null)
            {
                sideCamera.gameObject.SetActive(true);
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (topCam != null && sideCam != null && sideCam.gameObject.activeSelf)
            {
                topCam.gameObject.SetActive(true);
                sideCam.gameObject.SetActive(false);
                sideCamera.gameObject.SetActive(false);
            }
            else if (topCam != null && sideCam != null && !sideCam.gameObject.activeSelf)
            {
                topCam.gameObject.SetActive(false);
                sideCam.gameObject.SetActive(true);
                sideCamera.gameObject.SetActive(true);
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
    }
    // Update is called once per frame
    void Update()
    {
        if (isSwing)
        {
            this.transform.position = swingpositon.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSwing = false;
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(swingpositon.GetComponent<Rigidbody>().velocity.x, swingpositon.GetComponent<Rigidbody>().velocity.y + swingForce,
                    swingpositon.GetComponent<Rigidbody>().velocity.z);
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (deathFlag && collision.gameObject.tag.Equals("Death"))
        if (collision.gameObject.name == "water_plane")
        {
            Debug.Log("You Die: you restart");
            if (youDie)
            {
                SceneManager.LoadScene("NEWLEVEL1SCENE");
            }
        }

        
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
        if (other.gameObject.tag == "Swingable")
        {
            other.GetComponent<Rigidbody>().velocity = vineVelocityWhenGrabbed;
            isSwing = true;
            swingpositon = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Rideable"))
        {
            this.transform.parent = null;
        }
        if (other.gameObject.tag.Equals("Mirror"))
        {
            stopInteraction();
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject != null)
        {
            Debug.Log("Object:" + other.gameObject.name);
            if (other.gameObject.tag.Equals("Mirror")) //Interaction with mirror
            {
                interactionMirror(other.gameObject);
                return;
            }

        }
        if (other.gameObject.tag == "Swingable")
        {
            swingpositon = other.transform;
        }
    }
    private void interactionMirror(GameObject obj)
    {
        if (Input.GetKey(interactionKey))
        {
            objectInteraction = obj.gameObject;
            if (objectInteraction.GetComponent<Mirror>() != null)
            {
                //this.gameObject.GetComponent<PlayerMovement>().enabled = false;
                objectInteraction.GetComponent<Mirror>().enabled = true;
                obj.gameObject.GetComponent<Mirror>().ChangeDirection();
            }
        }else if (Input.GetKeyUp(interactionKey))
        {
            if (objectInteraction.GetComponent<Mirror>() != null)
            {
                //this.gameObject.GetComponent<PlayerMovement>().enabled = false;
                objectInteraction.GetComponent<Mirror>().enabled = true;
                if (!objectInteraction.GetComponent<Mirror>().wheelisTurn)
                {
                    obj.gameObject.GetComponent<Mirror>().isTurn = false;
                }
            }
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
            
            objectInteraction.gameObject.GetComponent<Mirror>().isTurn = false;
            objectInteraction.gameObject.GetComponent<Mirror>().wheelisTurn = false;
            objectInteraction.GetComponent<Mirror>().enabled = false;
            objectInteraction = null;
            this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            
        }
    }
}
