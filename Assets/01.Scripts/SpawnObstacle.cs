
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnObstacle : PoolableMono  
{
    [Header("�÷��̾�� ������ �� ����")]
    //ũ�Ⱑ ũ�� ũ�⸦ ���� �ٿ��ְ� �µ��� ���� ������
    public int damage = 0; //�÷��̾��� ũ�⸦ �󸶳� �ٿ�����
    public int lowerTem = 0; //�÷��̾��� �µ��� �󸶳� �����ٰ���
    public int lowerSpeed = 0; //�÷��̾��� �ӵ��� �󸶳� �����ٰ���
    [Header("�� ����")]
    public int MaxSpeed; //�������� �ӵ�
    protected Vector3 dir; //������ ����

    protected float speed = 0;

    [Header("���� ����")]
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

        speed = Random.Range(0, (float)MaxSpeed); //�������� ���ǵ�
        dir = Random.insideUnitSphere * 1f; //���� ����
    }
    protected virtual void Update()
    {
        //�����̴� �ڵ�
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        //ȸ���ϴ� �ڵ�
        transform.Rotate(dir.normalized, (speed + 1f * 50f) * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //�÷��̾��� �µ�, �ӵ��� �ٿ��ְ� ũ�⸦ �ٿ���
        CollisonEvent(collision);
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
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
