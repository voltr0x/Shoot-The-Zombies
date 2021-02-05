using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }

        if (other.CompareTag("EnemyActive"))
        {
            EnemyActiveController enemyActive = other.GetComponent<EnemyActiveController>();
            if(enemyActive != null)
                enemyActive.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void Update()
    {
        //If bullet goes out of bounds without hitting anything
        Destroy(gameObject, 3);
    }
}
