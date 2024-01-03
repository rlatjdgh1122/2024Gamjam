using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Asteroid : SpawnObstacle
{
    public ParticleLifeTimer _particle;
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
            //if (player.transform.root.TryGetComponent<PlayerManager>(out var Compo))
            {
                //Compo.GetMoveToForward;
            }
            PlayerManager.Instance.GetMoveToForward.ApplySpeed(lowerSpeed);
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-0.2f);

            //카메라 쉐이크
            PlayerFollowCam.Instance.ShakeTest();

            //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
            var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
            obj.Setting(transform, size / 2f);

            PoolManager.Instance.Push(this);
        }
    }

    public override void Init()
    {
    }
}
