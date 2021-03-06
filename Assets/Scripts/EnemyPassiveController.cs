﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPassiveController : MonoBehaviour
{
    bool isPlayerAlive;
    bool isPlayerClose;
    PlayerController player;

    float distance;
    public NavMeshAgent nav;
    Animator enemyAnim;

    int health = 60;
    float damageRate = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerClose = false;
        enemyAnim = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerAlive = player.isAlive;
        
        distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerClose = distance <= 10f;

        if (isPlayerAlive && isPlayerClose)
        {
            nav.enabled = true;
            nav.SetDestination(player.transform.position);
            transform.LookAt(player.transform);

            if (distance > 1.1f)
            {
                enemyAnim.SetBool("isChasing", true);
                enemyAnim.SetBool("isAttacking", false);
            }
            else
            {
                enemyAnim.SetBool("isAttacking", true);
                enemyAnim.SetBool("isChasing", false);

                //Deal damage to player
                player.TakeDamage(damageRate);
            }
        }
        else
        {
            nav.enabled = false;
            enemyAnim.SetBool("isChasing", false);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        //TO-DO: Death animation
        Destroy(gameObject);
    }
}
    