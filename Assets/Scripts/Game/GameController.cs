using System;
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
    public Text gameStatusText;

    [Header("Game elements")] public GameObject player;
    public GameObject enemies;
    public GameObject bullets;
    public GameObject cameras;

    [Header("Game settings")] public float levelTimeInSeconds;
    public float levelExponentialFactor;

    private bool _isGameStarted;
    private int _killsCounter;
    private float _levelTimeRemaining;
    private int _currentLevel = 1;

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
        player.GetComponent<Health>().onDie.AddListener(LoseGame);
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
            UpdateTimeBarInHud();
            if (_levelTimeRemaining < 0)
                WinLevel();
        }
    }


    public bool IsGameStarted
    {
        get { return _isGameStarted; }
    }

    public void IncrementEnemyKills()
    {
        _killsCounter++;
        UpdateKillsInHud();
    }

    public void StartNewGame()
    {
        _currentLevel = 1;
        player.GetComponent<Health>().RecoverAllHealth();
        _killsCounter = 0;
        UpdateKillsInHud();
        
        EnemySpawnController.instance.ResetDifficulty();
        ResetGame();
    }

    private void LoadNewLevel()
    {
        _currentLevel++;
        ResetGame();
        EnemySpawnController.instance.IncresaseDifficulty(levelExponentialFactor);
    }

    private void ResetGame()
    {
        ResetHubElements();
        ResetPlayerProperties();
        DeleteStageElements();
        EnemySpawnController.instance.ResetSpawnSettings();
        UpdateGameStatusText("LEVEL " + _currentLevel + " GO!", 2);
        _isGameStarted = true;
    }

    private void WinLevel()
    {
        _isGameStarted = false;
        UpdateGameStatusText("LEVEL " + _currentLevel + " FINISHED!", timeToHide: 2, callbackAction: LoadNewLevel);
    }

    private void LoseGame()
    {
        _currentLevel = 1;
        _isGameStarted = false;
        player.gameObject.SetActive(false);
        UpdateGameStatusText("YOU LOSE!", timeToHide: 2,
            callbackAction: delegate { MenuManager.instance.ShowMenu(true); });
    }

    private void ResetHubElements()
    {
        _levelTimeRemaining = levelTimeInSeconds;
        levelName.text = "LEVEL " + _currentLevel;
        UpdateTimeBarInHud();
    }


    private void ResetPlayerProperties()
    {
        cameras.transform.rotation = Quaternion.identity;
        player.transform.rotation = Quaternion.identity;
        player.gameObject.SetActive(true);
    }

    private void DeleteStageElements()
    {
        GeneralUtils.DestroyAllChildren(enemies.transform);
        GeneralUtils.DestroyAllChildren(bullets.transform);
    }

    private void UpdateTimeBarInHud()
    {
        timeBar.fillAmount = _levelTimeRemaining / levelTimeInSeconds;
    }

    private void UpdateKillsInHud()
    {
        killsCounterText.text = _killsCounter.ToString();
    }

    private void UpdateGameStatusText(string text, float timeToHide = -1, Action callbackAction = null)
    {
        StartCoroutine(FadeTextUtils.FadeTextToFullAlpha(0.2f, gameStatusText));
        gameStatusText.text = text;
        gameStatusText.gameObject.SetActive(true);
        if (timeToHide > 0)
        {
            StartCoroutine(HideUpdateGameStatusText(timeToHide, callbackAction));
            StartCoroutine(FadeTextUtils.FadeTextToZeroAlpha(timeToHide, gameStatusText));
        }
    }

    IEnumerator HideUpdateGameStatusText(float timeToHide, Action callbackAction = null)
    {
        yield return new WaitForSeconds(timeToHide);
        gameStatusText.gameObject.SetActive(false);
        if (callbackAction != null)
            callbackAction.Invoke();
    }
}