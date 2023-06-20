using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool shooting = false;

    [SerializeField] private int damageModifierTimeMs = 250;
    private bool _isEnemyInCollider = false;

    private List<GameObject> colliderItems = new List<GameObject>();
    
    private float _nextUpdate = 0.25f;

    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        if (_particleSystem == null) _particleSystem = new ParticleSystem();
        else _particleSystem.Stop();
    }

    void OnShoot()
    {
        if (shooting)
        {
            shooting = false;
            _particleSystem.Stop();
        }
        else
        {
            shooting = true;
            _particleSystem.Play();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (shooting && other.transform.CompareTag("Enemy"))
        {
            colliderItems.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        colliderItems.Remove(other.gameObject);
    }

    private void ReduceHealthEnemies()
    {
        List<GameObject> itemsToRemove = new List<GameObject>();
        foreach (GameObject enemy in colliderItems)
        {
            if (!enemy.IsDestroyed())
            {
                var enemyStats = enemy.GetComponentInParent<EnemyStats>();
                enemyStats.ReduceHealth(2);
            }
            else itemsToRemove.Add(enemy);
        }
        
        // remove the items that are necessary to be removed. 
        foreach (GameObject removeItem in itemsToRemove)
        {
            colliderItems.Remove(removeItem);
        }
    }

    private void Update()
    {
        
        if(Time.time>=_nextUpdate){
            //Debug.Log(Time.time+">="+_nextUpdate);
            // Change the next update (current second+1)
            _nextUpdate=Time.time+0.25f;
            // Call your fonction
            ReduceHealthEnemies();
            
        }
    }
}
