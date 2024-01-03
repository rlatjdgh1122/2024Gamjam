using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAliens : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform FirePos;
    [SerializeField] GameObject Bullet;
    [SerializeField] float TargetDistance = 10f;
    private float DistanceToTarget;

    bool Canshoot = true;

    private void Update()
    {
        DistanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (DistanceToTarget <= TargetDistance && Canshoot)
        {
            StartCoroutine(EnemySpawn());
        }
    }
    
    IEnumerator EnemySpawn()
    {
        Instantiate(Bullet, FirePos.transform.position, Quaternion.identity);

        Canshoot = false;
        yield return new WaitForSeconds(2f);
        Canshoot = true;
    }
}
