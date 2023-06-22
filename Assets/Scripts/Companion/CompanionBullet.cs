using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionBullet : MonoBehaviour
{
    [SerializeField] int speed = 25;


    public void ShootAtEnemy(GameObject target)
    {
        StartCoroutine(FlyTowards(target));
    }

    private IEnumerator FlyTowards(GameObject target)
    {
        var timePassed = 0f;
        var startPosition = transform.position;
        bool shouldFly = true;

        while (shouldFly)
        {
            transform.LookAt(target.transform.position);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            timePassed += Time.deltaTime;

            var distanceFromStartPos = Vector3.Distance(transform.position, startPosition);
            if (distanceFromStartPos > 200)
                shouldFly = false;

            yield return null;
        }
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            var enemyStats = other.GetComponentInParent<EnemyStats>();
            enemyStats.ReduceHealth(1);

            Destroy(gameObject);
        }
    }
}
