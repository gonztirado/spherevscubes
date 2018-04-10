using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Header("Spawn Settings")] public float spawnInterval;
    public float minDistance;
    public float maxDistance;
    public Transform playerPosition;
    public int maxEnemies;
    public int numEnemiesKilledToSpawnBoss;

    [Header("Enemy Prefabs")] public List<Enemy> enemyPrefabs;
    public Enemy bossPrefab;


    private int _numEnemiesInGame;
    private bool _isBossInGame;

    private List<Stack<Enemy>> enemyPools;


    void Start()
    {
        InitEnemyPools();
        InvokeRepeating("SpawnEnemies", spawnInterval, spawnInterval);
    }

    private void InitEnemyPools()
    {
        enemyPools = new List<Stack<Enemy>>();
        foreach (Enemy enemyPrefab in enemyPrefabs)
        {
            Stack<Enemy> enemyPool = new Stack<Enemy>();
            enemyPools.Add(enemyPool);
        }
    }

    private void SpawnEnemies()
    {
        if (_numEnemiesInGame < maxEnemies)
        {
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                TrySpawnEnemy(enemyPrefabs[i], poolIndex: i);
            }
        }
    }

    private void TrySpawnEnemy(Enemy enemyPrefab, int poolIndex)
    {
        Enemy newEnemy = GetNewEnemy(enemyPrefab, poolIndex);

        if (newEnemy.spawnProbability < Random.Range(0f, 1f))
        {
            newEnemy.SetMoveDirection(playerPosition);
            newEnemy.transform.position = RandomPositionInArea();
            newEnemy.GetComponent<Health>().RecoverAllHealth();
            newEnemy.GetComponent<Health>().onDie.AddListener(delegate
            {
                GameController.instance.IncrementEnemyKills();
                newEnemy.gameObject.SetActive(false);
                enemyPools[poolIndex].Push(newEnemy);
                _numEnemiesInGame--;
            });
            newEnemy.GetComponent<HealthColorModifier>().ResetInitTransparency();
            _numEnemiesInGame++;
            newEnemy.gameObject.SetActive(true);
        }
        else
        {
            newEnemy.gameObject.SetActive(false);
            enemyPools[poolIndex].Push(newEnemy);
        }
    }

    private Enemy GetNewEnemy(Enemy enemyPrefab, int poolIndex)
    {
        if (enemyPools[poolIndex].Count > 0)
            return enemyPools[poolIndex].Pop();
        return CreateNewEnemy(enemyPrefab);
    }

    private Enemy CreateNewEnemy(Enemy enemyPrefab)
    {
        Enemy enemy = Instantiate(enemyPrefab, RandomPositionInArea(), Quaternion.identity);
        enemy.transform.SetParent(transform);
        return enemy;
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
                return new Vector3(Random.Range(-minDistance, maxDistance), 0,
                    Random.Range(-maxDistance, -minDistance));
            default:
                return new Vector3(Random.Range(-maxDistance, -minDistance), 0,
                    Random.Range(-maxDistance, minDistance));
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