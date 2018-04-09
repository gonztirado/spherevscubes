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
            UpdateTimeBarInHud();
            if (_levelTimeRemaining < 0)
                WinGame();
        }
    }

    private void WinGame()
    {
        _isGameStarted = false;
        UpdateGameStatusText("YOU WIN!", timeToHide:2, callbackAction:delegate { MenuManager.instance.ShowMenu(true); });
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

    public void ResetGame()
    {
        _isGameStarted = true;
        ResetHubElements();
        ResetPlayerProperties();
        DeleteStageElements();
        UpdateGameStatusText("GO!", 2);
    }

    private void ResetHubElements()
    {
        player.GetComponent<Health>().RecoverAllHealth();
        
        _killsCounter = 0;
        UpdateKillsInHud();
        
        _levelTimeRemaining = levelTimeInSeconds;
        UpdateTimeBarInHud();
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
        if (timeToHide > 0) {
            StartCoroutine(HideUpdateGameStatusText(timeToHide, callbackAction));
            StartCoroutine(FadeTextUtils.FadeTextToZeroAlpha(timeToHide, gameStatusText));
        }
    }
    
    IEnumerator HideUpdateGameStatusText(float timeToHide, Action callbackAction = null)
    {
        yield return new WaitForSeconds(timeToHide);
        gameStatusText.gameObject.SetActive(false);
        if(callbackAction != null)
            callbackAction.Invoke();
    }
    
}