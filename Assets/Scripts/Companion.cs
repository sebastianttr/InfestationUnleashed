using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Companion : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Vector3 _lastPosition;
    private bool isMoving = true; 

    private void Awake()
    {
        _lastPosition = transform.position;
        _animator.SetBool("Idle", true);
    }

    private void CheckMovement()
    {
        float velocity = (transform.position-_lastPosition).magnitude;
        isMoving = velocity > 0.005f * 0.005f;
        _lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RunAnimationStates();
    }

    private void FixedUpdate()
    {
        CheckMovement();
    }


    private void RunAnimationStates()
    {
        if (isMoving)
        {
            _animator.SetBool("Idle",false);
            _animator.SetBool("Walk",true);
            _animator.SetBool("IdleToWalk", true);
        }
        else
        {
            _animator.SetBool("Idle",true);
            _animator.SetBool("Walk",false);
            _animator.SetBool("IdleToWalk", false);
        }
    }
}
