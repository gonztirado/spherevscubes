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
    public Text killsCounter;

    [Header("Game elements")] public GameObject player;
    public GameObject enemies;
    public GameObject bullets;
    public GameObject cameras;

    private bool _isGameStarted;

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
    }

    public bool IsGameStarted
    {
        get { return _isGameStarted; }
    }

    public void ResetGame()
    {
        _isGameStarted = true;
        ResetPlayerProperties();
        ResetStage();
    }

    private void ResetPlayerProperties()
    {
        cameras.transform.rotation = Quaternion.identity;
        player.transform.rotation = Quaternion.identity;
        
    }

    private void ResetStage()
    {
        GeneralUtils.DestroyAllChildren(enemies.transform);
        GeneralUtils.DestroyAllChildren(bullets.transform);
    }
}
