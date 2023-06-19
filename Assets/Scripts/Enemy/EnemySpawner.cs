using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct StageSpawnSets
{
    public Transform[] spawnPoints;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private StageSpawnSets[] stageSpawnSets;

    private bool waveOneSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies(0, 8);
        BoxCollider bc = enemyPrefab.GetComponent<BoxCollider>();

        if (bc != null)
        {
            Debug.Log("Not null");
            Debug.Log(bc.size.y);
            //return rectTransform.rect;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var roundedTime = Math.Round(DataStorage.instance.playTime, 0);
        if (roundedTime == 20 && !waveOneSpawned)
        {
            SpawnEnemies(0, 4);
            Debug.Log("New wave spawned");
            waveOneSpawned=true;
        }
    }

    private void SpawnEnemies(int levelIndex, int enemyNumber)
    {
        for(int i = 0; i < enemyNumber; i++)
        {
            int randomNumber = Mathf.RoundToInt(UnityEngine.Random.Range(0f, stageSpawnSets[levelIndex].spawnPoints.Length - 1));

            Vector3 enemyPosition = stageSpawnSets[levelIndex].spawnPoints[randomNumber].transform.position;
            
            // adjust the height of the spawned enemies. 
            BoxCollider bc = enemyPrefab.GetComponent<BoxCollider>();
            enemyPosition.y += bc.size.y;
            
            Instantiate(
                enemyPrefab, 
                enemyPosition, 
                Quaternion.identity
            );

            
        }
    }
}
