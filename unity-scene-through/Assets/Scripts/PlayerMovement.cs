using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

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

    //public float speed;
    public float rotationSpeed;

    Vector3 cameraForward;
    Vector3 cameraRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;



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



    private void MovePlayer()
    {


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
        characterCollider.height = .03f;
        characterCollider.center = new Vector3(0f, -1.21f, 0f);
        boxCollide.size = new Vector3(.49f, .38f, .75f);
        boxCollide.center = new Vector3(0f, .15f, .06f);

    }

    private void StandUp()
    {
        // Restore player height when standing up
        characterCollider.height = 1.08f;
        characterCollider.center = new Vector3(0f, .49f, 0f);

        // Adjust the BoxCollider
        boxCollide.size = new Vector3(0f, .7f, 0f);
        boxCollide.center = new Vector3(.49f, .71f, .45f);


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "death_coll")
        {
            Debug.Log("Player Should die. ");
            Respawn(); 
        }
        else
        {
            Debug.Log("Player not dying... ");

        }

    }

    public void Respawn()
    {
        transform.position = new Vector3(-25.025f, 1.438f, 24.93f);
    }
}