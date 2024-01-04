using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAliens : MonoBehaviour
{
    [SerializeField] Transform _firePos;
    [SerializeField] Tomato _bullet;

    [SerializeField]
    private float fireDistance;

    int cnt = 0;

    private bool canshoot = true;
    private bool canshoot2
    {
        get
        {
            if(cnt <= 300 && Vector3.Distance(PlayerManager.Instance.Player.transform.position, transform.position) <= fireDistance)
            {
                return true;
            }
            return false;
        }
    }


    private void Update()
    {
        if(canshoot && canshoot2)
        {
            StartCoroutine(EnemySpawn());
        }
    }

    private IEnumerator EnemySpawn()
    {
        cnt++;
        float randomX = Random.Range(-30f, 30f);
        float randomY = Random.Range(-15f, 15f);
        float randomZ = Random.Range(30f, 120f);
        Tomato bullet = PoolManager.Instance.Pop(_bullet.name) as Tomato;

        bullet.transform.position = _firePos.position;
        StartCoroutine(bullet.SetDir(new Vector3(randomX, randomY, randomZ).normalized));

        canshoot = false;
        yield return new WaitForSeconds(0.1f);
        canshoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireDistance);
    }
}
