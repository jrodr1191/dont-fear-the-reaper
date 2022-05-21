using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Walking,
        Knockback,
        Dead
    }

    State currentState;

    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float knockbackDuration;
    [SerializeField] float lastTouchDamageTime;
    [SerializeField] float touchDamageCooldown;
    [SerializeField] float touchDamage;
    [SerializeField] float touchDamageWidth;
    [SerializeField] float touchDamageHeight;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform touchDamageCheck;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsPlayer;

    [SerializeField] Vector2 knockbackSpeed;

    float currentHealth;
    float knockbackStartTime;

    float[] attackDetails = new float[2];

    int facingDirection;
    int damageDirection;

    Vector2 movement;
    Vector2 touchDamageBotLeft;
    Vector2 touchDamageTopRight;

    bool groundDetected;
    bool wallDetected;

    GameObject alive;
    Rigidbody2D aliveRb;
    Animator aliveAnim;

    void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();

        currentHealth = maxHealth;
        facingDirection = 1;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                UpdateWalking();
                break;
            case State.Knockback:
                UpdateKnockback();
                break;
            case State.Dead:
                UpdateDead();
                break;
        }
    }

    //--IDLE STATE--

    void EnterWalking()
    {

    }

    void UpdateWalking()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        CheckTouchDamage();

        if (!groundDetected || wallDetected)
        {
            Flip();
        }
        else {
            movement.Set(movementSpeed * facingDirection, aliveRb.velocity.y);
            aliveRb.velocity = movement;
        }
    }

    void ExitWalking()
    {

    }

    //--KNOCKBACK STATE--

    void EnterKnockback()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("KnockBack", true);
    }

    void UpdateKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Walking);
        }
    }

    void ExitKnockback()
    {
        aliveAnim.SetBool("KnockBack", false);
    }

    //--DEAD STATE--
    void EnterDead()
    {
        Destroy(gameObject, 5f);
        aliveAnim.SetBool("isDead", true);

        GetComponentInChildren<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }

    void UpdateDead()
    {

    }

    void ExitDead()
    {

    }

    //--OTHER FUNCTIONS--

    void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];

        if(attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        if(currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        else if(currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    void CheckTouchDamage()
    {
        if(Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);

            if(hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalking();
                break;
            case State.Knockback:
                ExitKnockback();
                break;
            case State.Dead:
                ExitDead();
                break;
        }

        switch (state)
        {
            case State.Walking:
                EnterWalking();
                break;
            case State.Knockback:
                EnterKnockback();
                break;
            case State.Dead:
                EnterDead();
                break;
        }

        currentState = state;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
