using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance;


    [field: SerializeField]
    public int health
    {
        get;
        set;
    }

    public int lives
    {
        get;
        set;
    }

    public int level
    {
        get;
        set;
    }

    public int kills
    {
        get;
        private set;
    }

    public float playTime
    {
        get;
        private set;
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }

    public void IncreaseHealth(int increaseBy = 1)
    {
        health += increaseBy;
        // TODO: add the sounds for health increase
        // TODO: update the score in the UI
    }

    public void DecreaseHealth(int increaseBy = 1)
    {
        health -= increaseBy;
        // TODO: add the sounds for health increase
        // TODO: update the score in the UI
    }

    public void IncreaseKills(int increaseBy = 1)
    {
        kills += increaseBy;
        // TODO: add the sounds for score increase
        // TODO: update the score in the UI
    }

    public void DecreaseLives(int decreaseBy = 1)
    {
        lives -= decreaseBy;
    }

}
