using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Asteroid : SpawnObstacle
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void CollisonEvent(Collision player)
    {
        //�÷��̾��� �µ�, �ӵ��� �ٿ��ְ� ũ�⸦ �ٿ���
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }
}
