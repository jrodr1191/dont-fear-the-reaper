using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    [SerializeField]
    float activeTime = 0.1f;
    float timeActivated;
    float alpha;
    [SerializeField]
    float alphaSet = 0.8f;
    float alphaMultiplier = 0.85f;

    private Transform player;

    private SpriteRenderer SR;
    private SpriteRenderer playerSR;

    private Color color;

    void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {

            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
