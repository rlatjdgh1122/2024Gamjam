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
            if (player.transform.TryGetComponent<PlayerManager>(out var Compo))
            {
                //Compo.GetMoveToForward;
                Compo.GetMoveToForward.ApplySpeed(lowerSpeed);
            }

            PoolManager.Instance.Push(this);
        }
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
    }

    public override void Init()
    {
    }
}
