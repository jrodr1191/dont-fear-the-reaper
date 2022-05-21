using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] GameObject player;
    [SerializeField] float respawnTime;
    float respawnStartTime;
    bool respawn;

    CinemachineVirtualCamera CVC;

    private void Start()
    {
        CVC = GameObject.Find("VC follow camera").GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        respawnStartTime = Time.time;
        respawn = true;
    }

    void CheckRespawn()
    {
        if(Time.time >= respawnStartTime + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }
}
