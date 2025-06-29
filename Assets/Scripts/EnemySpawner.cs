using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Debug.Log("Spawner active on server");
            StartCoroutine(SpawnLoop());
        }
    }


    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Vector3 pos = transform.position + Random.insideUnitSphere * 5f;
            pos.y = 0;

            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn(true);
            Debug.Log("Spawned enemy at " + pos);
        }
    }
}
