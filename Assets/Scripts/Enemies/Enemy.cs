using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static float STEP_ANGLE = 45f;

    public float stepTime;
    public float moveSpeed;

    private Transform moveDirection;
    private float _rotationDirection = 1f;
    private float _rotatingAngles = 0f;
    private bool isFirstStep = true;

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        MoveToMoveDirection();
        RotateCube();
        CheckRotationDirection();
    }

    public void SetMoveDirection(Transform moveDirection)
    {
        this.moveDirection = moveDirection;
        transform.LookAt(moveDirection.position);
    }

    private void MoveToMoveDirection()
    {
        if (moveDirection != null)
        {
            Vector3 direction = moveDirection.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
    }


    private void RotateCube()
    {
        float rotateAngle = STEP_ANGLE * Time.deltaTime / stepTime;
        transform.Rotate(Vector3.up * rotateAngle * _rotationDirection);
        _rotatingAngles += rotateAngle;
    }

    private void CheckRotationDirection()
    {
        float expetedStepAngle = isFirstStep ? STEP_ANGLE : 2 * STEP_ANGLE;
        if (_rotatingAngles >= expetedStepAngle)
        {
            _rotatingAngles = 0;
            _rotationDirection *= -1;
            isFirstStep = false;
        }
    }
}