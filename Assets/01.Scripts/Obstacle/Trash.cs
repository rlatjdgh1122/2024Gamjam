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
        //플레이어의 크기를 줄여줌
        //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }
}
