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

        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //�÷��̾��� �µ�, �ӵ��� �ٿ��ְ� ũ�⸦ �ٿ���
            if (player.gameObject.TryGetComponent<PlayerManager>(out var Compo))
            {
                //Compo.GetMoveToForward;
            }
        }
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
        PoolManager.Instance.Push(this);
    }

    public override void Init()
    {
    }
}
