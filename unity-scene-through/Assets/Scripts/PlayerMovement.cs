using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public MovementState state;

    public enum MovementState
    {
        freeze,
        unlimited
    }

    public bool freeze;
    public bool unlimited;

    public bool restricted;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public AudioSource audio;
    bool readyToJump;
    private float seconds = 0;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.Tab;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public CapsuleCollider characterCollider;
    public BoxCollider boxCollide;
    private bool wasCrouched = false;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    
    Vector3 moveDirection;

    Rigidbody rb;

    //reference to animator
    private Animator animator;
    public Animator door_aniamtor;

    //public float speed;
    public float rotationSpeed;

    Vector3 cameraForward;
    Vector3 cameraRight;

    [SerializeField] private List<Scene> _sceneList;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;


        Vector3 cameraForward = new Vector3(); 
        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();
        // Get the CapsuleCollider component attached to the player
        characterCollider = GetComponent<CapsuleCollider>();
        boxCollide = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;


    }

    void FixedUpdate()
    {
       
        MovePlayer();
        SpeedControl();
    }

    private void MyInput()

    {



        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            animator.SetBool("isJumping", true);
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }


        bool isCrouchedNow = Input.GetKey(crouchKey);
        if (isCrouchedNow && !wasCrouched)
        {
            animator.SetBool("isCrouched", true);
            Crouch();
        }
        else if (!isCrouchedNow && wasCrouched)
        {
            animator.SetBool("isCrouched", false);
            StandUp();
        }

        wasCrouched = isCrouchedNow;


    }

    private void StateHandler()
    {
        //Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
        }

        //Mode - Unlimited
        else if (unlimited)
        {
            state = MovementState.unlimited;
            moveSpeed = 999f;
            return;
        }
    }

    private void MovePlayer()
    {

        if(Camera.main != null)
        {
            if (restricted) return;

            cameraForward = Camera.main.transform.forward;
            cameraRight = Camera.main.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = cameraForward * verticalInput + cameraRight * horizontalInput;
            moveDirection.Normalize();

            horizontalInput = moveDirection.x;
            verticalInput = moveDirection.z;

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            //Debug.Log(moveDirection);
            if(moveDirection.x != 0 || moveDirection.z != 0)
            {
                seconds -= 1 * Time.deltaTime;
                if (seconds <= 0)
                {
                    audio.Play();
                    seconds = 0.5f;
                }
            }

            if (animator.GetBool("isCrouched") == false)
            {
                if (moveDirection != Vector3.zero)
                {
                    animator.SetBool("isWalking", true);
                    transform.forward = moveDirection;
                }
                else
                {
                    animator.SetBool("isWalking", false);
                }
            }
            else
            {
                if (moveDirection != Vector3.zero)
                {
                    animator.SetBool("isCrawl", true);
                    transform.forward = moveDirection;
                }
                else
                {
                    animator.SetBool("isCrawl", false);

                }
            }


            //on ground
            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
     


    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        animator.SetBool("isJumping", false);
    }

    private void Crouch()
    {
        // Reduce player height when crouching
        characterCollider.height = .06f;
        characterCollider.center = new Vector3(0.003f, .03f, 0f);
        boxCollide.size = new Vector3(.06f, .02f, .07f);
        boxCollide.center = new Vector3(0f, .03f, 0f);


    }

    private void StandUp()
    {
        // Restore player height when standing up
        characterCollider.height = .19f;
        characterCollider.center = new Vector3(0.003f, .08f, 0f);

        // Adjust the BoxCollider
        boxCollide.size = new Vector3(0.06f, .1f, 0.07f);
        boxCollide.center = new Vector3(0f, .07f, 0f);


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "death_coll" || collision.gameObject.name == "death_bridge" || collision.gameObject.name ==  "deatharm")
        {
            Debug.Log("Player Should die. ");

            if (collision.gameObject.name == "deatharm")
            {
                Debug.Log("Player Should get zapped.\n ");

                StartCoroutine(PlayAnimationAndRespawn());
            }
            else
            {
                Respawn(); 
            }
           
        }

        IEnumerator PlayAnimationAndRespawn()
        {
            // Play animation
            animator.Play("rueelectrocuted");

            // Wait for a few seconds
            yield return new WaitForSeconds(4.0f); // Adjust the duration as needed

            // After waiting, respawn the player
            Respawn();
        }



        //  Debug.Log("should detect collision. ");
        if (collision.gameObject.name == "nextscenecollider") //chase says this sucks , ask him to help fix later
        {
            Debug.Log("Door should close... ");
            door_aniamtor.Play("close_door2");
            loadNextScene(); 
          
        }

    }

    public void loadNextScene() {

        //  int currentScene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("IN LOAD SCENE\n");
        LoadingScreenManager.Instance.SwitchToScene(SceneManager.GetActiveScene().buildIndex + 1);
        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Collided");
            SceneManager.LoadScene(nextSceneIndex);
        }
       // SceneManager.LoadScene("backup2");*/
        
        Debug.Log("LOAD SCENE");
    }

    public void Respawn()
    {
        //transform.position = new Vector3(-25.025f, 1.438f, 24.93f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

