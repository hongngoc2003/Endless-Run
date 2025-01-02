using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    public bool canMove;

    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float ceilingCheckDistance;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private Transform groundForwardCheck; 
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    private bool isGrounded;
    private bool groundForward;
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
    }
    private void Update() {
        CheckCollision();
        AnimatorControllers();
        ControlMovement();

        if (isGrounded && !groundForward)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorControllers() {
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    private void ControlMovement() {
        if (canMove) {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void CheckCollision() {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        groundForward = Physics2D.Raycast(groundForwardCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
    }


    private void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundForwardCheck.position, new Vector2(groundForwardCheck.position.x, groundForwardCheck.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));
    }
}
