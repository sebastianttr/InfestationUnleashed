using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAttack : MonoBehaviour
{
    [SerializeField] private GameObject companionBullet;
    [SerializeField] private int companionAttackInterval;
    [SerializeField] private float range = 5f;

    private bool enemyInRange = false;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject targetEnemy;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(CompanionShoot(companionAttackInterval));
    }

    // Update is called once per frame
    private void Update()
    {
        Collider[] enemiesHit = Physics.OverlapBox(transform.position, new Vector3(range, range, range), Quaternion.identity);
        foreach (Collider collider in enemiesHit)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemiesInRange.Add(collider.gameObject);
                //Debug.Log("enemy in range");
            }
        }

        if (enemiesInRange.Count > 0)
        {
            enemyInRange = true;
            targetEnemy = enemiesInRange[Mathf.RoundToInt(Random.Range(0f, enemiesInRange.Count - 1))];
        }
        else
            enemyInRange = false;
    }

    private IEnumerator CompanionShoot(int interval)
    {
        while (true)
        {
            if (enemyInRange)
            {
                Vector3 companionHead = transform.position;
                companionHead.y = 1f;
                GameObject bullet = Instantiate(companionBullet, companionHead, Quaternion.identity);
                CompanionBullet bulletScript = bullet.GetComponent<CompanionBullet>();
                if (bulletScript != null)
                {
                    bulletScript.ShootAtEnemy(targetEnemy);
                    Debug.Log("Shoot at enemy called");
                }
            }
            yield return new WaitForSeconds(interval);
        }
    }
}