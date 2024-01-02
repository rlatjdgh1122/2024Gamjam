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
            //플레이어의 온도, 속도를 줄여주고 크기를 줄여줌
            if (player.transform.TryGetComponent<PlayerManager>(out var Compo))
            {
                //Compo.GetMoveToForward;
                Compo.GetMoveToForward.ApplySpeed(lowerSpeed);
            }

            PoolManager.Instance.Push(this);
        }
        //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
    }

    public override void Init()
    {
    }
}
