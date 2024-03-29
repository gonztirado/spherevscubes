﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class JumpEnemy : Enemy
{
    [Header("Jump settings")] public float jumpProbability;
    public float jumpHeight;
    public float jumpTime;


    private float _currentJumpHeight;
    private bool _isJumpingUp;
    private bool _isJumpingDown;

   
    protected void OnEnable()
    {
        base.OnEnable();
        _currentJumpHeight = 0;
        _isJumpingUp = false;
        _isJumpingDown = false;
    }
    

    public override void Move()
    {
        if (_isJumpingUp)
            JumpUp();
        else if (_isJumpingDown)
            JumpDown();
        else
            ChooseJumpOrMove();
        
        if(transform.position.y < 0)
           transform.position = new Vector3(transform.position.x, 0, transform.position.z); 
    }


    private void ChooseJumpOrMove()
    {
        bool canJump = Random.Range(0f, 1f) < jumpProbability;
        if (canJump)
            JumpUp();
        else
            base.Move();
    }

    private void JumpUp()
    {
        _isJumpingUp = true;
        float currentJump = jumpHeight * Time.deltaTime / jumpTime;
        if (_currentJumpHeight + currentJump > jumpHeight)
            currentJump = jumpHeight - _currentJumpHeight;
        
        _currentJumpHeight += currentJump;
        transform.Translate(Vector3.up * currentJump);
        
        if (_currentJumpHeight >= jumpHeight)
        {
            _isJumpingUp = false;
            _isJumpingDown = true;
        }
    }

    private void JumpDown()
    {
        float currentJump = jumpHeight * Time.deltaTime / jumpTime;
        if (_currentJumpHeight - currentJump < 0)
            currentJump = _currentJumpHeight;
        
        transform.Translate(Vector3.down * currentJump);
        _currentJumpHeight -= currentJump;

        if (_currentJumpHeight <= 0)
        {
            _isJumpingDown = false;
            _currentJumpHeight = 0;
        }
    }
}