using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Move Settings")] public string moveCameraAxis = "Horizontal";
    public float moveSpeed;
    
    [Header("Cameras")]
    public string changeCameraAxis = "Vertical";
    private bool isInThirdPersonView = true;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    

    private void Start()
    {
        if(isInThirdPersonView)
            ShowThirdPersonView();
        else
            ShowFirstPersonView();
    }

    private void Update()
    {
        CheckCameraRotation();
        CheckCameraChange();
    }

    protected virtual void CheckCameraRotation()
    {
        float horizontalMove = Input.GetAxisRaw(moveCameraAxis);
        RotateCamera(horizontalMove);
    }

    protected void RotateCamera(float horizontalMove)
    {
        if (horizontalMove != 0 && Time.timeScale > 0f)
        {
            transform.Rotate(Vector3.up * horizontalMove * moveSpeed * Time.deltaTime);
        }
    }

    private void CheckCameraChange()
    {
        float verticalMove = Input.GetAxisRaw(changeCameraAxis);

        if (verticalMove > 0 && isInThirdPersonView && Time.timeScale > 0f)
            ShowFirstPersonView();
        else if (verticalMove < 0 && !isInThirdPersonView && Time.timeScale > 0f)
            ShowThirdPersonView();
    }


    private void ShowFirstPersonView()
    {
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        isInThirdPersonView = false;
    }

    private void ShowThirdPersonView()
    {
        thirdPersonCamera.enabled = true;
        firstPersonCamera.enabled = false;
        isInThirdPersonView = true;
    }
}