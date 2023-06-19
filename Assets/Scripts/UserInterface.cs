
using System;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Image healthCircle;

    private void Awake()
    {

    }

    void Update()
    {
        float healthPercent = DataStorage.instance.health / 100f;

        healthCircle.rectTransform.localScale = new Vector3(1, healthPercent, 1);
    }
}
