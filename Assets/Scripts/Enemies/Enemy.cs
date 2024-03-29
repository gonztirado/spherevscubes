﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static float STEP_ANGLE = 45f;

    [Header("Spawn Probability")] public float spawnProbability;

    [Header("Move")] public float stepTime;
    public float moveSpeed;
    public Transform moveDirection;

    [Header("Other enemies detector")] public float checkOtherEnemiesInFrontRadius = 5;
    public float checkOtherEnemiesInFrontOffset = 5;
    public LayerMask layerEnemies;

    [Header("Player Damage")] public LayerMask layerPlayer;
    public float damageRadius;
    public int damage;

    private float _rotationDirection = 1f;
    private float _rotatingAngles = 0f;
    private bool isFirstStep = true;

    private float _initSpawnProbability;
    private float _initStepTime;
    private float _initMoveSpeed;
    private int _initDamage;

    private void Awake()
    {
        _initSpawnProbability = spawnProbability;
        _initStepTime = stepTime;
        _initMoveSpeed = moveSpeed;
        _initDamage = damage;
    }

    protected void OnEnable()
    {
        _rotationDirection = 1;
        _rotatingAngles = 0f;
        isFirstStep = true;
    }

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
            GetComponent<Health>().Die();
        }
    }

    public void SetDifficulty(float difficulty)
    {
        spawnProbability = DifficultyUtils.IncreaseExpontential(spawnProbability, _initSpawnProbability, difficulty);
        stepTime = DifficultyUtils.DecreaseExpontential(stepTime, _initStepTime, difficulty);
        moveSpeed = DifficultyUtils.IncreaseExpontential(moveSpeed, _initMoveSpeed, difficulty);
        damage = DifficultyUtils.IncreaseExpontential(damage, _initDamage, difficulty);
    }

    public virtual void Move()
    {
        if (CheckCanMove())
        {
            MoveToMoveDirection();
            RotateCube();
            CheckRotationDirection();
        }
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

    protected bool CheckCanMove()
    {
        Collider[] playerCollision =
            Physics.OverlapSphere(GetOtherEnemiesDetectorCenter(), checkOtherEnemiesInFrontRadius, layerPlayer);
        if (playerCollision.Length > 0)
            return true;

        Collider[] otherEnemiesCollision =
            Physics.OverlapSphere(GetOtherEnemiesDetectorCenter(), checkOtherEnemiesInFrontRadius, layerEnemies);
        if (otherEnemiesCollision.Length > 0)
            return false;

        return true;
    }

    private Vector3 GetOtherEnemiesDetectorCenter()
    {
        return transform.position + transform.forward * checkOtherEnemiesInFrontOffset;
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
        Gizmos.DrawWireSphere(GetOtherEnemiesDetectorCenter(), checkOtherEnemiesInFrontRadius);
    }
}