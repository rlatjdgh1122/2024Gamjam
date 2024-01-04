
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class SpawnObstacle : ETC
{

    [Header("플레이어에게 영향을 줄 스탯")]

    //크기가 크면 크기를 많이 줄여주고 온도를 많이 낮줘줌
    public float MaxDamage = 3; //플레이어의 크기를 얼마나 줄여줄지
    public int MaxLowerTem = 3; //플레이어의 온도를 얼마나 낮춰줄건지
    public int MaxLowerSpeed = 3; //플레이어의 속도를 얼마나 낮춰줄건지
    public float MaxSize = 8;
    public float MinSize = 3;

    protected float size = 0f;

    protected float damage = 3;
    protected int lowerTem = 3;
    protected int lowerSpeed = 3;
    protected int maxSize = 5;

    [Header("내 스탯")]
    public int MaxSpeed; //움직임의 속도
    protected Vector3 dir; //움직일 방향

    protected float speed = 0;

    [Header("스폰 설정")]
    [SerializeField] private float _obstacleRadius;

    [SerializeField] private LayerMask _obstacle;

    public bool CanSpawn;

    protected virtual void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        speed = Random.Range(0, (float)MaxSpeed); //랜덤으로 스피드
        dir = Random.insideUnitSphere * 1f; //랜덤 방향

        size = Random.Range(MinSize, MaxSize);
        transform.localScale = new Vector3(size, size, size);

        damage = (int)size * (MaxDamage / MaxSize);
        lowerTem = (int)size * MaxLowerTem;
        lowerSpeed = (int)size * MaxLowerSpeed * 3;
    }
    protected virtual void Update()
    {
        //움직이는 코드
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        //회전하는 코드
        transform.Rotate(dir.normalized, speed * 50f * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisonEvent(collision);
        //플레이어의 온도, 속도를 줄여주고 크기를 줄여줌
        //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
    }
    public abstract void CollisonEvent(Collision player);

    public void Spawn(Vector3 randomPos)
    {
        transform.position = randomPos;

        SpawnCheck();
    }

    public void SpawnCheck()
    {
        bool obstacle = false;

        Collider[] obstacleHit = Physics.OverlapSphere(transform.position,
            _obstacleRadius, _obstacle);

        for (int i = 0; i < obstacleHit.Length; i++)
        {
            obstacle = true;
        }

        if (!obstacle)
        {
            transform.gameObject.SetActive(true);
        }
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _obstacleRadius);
    }

#endif
}
