using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    
    private void Awake()
    {
        _animator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
