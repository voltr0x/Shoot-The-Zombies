using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform playerTransform;
    bool isPlayerAlive;
    bool isPlayerClose;
    float distance;

    public NavMeshAgent nav;
    Animator enemyAnim;

    int health = 30;
    float damageRate = 0.2f;

    PlayerController player;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerClose = false;
        enemyAnim = GetComponent<Animator>();
        player = playerPrefab.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerAlive = player.isAlive;
        
        distance = Vector3.Distance(playerTransform.position, transform.position);
        isPlayerClose = distance <= 6f;

        if (isPlayerAlive && isPlayerClose)
        {
            nav.enabled = true;
            nav.SetDestination(playerTransform.position);
            transform.LookAt(playerTransform);

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
    