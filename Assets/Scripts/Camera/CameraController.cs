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
        if (horizontalMove != 0)
        {
            transform.Rotate(Vector3.up * horizontalMove * moveSpeed * Time.deltaTime);
        }
    }

    private void CheckCameraChange()
    {
        float verticalMove = Input.GetAxisRaw(verticalAxis);

        if (verticalMove > 0 && _isInThirdPersonView)
            ShowFirstPersonView();
        else if (verticalMove < 0 && !_isInThirdPersonView)
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