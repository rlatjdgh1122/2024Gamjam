using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : Obstacle
{
    protected override void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        speed = MaxSpeed; //랜덤으로 스피드
        dir = Vector3.forward;
    }
    protected override void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
    public override void CollisonEvent(Collision player)
    {
        throw new System.NotImplementedException();
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }
}
