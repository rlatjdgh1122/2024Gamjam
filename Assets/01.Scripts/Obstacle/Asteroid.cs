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
        PoolManager.Instance.Push(this);
    }

    public override void Init()
    {
    }
}
