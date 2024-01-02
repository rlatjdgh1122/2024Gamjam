using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Obstacle : PoolableMono
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
}
