using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : NetworkBehaviour
{
    public float shootingIntervalMin = 2f;
    public float shootingIntervalMax = 3f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public int damage = 20;
    public float patrolRadius = 5f;

    private NavMeshAgent agent;

    void Start()
    {
        if (!IsServer) return;

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Think());
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator Think()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(shootingIntervalMin, shootingIntervalMax));
            ShootAtClosest();
        }
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Vector3 randomDir = Random.insideUnitSphere * patrolRadius;
            randomDir += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, patrolRadius, 1))
            {
                agent.SetDestination(hit.position);
            }
            yield return new WaitForSeconds(4f);
        }
    }

    void ShootAtClosest()
    {
        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var go = client.PlayerObject?.gameObject;
            if (go == null) continue;

            float dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = go.transform;
            }
        }

        if (closest != null)
        {
            Vector3 dir = (closest.position + Vector3.up - firePoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(dir);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;

            var bulletNet = bullet.GetComponent<NetworkObject>();
            bulletNet.Spawn(true);

            bullet.GetComponent<Bullet>().Init(damage);
        }
    }
}
