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

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

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

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            transform.forward = moveDirection;
        }
        else
        {
            animator.SetBool("isWalking", false); 
        }

        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
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
}
