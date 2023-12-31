using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool shooting = false;

    [SerializeField] private int damageModifierTimeMs = 250;
    [SerializeField] private Animator _animator;

    
    private bool _isEnemyInCollider = false;

    private List<GameObject> colliderItems = new List<GameObject>();
    
    private float _nextUpdate = 0.25f;

    private ParticleSystem _particleSystem;
    private uint soundPlayEventID;

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
            AkSoundEngine.StopPlayingID(soundPlayEventID);
            _animator.SetBool("idle", true);
            _animator.SetBool("attack", false);
            _particleSystem.Stop();
        }
        else
        {
            shooting = true;
            soundPlayEventID = AkSoundEngine.PostEvent("Play_flamethrower", gameObject);
            _animator.SetBool("idle", false);
            _animator.SetBool("attack", true);
            _particleSystem.Play();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (shooting && other.transform.CompareTag("Enemy"))
        {
            Debug.Log("Adding enemy");
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
                Debug.Log("Reducing health");
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
