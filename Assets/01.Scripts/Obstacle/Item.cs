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
        //플레이어의 체력(크기)을 늘려줄게요
    }

    public override void Init()
    {

    }
}
