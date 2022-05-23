using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour
{
    [SerializeField] bool applyKnockback;
    [SerializeField] float knockbackSpeedX;
    [SerializeField] float knockbackSpeedY;
    [SerializeField] float knockbackDeathSpeedX;
    [SerializeField] float knockbackDeathSpeedY;
    [SerializeField] float deathTorque;
    [SerializeField] float knockbackDuration;
    [SerializeField] float maxHealth;
    float currentHealth;
    float knockbackStart;
    int playerFacingDirection;
    bool playerOnLeft;
    bool knockback;

    PlayerMovement pc;
    GameObject aliveGO;
    Rigidbody2D rbAlive;

    private void Start()
    {
        currentHealth = maxHealth;

        pc = GameObject.Find("Player").GetComponent<PlayerMovement>();

        aliveGO = transform.Find("Alive").gameObject;

        rbAlive = aliveGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
    }

    private void Update()
    {
        CheckKnockback();
    }

    void Damage(float[] details)
    {
        currentHealth -= details[0];

        if(details[1] < aliveGO.transform.position.x)
        {
            playerFacingDirection = 1;
        }
        else
        {
            playerFacingDirection = -1;
        }

        if(playerFacingDirection == 1)
        {
            playerOnLeft = true;
        }
        else
        {
            playerOnLeft = false;
        }

        if(applyKnockback && currentHealth > 0.0f)
        {
            Knockback();
        }
        if(currentHealth <= 0.0f)
        {
            Die();
        }

    }

    void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
    }

    void CheckKnockback()
    {
        if(Time.time >= knockbackStart + knockbackDuration)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    void Die()
    {
        aliveGO.SetActive(false);
    }

}
