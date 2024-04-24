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
    public GameObject objectInteractionkey = null; //Store the current object the player is interacting with. 
    public KeyCode quitKey = KeyCode.Q;  //Key for quitting an interaction.
    public KeyCode interactionKey = KeyCode.E; //Key for interaction with object.
    public bool deathFlag = false; // Set if you want to dies.
    //public bool canSeeTopDown = false;

    public GameObject projectionMain; //Projection Object that lights up on completion.
    public GameObject projectionSecondary; //Sometimes there's two!
    public bool twoProjectors = false; //Inside the update call this makes sure to wait for both if set.
    public CinemachineVirtualCamera goalCam;
    public Animator animator; //Animator for each Camera that will be used.
    public bool animationPlayed = false; //Inside the update I need this to make sure the animator is called only once!

    public GameObject UI;

    
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
        if(UI != null)
        {
            UI.SetActive(false);
        }
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

        if(projectionSecondary != null)
        {
            twoProjectors = true;
        }
        if (goalCam != null && goalCam.gameObject.activeSelf)
        {
            goalCam.gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {

        if (sideCam != null && sideCam.isActiveAndEnabled)
        {
            float new_x = x_position - this.gameObject.transform.position.x;
            //Debug.Log("new_x 2 " + new_x);
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

        if (twoProjectors)
        {
            if (projectionMain.activeSelf && projectionSecondary.activeSelf)
            {
                if (animationPlayed == false)
                {
                    stopInteraction();
                    int i = SceneManager.GetActiveScene().buildIndex;
                    Debug.Log(i);
                    animationPlayed = true;
                    StartCoroutine(playAnimation(i));
                }
            }
            else
            {
                animationPlayed = false;
            }
        }
        else if (projectionMain.activeSelf)
        {
            
            if (animationPlayed == false)
            {
                stopInteraction();
                int i = SceneManager.GetActiveScene().buildIndex;
                animationPlayed = true;
                StartCoroutine(playAnimation(i));
                
            }
        }
        else
        {
            animationPlayed = false;
        }
        if(!animationPlayed)
        {
            int i = SceneManager.GetActiveScene().buildIndex;
            if (i == 2)
            {
                animator.Play("GoalCamIble1");
            }
            else if (i == 3)
            {
                animator.Play("GoalCamIble2");
            }
            else if (i == 4)
            {
                animator.Play("GoalCamIble3");
            }
            //The Ible for  animator scene 3
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
     * Called once inside update on level completion.
     */
    private IEnumerator playAnimation(int option)
    {
        bool istrue = false;
        if(sideCam != null && sideCam.gameObject.activeSelf && animationPlayed)
        {
            sideCam.gameObject.SetActive(false);
            goalCam.gameObject.SetActive(true);
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
            istrue = true;

        }
        if (istrue)
        {
            if (option == 2)
            {
                yield return new WaitForSeconds(0.5f);
                animator.Play("GoalCam1");
                yield return new WaitForSeconds(5f);
            }
            else if (option == 3)
            {
                yield return new WaitForSeconds(0.5f);
                animator.Play("GoalCam2");
                yield return new WaitForSeconds(7f);
            }
            else if (option == 4)
            {
                yield return new WaitForSeconds(0.5f);
                animator.Play("GoalCam3");
                yield return new WaitForSeconds(5f);
            }
        }
        if (sideCam != null)
        {
            goalCam.gameObject.SetActive(false);
            sideCam.gameObject.SetActive(true);
            this.gameObject.GetComponent<PlayerMovement>().enabled = true;

        }

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
        if (other.gameObject.tag.Equals("Key"))
        {
            if(objectInteractionkey != null)
            {
                objectInteractionkey.SetActive(true);
                objectInteractionkey = other.gameObject;
                other.gameObject.SetActive(false);
            }
            else
            {
                objectInteractionkey = other.gameObject;
                other.gameObject.SetActive(false);
            }
            
        }
        //if (other.gameObject.tag.Equals("Cabinet") && Input.GetKey(interactionKey))
        //{
        //    other.gameObject.GetComponent<Cabinet>().isNotLock = !other.gameObject.GetComponent<Cabinet>().isNotLock;
        //}
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
            if (other.gameObject.tag.Equals("Cabinet") && Input.GetKeyDown(interactionKey))
            {
                other.gameObject.GetComponent<Cabinet>().isNotLock = true;
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
                    if(UI != null)
                    {
                        this.UI.SetActive(true);
                    }
                    sideCam.gameObject.SetActive(false);
                    sideCamera.gameObject.SetActive(false);
                }
            }
            if (objectInteraction.GetComponent<AllPurposeMirror>() != null)
            {
                this.gameObject.GetComponent<PlayerMovement>().enabled = false;
                objectInteraction.GetComponent<AllPurposeMirror>().enabled = true;
                //obj.gameObject.GetComponent<Mirror>().ChangeDirection();
                Debug.Log("Mirror Click");
                if (obj.GetComponent<AllPurposeMirror>().topCam != null && sideCam != null && sideCam.gameObject.activeSelf)
                {
                    Debug.Log("Click Click Click");
                    obj.GetComponent<AllPurposeMirror>().topCam.gameObject.SetActive(true);
                    if (UI != null)
                    {
                        this.UI.SetActive(true);
                    }
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
            if (UI != null)
            {
                this.UI.SetActive(false);
            }
            return;
        }
        Debug.Log("You press Q");
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
                if (UI != null)
                {
                    this.UI.SetActive(false);
                }
                this.gameObject.GetComponent<PlayerMovement>().enabled = true;
            }
            
            
        }else if (objectInteraction.GetComponent<AllPurposeMirror>() != null)
        {
            if (objectInteraction.GetComponent<AllPurposeMirror>().topCam != null && sideCam != null && !sideCam.gameObject.activeSelf)
            {
                objectInteraction.GetComponent<AllPurposeMirror>().topCam.gameObject.SetActive(false);
                sideCam.gameObject.SetActive(true);
                sideCamera.gameObject.SetActive(true);
            }
            objectInteraction.GetComponent<AllPurposeMirror>().enabled = false;
            if (UI != null)
            {
                this.UI.SetActive(false);
            }
            objectInteraction = null;
            if (objectInteraction == null)
            {
                Debug.Log("Hello Move");
                this.gameObject.GetComponent<PlayerMovement>().enabled = true;
                if (UI != null)
                {
                    this.UI.SetActive(false);
                }
            }


        }
        //Add more stop interaction code below
        //----------------------------------------------------------
    }
}
