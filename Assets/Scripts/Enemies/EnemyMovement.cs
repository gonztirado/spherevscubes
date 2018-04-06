using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private static float STEP_ANGLE = 45f;

    public float stepTime;

    private float _rotationDirection = 1f;
    private float _rotatingAngles = 0f;
    private bool isFirstStep = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RotateCube();
        CheckRotationDirection();
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