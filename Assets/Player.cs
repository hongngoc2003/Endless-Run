using System;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Speed info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float milestoneIncreaser;
    private float speedMilestone;
    private float defaultMilestoneIncreaser;
    private float defaultSpeed;


    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    private bool canDoubleJump;

    private bool playerStartToRun;

    [Header("Slide info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCooldown;
    private float slideTimeCounter;
    private float slideCooldownCounter;
    private bool isSliding;

    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float ceilingCheckDistance;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isGrounded;
    private bool wallDetected;
    private bool ceilingDetected;
    [HideInInspector] public bool ledgeDetected;

    [Header("Ledge info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBeginPosition;
    private Vector2 climOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    private void Update() {
        CheckCollision();
        AnimatorController();

        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if (playerStartToRun == true)
            Move();

        if (isGrounded)
            canDoubleJump = true;

        ControlSpeed();

        CheckForLedge();
        CheckForSlide();
        CheckInput();

    }

    private void ResetSpeed() {
        moveSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
    }

    private void ControlSpeed() {
        if (moveSpeed == maxSpeed)
            return;

        if(transform.position.x > speedMilestone) {
            speedMilestone += milestoneIncreaser;

            moveSpeed *= speedMultiplier;
            milestoneIncreaser *= speedMultiplier;

            if(moveSpeed > maxSpeed)
                moveSpeed = maxSpeed;
        }
    }

    private void CheckForLedge() {
        if(ledgeDetected && canGrabLedge) {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBeginPosition = ledgePosition + offset1;
            climOverPosition = ledgePosition + offset2;

            canClimb = true;
        }

        if(canClimb)
            transform.position = climbBeginPosition;
    }

    private void LedgeClimbOver() {
        canClimb = false;
        transform.position = climOverPosition;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;

    private void CheckForSlide() {
        if(slideTimeCounter < 0 && !ceilingDetected)
            isSliding = false;
    }

    private void Move() {
        if (wallDetected) {
            ResetSpeed();
            return;
        }

        if(isSliding)
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void AnimatorController() {
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
        
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("canClimb",canClimb);
    }

    private void CheckCollision() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);

        Debug.Log(ledgeDetected);
    }

    private void CheckInput() {
        if (Input.GetButtonDown("Fire2"))
            playerStartToRun = true;

        if (Input.GetButtonDown("Jump"))
            JumpButton();

        if (Input.GetKeyDown(KeyCode.F))
            SlideButtonCheck();

    }

    private void SlideButtonCheck() {
        if(rb.velocity.x != 0 || slideCooldownCounter < 0) {
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void JumpButton() {
        if (isSliding)
            return;

        if (isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else if (canDoubleJump) {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));
    }
}
