using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : SpawnObstacle
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
        //�÷��̾��� ũ�⸦ �ٿ���
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }
}
