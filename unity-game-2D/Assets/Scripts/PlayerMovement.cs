using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    bool jumpInput;
    float moveHorizontal;
    float moveVertical;
    bool jumpInputReleased;



    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    void Jump()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetKeyDown(KeyCode.W);
        jumpInputReleased = Input.GetKeyUp(KeyCode.W);

        rb2d.velocity = new Vector2(moveHorizontal * moveSpeed, rb2d.velocity.y);

        if (jumpInput && isGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        if (jumpInputReleased && rb2d.velocity.y > 0) 
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
        /*
        if(moveHorizontal != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveHorizontal), 1, 1);
        }
        */
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
