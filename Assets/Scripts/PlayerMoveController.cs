using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("Move")] public string horizontalAxis;
    public string verticalAxis;
    public float moveSpeed;

    void Update()
    {
        CheckMove();
    }

    private void CheckMove()
    {
        float h = Input.GetAxisRaw(horizontalAxis);
        float v = Input.GetAxisRaw(verticalAxis);

        if (h != 0 || v != 0)
        {
            transform.Rotate(Vector3.up * h  * moveSpeed * Time.deltaTime);
        }
    }
}