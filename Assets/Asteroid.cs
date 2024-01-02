using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [Header("�÷��̾�� ������ �� ����")]
    //ũ�Ⱑ ũ�� ũ�⸦ ���� �ٿ��ְ� �µ��� ���� ������
    public int damage = 0; //�÷��̾��� ũ�⸦ �󸶳� �ٿ�����
    public int lowerTem = 0; //�÷��̾��� �µ��� �󸶳� �����ٰ���
    public int lowerSpeed = 0; //�÷��̾��� �ӵ��� �󸶳� �����ٰ���
    [Header("�� ����")]
    public int MaxSpeed; //�������� �ӵ�
    private Vector2 dir; //������ ����

    private float speed = 0;
    private void Start()
    {
        speed = Random.Range(0, (float)MaxSpeed); //�������� ���ǵ�
        dir = Random.insideUnitSphere * 1f; //���� ����
    }

    private void Update()
    {
        //�����̴� �ڵ�
        transform.Translate(dir.normalized * speed * Time.deltaTime);
        transform.Rotate(dir.normalized, speed * 50f * Time.deltaTime);
        //ȸ���ϴ� �ڵ�
    }
    private void OnCollisionEnter(Collision collision)
    {
        //�÷��̾��� �µ�, �ӵ��� �ٿ��ְ� ũ�⸦ �ٿ���
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
    }
}
