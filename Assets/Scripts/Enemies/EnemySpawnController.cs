using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public static EnemySpawnController instance;

    [Header("Spawn Settings")] public float spawnInterval;
    public float minDistance;
    public float maxDistance;
    public Transform playerPosition;
    public int maxEnemies;
    public int numEnemiesKilledToSpawnBoss;

    [Header("Enemy Prefabs")] public List<Enemy> enemyPrefabs;
    public Enemy bossPrefab;


    private int _numEnemiesInGame;
    private int _killEnemiesForBossCounter;
    private Enemy _bossInGame;

    private float _difficulty;

    private List<Stack<Enemy>> _enemyPools;

    private int _initMaxEnemies;
    private int _initNumEnemiesKilledToSpawnBoss;
    private float _currentDifficulty;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SaveInitDifficulty();
        InitEnemyPools();
        InvokeRepeating("SpawnEnemies", 2, spawnInterval);
    }

    public void ResetSpawnSettings()
    {
        InitEnemyPools();
        _numEnemiesInGame = 0;
        _killEnemiesForBossCounter = 0;
        if (_bossInGame != null)
        {
            Destroy(_bossInGame.gameObject);
            _bossInGame = null;
        }
    }

    public void ResetDifficulty()
    {
        _currentDifficulty = 1;
        maxEnemies = _initMaxEnemies;
        numEnemiesKilledToSpawnBoss = _initNumEnemiesKilledToSpawnBoss;
    }

    public void IncresaseDifficulty(float difficulty)
    {
        _currentDifficulty = Mathf.Pow(_currentDifficulty, difficulty);
        if (_currentDifficulty <= 1)
            _currentDifficulty = 1.1f;
        maxEnemies = DifficultyUtils.IncreaseExpontential(maxEnemies, _initMaxEnemies, _currentDifficulty);
        numEnemiesKilledToSpawnBoss = DifficultyUtils.DecreaseExpontential(numEnemiesKilledToSpawnBoss,
            _initNumEnemiesKilledToSpawnBoss, _currentDifficulty);
    }

    private void SaveInitDifficulty()
    {
        _initMaxEnemies = maxEnemies;
        _initNumEnemiesKilledToSpawnBoss = numEnemiesKilledToSpawnBoss;
    }


    private void InitEnemyPools()
    {
        _enemyPools = new List<Stack<Enemy>>();
        foreach (Enemy enemyPrefab in enemyPrefabs)
        {
            Stack<Enemy> enemyPool = new Stack<Enemy>();
            _enemyPools.Add(enemyPool);
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

        TrySpawnBoss();
    }

    private void TrySpawnBoss()
    {
        if (_bossInGame == null && _killEnemiesForBossCounter >= numEnemiesKilledToSpawnBoss)
        {
            _bossInGame = Instantiate(bossPrefab, RandomPositionInArea(), Quaternion.identity);
            _bossInGame.transform.SetParent(transform);
            _bossInGame.SetDifficulty(_currentDifficulty);

            _bossInGame.SetMoveDirection(playerPosition);
            _bossInGame.transform.LookAt(playerPosition.position);
            _bossInGame.GetComponent<Health>().onDie.AddListener(delegate
            {
                GameController.instance.IncrementEnemyKills();
                Destroy(_bossInGame.gameObject);
                _bossInGame = null;
                _numEnemiesInGame--;
                _killEnemiesForBossCounter = 0;
            });
            _bossInGame.GetComponent<HealthColorModifier>().ResetInitTransparency();
            _numEnemiesInGame++;
        }
    }

    private void TrySpawnEnemy(Enemy enemyPrefab, int poolIndex)
    {
        Enemy newEnemy = GetNewEnemy(enemyPrefab, poolIndex);
        newEnemy.SetDifficulty(_currentDifficulty);
        if (Random.Range(0f, 1f) < (newEnemy.spawnProbability))
        {
            newEnemy.SetMoveDirection(playerPosition);
            newEnemy.transform.position = RandomPositionInArea();
            newEnemy.transform.LookAt(playerPosition.position);
            newEnemy.GetComponent<HealthColorModifier>().ResetInitTransparency();
            _numEnemiesInGame++;
            newEnemy.gameObject.SetActive(true);
        }
        else
        {
            newEnemy.GetComponent<HealthColorModifier>().ChangeColorTransparency(0);
            newEnemy.gameObject.SetActive(false);
            _enemyPools[poolIndex].Push(newEnemy);
        }
    }

    private Enemy GetNewEnemy(Enemy enemyPrefab, int poolIndex)
    {
        Enemy newEnemy;

        if (_enemyPools != null && _enemyPools[poolIndex].Count > 0)
        {
            newEnemy = _enemyPools[poolIndex].Pop();
        }
        else
        {
            newEnemy = CreateNewEnemy(enemyPrefab);
            newEnemy.GetComponent<Health>().onDie.AddListener(delegate
            {
                newEnemy.gameObject.SetActive(false);
                GameController.instance.IncrementEnemyKills();
                
                _killEnemiesForBossCounter++;
                _enemyPools[poolIndex].Push(newEnemy);
                if(_numEnemiesInGame > 0)
                    _numEnemiesInGame--;
                newEnemy.GetComponent<Health>().RecoverAllHealth();
            });
        }

        return newEnemy;
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