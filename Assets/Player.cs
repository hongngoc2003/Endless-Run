using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;

    private bool runBegin;

    [Header("Collision info")]
    public LayerMask whatIsGround;
    public float groundCheckDistance;
    private bool isGrounded;

    private void Start() {
        
    }

    private void Update() {
        if (runBegin == true)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        CheckCollision();

        CheckInput();

    }

    private void CheckCollision() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput() {
        if (Input.GetButtonDown("Fire2"))
            runBegin = true;

        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y -groundCheckDistance));
    }
}
