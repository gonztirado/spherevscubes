using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;


    [Header("Hub elements")] public Image healthBar;
    public Text ammoPanel;
    public Image timeBar;
    public Text levelName;
    public Text killsCounterText;

    [Header("Game elements")] public GameObject player;
    public GameObject enemies;
    public GameObject bullets;
    public GameObject cameras;

    [Header("Game settings")] public float levelTimeInSeconds;

    private bool _isGameStarted;
    private int _killsCounter;
    private float _levelTimeRemaining;

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
        player.GetComponent<Health>().uIBar = healthBar;
    }


    void Update()
    {
        UpdateLevelRemainingTime();
    }

    private void UpdateLevelRemainingTime()
    {
        if (_isGameStarted && Time.timeScale > 0)
        {
            _levelTimeRemaining -= Time.deltaTime;
            UpdateTimeBar();
        }
    }


    public bool IsGameStarted
    {
        get { return _isGameStarted; }
    }

    public void ResetGame()
    {
        _isGameStarted = true;
        ResetHubElements();
        ResetPlayerProperties();
        DeleteStageElements();
    }

    private void ResetHubElements()
    {
        player.GetComponent<Health>().RecoverAllHealth();
        _killsCounter = 0;
        killsCounterText.text = _killsCounter.ToString();
        _levelTimeRemaining = levelTimeInSeconds;
        UpdateTimeBar();
    }

    private void ResetPlayerProperties()
    {
        cameras.transform.rotation = Quaternion.identity;
        player.transform.rotation = Quaternion.identity;
    }

    private void DeleteStageElements()
    {
        GeneralUtils.DestroyAllChildren(enemies.transform);
        GeneralUtils.DestroyAllChildren(bullets.transform);
    }

    private void UpdateTimeBar()
    {
        timeBar.fillAmount = _levelTimeRemaining / levelTimeInSeconds;
    }
}