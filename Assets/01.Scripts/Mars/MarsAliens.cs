using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAliens : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] Transform FirePos;
    [SerializeField] GameObject Bullet;
    [SerializeField] float TargetDistance = 100f;
    private float DistanceToTarget;

    bool Canshoot = true;

    private void Update()
    {
        DistanceToTarget = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceToTarget <= TargetDistance && Canshoot)
        {
            StartCoroutine(EnemySpawn());
        }

        Vector3 directionToTarget = Target.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3);
    }
    
    IEnumerator EnemySpawn()
    {
        Instantiate(Bullet, FirePos.transform.position, Quaternion.identity);

        Canshoot = false;
        yield return new WaitForSeconds(2f);
        Canshoot = true;
    }
}
