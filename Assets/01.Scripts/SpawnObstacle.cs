
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnObstacle : PoolableMono  
{
    [Header("플레이어에게 영향을 줄 스탯")]
    //크기가 크면 크기를 많이 줄여주고 온도를 많이 낮줘줌
    public int damage = 0; //플레이어의 크기를 얼마나 줄여줄지
    public int lowerTem = 0; //플레이어의 온도를 얼마나 낮춰줄건지
    public int lowerSpeed = 0; //플레이어의 속도를 얼마나 낮춰줄건지
    [Header("내 스탯")]
    public int MaxSpeed; //움직임의 속도
    protected Vector3 dir; //움직일 방향

    protected float speed = 0;

    [Header("스폰 설정")]
    [SerializeField] private float _groundRadius;
    [SerializeField] private float _obstacleRadius;

    [SerializeField] private LayerMask _ground;
    [SerializeField] private LayerMask _obstacle;


    [SerializeField] private float _radius;

    public bool CanSpawn;
     protected virtual void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        speed = Random.Range(0, (float)MaxSpeed); //랜덤으로 스피드
        dir = Random.insideUnitSphere * 1f; //랜덤 방향
    }
    protected virtual void Update()
    {
        //움직이는 코드
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        //회전하는 코드
        transform.Rotate(dir.normalized, (speed + 1f * 50f) * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //플레이어의 온도, 속도를 줄여주고 크기를 줄여줌
        CollisonEvent(collision);
        //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
    }
    public abstract void CollisonEvent(Collision player);
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
