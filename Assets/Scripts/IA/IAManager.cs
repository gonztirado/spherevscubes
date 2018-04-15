using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    public static IAManager instance;

    [Header("IA Controllers")] public IACameraController iaCameraController;
    public IAPlayerMoveController iaPlayerMoveController;
    public IAGunController iaGunController;

    [Header("IA active Keys")] public KeyCode IaActiveAllKey;
    public KeyCode IaCameraControllerKey;
    public KeyCode IaPlayerMoveControllerKey;
    public KeyCode IaGunControllerKey;

    private bool _isActiveAllIA;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyUp(IaActiveAllKey))
            ToogleAllIA();
        if (Input.GetKeyUp(IaCameraControllerKey))
            ToogleIACamera();
        if (Input.GetKeyUp(IaPlayerMoveControllerKey))
            ToogleIAPlayerMove();
        if (Input.GetKeyUp(IaGunControllerKey))
            ToogleIAGun();
    }

    private void ToogleAllIA()
    {
        _isActiveAllIA = !_isActiveAllIA;
        iaCameraController.enableIA = _isActiveAllIA;
        iaPlayerMoveController.enableIA = _isActiveAllIA;
        iaGunController.enableIA = _isActiveAllIA;
        GameController.instance.UpdateGameStatusText("ALL AI " + (_isActiveAllIA ? "ENABLED" : "DISABLED"), 1.5f);
    }

    private void ToogleIACamera()
    {
        iaCameraController.enableIA = !iaCameraController.enableIA;
        GameController.instance.UpdateGameStatusText("CAMERA AI " + (iaCameraController.enableIA ? "ENABLED" : "DISABLED"), 1.5f);
    }

    private void ToogleIAPlayerMove()
    {
        iaPlayerMoveController.enableIA = !iaPlayerMoveController.enableIA;
        GameController.instance.UpdateGameStatusText("PLAYER MOVE AI " + (iaPlayerMoveController.enableIA ? "ENABLED" : "DISABLED"), 1.5f);
    }

    private void ToogleIAGun()
    {
        iaGunController.enableIA = !iaGunController.enableIA;
        GameController.instance.UpdateGameStatusText("GUN AI " + (iaGunController.enableIA  ? "ENABLED" : "DISABLED"), 1.5f);
    }
}