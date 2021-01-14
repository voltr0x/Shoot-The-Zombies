using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    NavMeshAgent agent;

    [SerializeField] private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move the player
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        //Stores the direction
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1)
        {
            agent.Move(direction * speed * Time.deltaTime);
            agent.SetDestination(transform.position + direction);

            //Start the animation
            float speedPercent = agent.velocity.magnitude / agent.speed;
            playerAnim.SetFloat("Speed", speedPercent);
        }           
    }
}
