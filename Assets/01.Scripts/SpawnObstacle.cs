
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : PoolableMono
{
    [SerializeField] private float _groundRadius;
    [SerializeField] private float _obstacleRadius;

    [SerializeField] private LayerMask _ground;
    [SerializeField] private LayerMask _obstacle;


    [SerializeField] private float _radius;

    public bool CanSpawn;

    public void Spawn()
    {
        transform.position = transform.position + Random.insideUnitSphere * _radius;

        SpawnCheck();
    }

    public void SpawnCheck()
    {
        bool ground = false;
        bool obstacle = false;

        Collider[] groundHit = Physics.OverlapSphere(transform.position, _groundRadius, _ground);
        Collider[] obstacleHit = Physics.OverlapSphere(transform.position, _groundRadius, _obstacle);

        for (int i = 0; i < groundHit.Length; i++)
        {
            ground = true;
        }
        for (int i = 0; i < obstacleHit.Length; i++)
        {
            obstacle = true;
        }

        if (!ground && !obstacle)
        {
            transform.gameObject.SetActive(true);
        }
    }
    public override void Init()
    {

    }



#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _groundRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _obstacleRadius);
    }

#endif
}
