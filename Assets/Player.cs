using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator anim;


    [Header("Move info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;

    private bool canDoubleJump;

    private bool playerStartToRun;

    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    private bool isGrounded;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    private void Update() {
        AnimatorController();

        if (playerStartToRun == true)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        CheckCollision();

        CheckInput();

    }

    private void AnimatorController() {
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded",isGrounded);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckCollision() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput() {
        if (Input.GetButtonDown("Fire2"))
            playerStartToRun = true;

        if (Input.GetButtonDown("Jump") )
            JumpButton();
    }

    private void JumpButton() {
        if (isGrounded) {
            canDoubleJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else if(canDoubleJump) {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y -groundCheckDistance));
    }
}
