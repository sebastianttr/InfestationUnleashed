using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

[Serializable]
public struct Wave
{
    public int numberOfEnemies;
    public int waveInterval;
}

[Serializable]
public struct StageSpawnSets
{
    public Transform[] spawnPoints;
    public Wave[] waves;
    public int stageInterval;
}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;

    [SerializeField] private StageSpawnSets[] stageSpawnSets;

    private int currentLevel = 0;
    private int currentWave = 0;

    private int spawnIntervalSum = 0;
    private int currentInterval = 0;
    private int currentNumberOfEnemies = 0;

    private double lastSecond;

    // Start is called before the first frame update
    private void Start()
    {
        //SpawnEnemies(0, 8);
        Debug.Log("Spawning wave -1");
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentLevel > stageSpawnSets.Length - 1)
            return;

        double roundedTime = Math.Round(DataStorage.instance.playTime, 0);

        currentInterval = stageSpawnSets[currentLevel].waves[currentWave].waveInterval;
        currentNumberOfEnemies = stageSpawnSets[currentLevel].waves[currentWave].numberOfEnemies;

        if (roundedTime == currentInterval + spawnIntervalSum && roundedTime != lastSecond)
        {
            Debug.Log("current level = " + (currentLevel + 1) + " wave = " + (currentWave + 1));

            lastSecond = roundedTime;
            spawnIntervalSum += currentInterval;

            SpawnEnemies(currentLevel, currentNumberOfEnemies);
            currentWave++;
            if (currentWave > stageSpawnSets[currentLevel].waves.Length - 1)
            {
                currentLevel++;
                currentWave = 0;
            }
        }
    }

    private void SpawnEnemies(int levelIndex, int enemyNumber)
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            int randomNumber = Mathf.RoundToInt(UnityEngine.Random.Range(0f, stageSpawnSets[levelIndex].spawnPoints.Length - 1));

            bool isRat = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f)) >= 0.5f;
            
            Vector3 enemyPosition = stageSpawnSets[levelIndex].spawnPoints[randomNumber].transform.position;

            GameObject prefabSelect = isRat ? enemyPrefab1 : enemyPrefab2;

            // adjust the height of the spawned enemies.
            BoxCollider bc = prefabSelect.GetComponent<BoxCollider>();
            enemyPosition.y += bc.size.y;

            Instantiate(
                prefabSelect,
                enemyPosition,
                Quaternion.identity
            );
        }
    }
}
