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

    [Header("RandomVec")]
    [SerializeField] float xMin, xMax;
    [SerializeField] float yMin, yMax;
    [SerializeField] float zMin, zMax;

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
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);
        float randomZ = Random.Range(zMin, zMax);
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
