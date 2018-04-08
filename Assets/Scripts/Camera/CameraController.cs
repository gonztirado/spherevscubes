using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Move")] public string horizontalAxis;
    public string verticalAxis;
    public float moveSpeed;

    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    private bool _isInThirdPersonView;

    void Update()
    {
        CheckCameraRotation();
        CheckCameraChange();
    }

    private void CheckCameraRotation()
    {
        float horizontalMove = Input.GetAxisRaw(horizontalAxis);
        if (horizontalMove != 0 && Time.timeScale > 0f)
        {
            transform.Rotate(Vector3.up * horizontalMove * moveSpeed * Time.deltaTime);
        }
    }

    private void CheckCameraChange()
    {
        float verticalMove = Input.GetAxisRaw(verticalAxis);

        if (verticalMove > 0 && _isInThirdPersonView && Time.timeScale > 0f)
            ShowFirstPersonView();
        else if (verticalMove < 0 && !_isInThirdPersonView && Time.timeScale > 0f)
            ShowThirdPersonView();
    }


    public void ShowFirstPersonView()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        _isInThirdPersonView = false;
    }

    public void ShowThirdPersonView()
    {
        thirdPersonCamera.enabled = true;
        firstPersonCamera.enabled = false;
        _isInThirdPersonView = true;
    }
}