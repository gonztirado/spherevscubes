using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static float STEP_ANGLE = 45f;

    [Header("Move")] public float stepTime;
    public float moveSpeed;
    public Transform moveDirection;

    [Header("Player Damage")] public LayerMask layerPlayer;
    public float damageRadius;
    public int damage;

    private float _rotationDirection = 1f;
    private float _rotatingAngles = 0f;
    private bool isFirstStep = true;

    private void FixedUpdate()
    {
        Move();
        CheckPlayerCollision();
    }

    private void CheckPlayerCollision()
    {
        Collider[] playerCollisions = Physics.OverlapSphere(transform.position, damageRadius, layerPlayer);
        if (playerCollisions.Length > 0)
        {
            playerCollisions[0].GetComponentInParent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        MoveToMoveDirection();
        RotateCube();
        CheckRotationDirection();
    }

    public void SetMoveDirection(Transform moveDirection)
    {
        if (moveDirection != null)
        {
            this.moveDirection = moveDirection;
            transform.LookAt(moveDirection.position);
        }
    }

    public virtual void MoveToMoveDirection()
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}