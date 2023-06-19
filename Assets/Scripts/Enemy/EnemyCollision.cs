using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
   [SerializeField] private int decreasePlayerHealthBy; 
   
    private void OnTriggerEnter(Collider collision)
   {
      if (collision.gameObject.CompareTag("Player"))
         DataStorage.instance.DecreaseHealth(decreasePlayerHealthBy);
   }
}
