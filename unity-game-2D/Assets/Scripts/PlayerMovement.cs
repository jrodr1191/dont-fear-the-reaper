using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    bool jumpInput;
    float moveHorizontal;
    bool jumpInputReleased;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        Jump();
        FlipSprite();
        Run();
    }

    void FixedUpdate()
    {

    }

    void Run()
    {
        rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }

    void Jump()
    {
        jumpInput = Input.GetKeyDown(KeyCode.W);
        jumpInputReleased = Input.GetKeyUp(KeyCode.W);

        if (jumpInput && isGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        if (jumpInputReleased && rb2d.velocity.y > 0) 
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
