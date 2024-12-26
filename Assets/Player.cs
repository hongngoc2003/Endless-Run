using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public bool isDead;
    private bool playerStartToRun;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float milestoneIncreaser;
    private float speedMilestone;
    private float defaultMilestoneIncreaser;
    private float defaultSpeed;


    [Header("Jump info")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;

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
        sr = GetComponent<SpriteRenderer>();

        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    private void Update() {
        CheckCollision();
        AnimatorController();

        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.K))
            Knockback();
        if (Input.GetKeyDown(KeyCode.D) && !isDead)
            StartCoroutine(Die());

        if(isDead)
            return;

        if (isKnocked)
            return;

        if (playerStartToRun == true)
            ControlMove();

        if (isGrounded)
            canDoubleJump = true;

        ControlSpeed();

        CheckForLedge();
        CancelSlide();
        CheckInput();

    }

    private IEnumerator Die() {
        isDead = true;
        canBeKnocked = false;
        rb.velocity = knockbackDir;
        anim.SetBool("isDead", true);

        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
    }

    private IEnumerator MakeInvicible() {
        Color originColor = sr.color;
        Color darkColor = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);

        canBeKnocked = false;

        for (int i = 0; i < 5; i++)
        {
            sr.color = darkColor;
            yield return new WaitForSeconds(.2f);

            sr.color = originColor;
            yield return new WaitForSeconds(.2f);
        }


        sr.color = originColor;
        canBeKnocked = true;
    }

    #region Knockback
    private void Knockback() {
        if (!canBeKnocked)
            return;

        StartCoroutine(MakeInvicible());
        isKnocked = true;
        rb.velocity = knockbackDir;
    }

    private void CancelKnockback() => isKnocked = false;
    #endregion

    #region SpeedControl
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
    #endregion

    #region LedgeControl
    private void CheckForLedge() {
        if(ledgeDetected && canGrabLedge) {
            canGrabLedge = false;
            rb.gravityScale = 0;

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
        rb.gravityScale = 6;
        transform.position = climOverPosition;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;

    #endregion

    private void CancelSlide() {
        if(slideTimeCounter < 0 && !ceilingDetected)
            isSliding = false;
    }
    private void RollAnimFinished() => anim.SetBool("canRoll", false);
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


    private void ControlMove() {
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
        anim.SetBool("isKnocked", isKnocked);


        if(rb.velocity.y < -20) {
            anim.SetBool("canRoll", true);
        }
    }




    private void CheckCollision() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
    }

    private void CheckInput() {
        if (Input.GetButtonDown("Fire2"))
            playerStartToRun = true;

        if (Input.GetButtonDown("Jump"))
            JumpButton();

        if (Input.GetKeyDown(KeyCode.F))
            SlideButtonCheck();

    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));
    }
}
