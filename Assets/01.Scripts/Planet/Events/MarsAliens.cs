using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAliens : MonoBehaviour
{
    private Transform _target;
    [SerializeField] Transform _firePos;
    [SerializeField] Tomato _bullet;
    private float distanceToTarget;

    [SerializeField]
    private float fireDistance;

    private Vector3 dir;

    private bool canshoot = true;

    private void Start()
    {
        _target = PlayerManager.Instance.Player.transform;
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);
        
        if(canshoot)
        {
            StartCoroutine(EnemySpawn());
        }
    }

    private IEnumerator EnemySpawn()
    {
        float randomX = Random.Range(-30f, 30f);
        float randomY = Random.Range(-15f, 15f);
        float randomZ = Random.Range(30f, 120f);
        float randomCool = Random.Range(0, 2f);
        Tomato bullet = PoolManager.Instance.Pop(_bullet.name) as Tomato;
        Debug.Log(randomY); 

        bullet.transform.position = _firePos.position;
        bullet.SetDir(new Vector3(randomX, randomY, randomZ).normalized);

        canshoot = false;
        yield return new WaitForSeconds(randomCool);
        canshoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireDistance);
    }
}
