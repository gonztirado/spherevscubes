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
    }

    private Vector3 RandomPositionInArea()
    {
        return new Vector3(RandomFloatInArea(), 0, RandomFloatInArea());
    }

    private float RandomFloatInArea()
    {
        float randomFloatInArea = Random.Range(minDistance, maxDistance);
        float randomSign = Random.Range(-1f, 1f);
        if (randomSign < 0)
            randomFloatInArea *= -1;
        return randomFloatInArea;
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