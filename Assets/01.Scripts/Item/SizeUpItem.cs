using UnityEngine;

public class SizeUpItem : SpawnObstacle
{
    [Header("아이템 스탯")]
    public float UpperSize = 1f;
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
            //플레이어 속도를 줄여줌
            PlayerManager.Instance.GetMoveToForward.ApplySpeed(lowerSpeed);
            //플레이어 크기를 늘려줌
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(10f);

            //파티클 할가말가
            // var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
            //obj.Setting(transform, size / 2f);

            PoolManager.Instance.Push(this);
        }
        //플레이어의 체력(크기)을 늘려줄게요
    }

    public override void Init()
    {

    }
}
