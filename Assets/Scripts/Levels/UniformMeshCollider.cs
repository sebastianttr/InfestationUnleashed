using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformMeshCollider : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private MeshFilter _meshFilter;

    [SerializeField] private bool _isTrigger = false;
    
    private void Awake()
    {
        // create a new mesh collider instance
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _meshFilter = GetComponent<MeshFilter>();
        _boxCollider.isTrigger = _isTrigger;
    }
}
