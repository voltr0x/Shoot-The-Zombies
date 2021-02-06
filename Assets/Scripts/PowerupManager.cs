using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerupManager : MonoBehaviour
{
    public float range = 30f;
    Vector3 offset = new Vector3(0f, 1f, 0f);
    public GameObject ammo;
    public GameObject health;

    float spawnCooldown = 4.0f;
    float spawnTime;

    void Start()
    {
        spawnTime = Time.deltaTime + spawnCooldown;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
                SpawnHealth();
            else
                SpawnAmmo();
            spawnTime = Time.deltaTime + spawnCooldown;
        }
    }

    void SpawnHealth()
    {
        Vector3 point;
        if (RandomPoint(transform.position, range, out point))
        {
            //Instantiate health
            Instantiate(health, point + offset, transform.rotation);
        }
    }

    void SpawnAmmo()
    {
        Vector3 point;
        if (RandomPoint(transform.position, range, out point))
        {
            //Instantiate ammo
            Instantiate(ammo, point + offset, transform.rotation);
        }
    }
}
