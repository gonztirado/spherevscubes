using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Header("Spawn Settings")] public float spawnInterval;
    public float minDistance;
    public float maxDistance;
    public Transform playerPosition;

    [Header("Enemy Prefabs")] public Enemy simpleEnemy;


    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
    }

    private void SpawnEnemy()
    {
        Enemy newEnemy = Instantiate(simpleEnemy, RandomPositionInArea(), Quaternion.identity);
        newEnemy.SetMoveDirection(playerPosition);
        newEnemy.transform.SetParent(transform);
        newEnemy.GetComponent<Health>().onDie.AddListener(delegate { GameController.instance.IncrementEnemyKills(); });
    }

    private Vector3 RandomPositionInArea()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return new Vector3(Random.Range(-maxDistance, minDistance), 0, Random.Range(minDistance, maxDistance));
            case 1:
                return new Vector3(Random.Range(minDistance, maxDistance), 0, Random.Range(-minDistance, maxDistance));
            case 2:
                return new Vector3(Random.Range(-minDistance, maxDistance), 0, Random.Range(-maxDistance, -minDistance));
            default:
                return new Vector3(Random.Range(-maxDistance, -minDistance), 0, Random.Range(-maxDistance, minDistance));
        }
    }

    // For debug purposes

    private void OnDrawGizmos()
    {
        DrawSpawnArea();
    }

    private void DrawSpawnArea()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(minDistance * 2, 10, minDistance * 2));
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(maxDistance * 2, 10, maxDistance * 2));
    }
}