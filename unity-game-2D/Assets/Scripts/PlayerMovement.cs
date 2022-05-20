using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;

    [Header("Move")]
    [SerializeField] float moveSpeed;
    float moveHorizontal;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    bool jumpInput;
    bool jumpInputReleased;

    [Header("Dash")]
    [SerializeField] float startDashTime;
    [SerializeField] float dashForce;
    [SerializeField] float dashCooldown = 2f;
    float dashCounter = 0f;
    float dashDirection;
    float currentDashTime;
    bool isDashing;

    [Header("Wall Jump")]
    [SerializeField] float wallSlideSpeed = 7f;
    bool isGrabbing;
    float gravityStore;

    bool isTouchingLeft;
    bool isTouchingRight;
    int touchingLeftorRight;
    bool wallJumping;
    bool wallSliding;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask groundLayer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravityStore = rb2d.gravityScale;
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        Jump();
        FlipSprite();
        Run();
        Dash();
        WallSlide();
        WallJump();

        animator.SetFloat("yVelocity", rb2d.velocity.y);
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    void Run()
    {
        rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void Dash()
    {
        if(Time.time > dashCounter)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isDashing = true;
                currentDashTime = startDashTime;
                rb2d.velocity = Vector2.zero;
                dashDirection = (int)moveHorizontal;
                dashCounter = Time.time + dashCooldown;
            }
        }   

        if (isDashing)
        {
            rb2d.velocity = transform.right * dashDirection * dashForce;
            currentDashTime -= Time.deltaTime;
            if(currentDashTime <= 0)
            {
                isDashing = false;
            }
        }

        animator.SetBool("isDashing", isDashing);
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
            animator.SetBool("isJumping", true);
        }

        if (jumpInputReleased && rb2d.velocity.y > 0) 
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }

    void WallSlide()
    {
        isTouchingLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - 0.05f,
                                                                  gameObject.transform.position.y),
                                                                  new Vector2(0.2f, 0.2f), 0f,
                                                                  groundLayer);
        isTouchingRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + 0.05f,
                                                                  gameObject.transform.position.y),
                                                                  new Vector2(0.2f, 0.2f), 0f,
                                                                  groundLayer);
        if (isTouchingLeft)
        {
            touchingLeftorRight = 1;
        }
        else if (isTouchingRight)
        {
            touchingLeftorRight = -1;
        }

        if ((isTouchingRight || isTouchingLeft) && !isGrounded())
        {
            rb2d.gravityScale = wallSlideSpeed;
            rb2d.velocity = Vector2.zero;
            wallSliding = true;
            if (Input.GetKeyDown(KeyCode.W))
            {
                wallJumping = true;
                Invoke("SetJumpingToFalse", 0.15f);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                rb2d.velocity = new Vector2(moveSpeed * touchingLeftorRight, rb2d.velocity.y);
            }            
        }
        else
        {
            rb2d.gravityScale = gravityStore;
            wallSliding = false;
        }

        animator.SetBool("isGrabbing", wallSliding);
    }

    void WallJump()
    {
        if (wallJumping)
        {
            rb2d.velocity = new Vector2(moveSpeed * touchingLeftorRight, jumpForce);
        }
    }

    void SetJumpingToFalse()
    {
        wallJumping = false;
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void GroundCheck()
    {
        bool isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
        animator.SetBool("isJumping", !isGrounded);
    }
}
