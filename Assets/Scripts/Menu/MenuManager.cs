using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public KeyCode showMenuKey = KeyCode.Escape;
    public GameObject menuCanvas;
    public GameObject hudCanvas;
    public Button continueButton;
    public Button startGameButton;
    public Button quitButton;


    private bool _isMenuShown;

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
        ShowMenu(true);
        AddButtonsListeners();
    }


    void Update()
    {
        CheckToogleShowMenu();
    }

    private void AddButtonsListeners()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(delegate { ShowMenu(false); });
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    private void CheckToogleShowMenu()
    {
        if (Input.GetKeyUp(showMenuKey))
            ShowMenu(!_isMenuShown);
    }

    public void ShowMenu(bool showMenu)
    {
        _isMenuShown = showMenu;
        menuCanvas.SetActive(_isMenuShown);
        hudCanvas.SetActive(!_isMenuShown);
        Time.timeScale = _isMenuShown ? 0f : 1f;
    }


    private void QuitGame()
    {
        Application.Quit();
    }
}