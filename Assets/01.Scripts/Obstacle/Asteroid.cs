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
            //�÷��̾��� �µ�, �ӵ��� �ٿ��ְ� ũ�⸦ �ٿ���
            //if (player.transform.root.TryGetComponent<PlayerManager>(out var Compo))
            {
                //Compo.GetMoveToForward;
            }
            //
            PlayerManager.Instance.GetMoveToForward.ApplySpeed(lowerSpeed); // 50 - 10 = 40

            float decreaseValue = PlayerManager.Instance.GetBuringSystem.BurningValue / (PlayerManager.Instance.GetMoveToForward.MoveSpeed / lowerSpeed);
            PlayerManager.Instance.GetBuringSystem.DecreaseBurningValue(decreaseValue); // 100 - 20 = 80

            //ī�޶� ����ũ
            PlayerFollowCam.Instance.ShakeTest();

            //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
            var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
            obj.Setting(transform, size / 2f);

            PoolManager.Instance.Push(this);
        }
    }

    public override void Init()
    {
    }
}
