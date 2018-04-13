using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagEnemy : Enemy
{
    [Header("Zigzag settings")] public float changeDirectionTime;
    public float changeAngle;


    private enum ZigZagMoveDirection
    {
        ToPlayer,
        ToRight,
        ToPlayerAgain,
        ToLeft
    }

    private ZigZagMoveDirection _zigZagMoveDirection = ZigZagMoveDirection.ToPlayer;
    private float _currentZigZagTime;
    
    protected void OnEnable()
    {
        base.OnEnable();
        changeDirectionTime = 0;
        _zigZagMoveDirection = ZigZagMoveDirection.ToPlayer;
        _currentZigZagTime = 0;
    }

    public override void MoveToMoveDirection()
    {
        _currentZigZagTime += Time.deltaTime;
        if (_currentZigZagTime > changeDirectionTime)
        {
            _zigZagMoveDirection = GetNextZizZagMoveDirection(_zigZagMoveDirection);
            _currentZigZagTime = 0;
        }
        
        transform.Translate(GetZigZagDirectionVector() * moveSpeed * Time.deltaTime, Space.World);
    }

    private Vector3 GetZigZagDirectionVector()
    {
        if (moveDirection != null)
        {
            Vector3 direction = moveDirection.position - transform.position;
            switch (_zigZagMoveDirection)
            {
                case ZigZagMoveDirection.ToRight:
                    direction = Quaternion.AngleAxis(changeAngle, Vector3.up) * direction;
                    break;
                case ZigZagMoveDirection.ToLeft:
                    direction = Quaternion.AngleAxis(-changeAngle, Vector3.up) * direction;
                    break;
            }

            direction.Normalize();
            return direction;
        }

        return Vector3.zero;
    }

    private ZigZagMoveDirection GetNextZizZagMoveDirection(ZigZagMoveDirection currentDirection)
    {
        switch (currentDirection)
        {
            case ZigZagMoveDirection.ToPlayer:
                return ZigZagMoveDirection.ToRight;
            case ZigZagMoveDirection.ToRight:
                return ZigZagMoveDirection.ToPlayerAgain;
            case ZigZagMoveDirection.ToPlayerAgain:
                return ZigZagMoveDirection.ToLeft;
            case ZigZagMoveDirection.ToLeft:
            default:
                return ZigZagMoveDirection.ToPlayer;
        }
    }
}