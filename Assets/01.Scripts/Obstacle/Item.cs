using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : SpawnObstacle
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
       this.gameObject.SetActive(false);
        //�÷��̾��� ü��(ũ��)�� �÷��ٰԿ�
    }

    public override void Init()
    {

    }
}
