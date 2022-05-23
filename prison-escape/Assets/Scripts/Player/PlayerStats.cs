using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 50f;
    float currentHealth;
    [SerializeField] Animator animator;
    public bool isDead;

    GameManager GM;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }

    void Die()
    {
        //GM.Respawn();
        GM.GameOver();
        //Destroy(gameObject);
        animator.SetBool("isDead", true);
        isDead = true;
    }

}
