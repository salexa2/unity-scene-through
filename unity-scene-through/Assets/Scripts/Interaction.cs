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
    // Start is called before the first frame update
    protected bool youDie = false;

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
        if(sideCam != null)
        {
            sideCam.gameObject.SetActive(true);
        }
        
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

        }
    }
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(quitKey))
        {
            Debug.Log("Stop interaction");
            stopInteraction();
        }*/

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (deathFlag && collision.gameObject.tag.Equals("Death"))
        {
            Debug.Log("You Die: you restart");
            if (youDie)
            {
                SceneManager.LoadScene("NEWLEVEL1SCENE");
            }
        }

        /*if (collision.gameObject.tag.Equals("Rideable"))
        {
           objectInteraction = collision.gameObject;
            this.gameObject.transform.position = collision.contacts[0].point;
        }*/
        if (collision.gameObject.tag.Equals("Floatable"))
        {
            this.transform.parent = collision.transform;
            this.gameObject.GetComponent<PlayerMovement>().orientation = this.transform.parent;
            Ray ray = new Ray(this.gameObject.transform.position, (-1) * this.gameObject.transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.distance <= 9 && hasJump)
                {
                    Debug.Log("Is land");
                    hasJump = false;
                }
                else
                {
                    Debug.Log("Is in air");
                    hasJump = true;
                }
            }

        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Floatable"))
        {
            this.transform.parent = collision.transform;
            this.gameObject.GetComponent<PlayerMovement>().orientation = this.transform.parent;
            Ray ray = new Ray(this.gameObject.transform.position, (-1) * this.gameObject.transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                if (hit.distance <= 9 && hasJump)
                {
                    Debug.Log("Is land");
                    hasJump = false;
                }
                else
                {
                    Debug.Log("Is in air");
                    hasJump = true;
                }
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
