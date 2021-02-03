using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    bool isPlayerAlive;
    bool isPlayerClose;
    float distance;

    public NavMeshAgent nav;
    Animator enemyAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        isPlayerAlive = true;
        isPlayerClose = false;

        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        Debug.Log(distance);

        isPlayerClose = distance <= 6f;

        if (isPlayerAlive && isPlayerClose)
        {
            nav.enabled = true;
            nav.SetDestination(player.position);
            transform.LookAt(player);

            if (distance > 1.1f)
            {
                enemyAnim.SetBool("isChasing", true);
                enemyAnim.SetBool("isAttacking", false);
                Debug.Log("Start attack animation");
            }
            else
            {
                enemyAnim.SetBool("isAttacking", true);
                enemyAnim.SetBool("isChasing", false);
                Debug.Log("Stop attack animation");
            }
        }
        else
        {
            nav.enabled = false;
            enemyAnim.SetBool("isChasing", false);
        }
    }
}
    