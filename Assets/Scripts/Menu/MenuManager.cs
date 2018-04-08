using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public KeyCode showMenuKey = KeyCode.Escape;
    public GameObject menuCanvas;
    public GameObject hudCanvas;


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
    }

    void Update()
    {
        CheckToogleShowMenu();
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
}