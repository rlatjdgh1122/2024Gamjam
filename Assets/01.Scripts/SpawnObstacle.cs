
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class SpawnObstacle : PoolableMono
{
    public readonly int MaxDamage = 3;
    public readonly int MaxLowerTem = 3;
    public readonly int MaxLowerSpeed = 3;
    public readonly int MaxSize = 5;
    [Header("�÷��̾�� ������ �� ����")]
    //ũ�Ⱑ ũ�� ũ�⸦ ���� �ٿ��ְ� �µ��� ���� ������
    public int damage = 3; //�÷��̾��� ũ�⸦ �󸶳� �ٿ�����
    public int lowerTem = 3; //�÷��̾��� �µ��� �󸶳� �����ٰ���
    public int lowerSpeed = 3; //�÷��̾��� �ӵ��� �󸶳� �����ٰ���
    public int maxSize = 5;

    [Header("�� ����")]
    public int MaxSpeed; //�������� �ӵ�
    protected Vector3 dir; //������ ����

    protected float speed = 0;

    [Header("���� ����")]
    [SerializeField] private float _obstacleRadius;

    [SerializeField] private LayerMask _obstacle;

    public bool CanSpawn;
    protected virtual void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        speed = Random.Range(0, (float)MaxSpeed); //�������� ���ǵ�
        dir = Random.insideUnitSphere * 1f; //���� ����

        var size = Random.Range(1f, MaxSize);
        transform.localScale = new Vector3(size, size, size);

        damage = (int)size * MaxDamage;
        lowerTem = (int)size * MaxLowerTem;
        lowerSpeed = (int)size * MaxLowerSpeed;
    }
    protected virtual void Update()
    {
        //�����̴� �ڵ�
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        //ȸ���ϴ� �ڵ�
        transform.Rotate(dir.normalized, speed * 50f * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisonEvent(collision);
        //�÷��̾��� �µ�, �ӵ��� �ٿ��ְ� ũ�⸦ �ٿ���
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
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
