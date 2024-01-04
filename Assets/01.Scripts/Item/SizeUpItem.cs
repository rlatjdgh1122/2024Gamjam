using UnityEngine;

public class SizeUpItem : SpawnObstacle
{
    [Header("������ ����")]
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
            //�÷��̾� �ӵ��� �ٿ���
            PlayerManager.Instance.GetMoveToForward.ApplySpeed(lowerSpeed);
            //�÷��̾� ũ�⸦ �÷���
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(10f);

            //��ƼŬ �Ұ�����
            // var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
            //obj.Setting(transform, size / 2f);

            PoolManager.Instance.Push(this);
        }
        //�÷��̾��� ü��(ũ��)�� �÷��ٰԿ�
    }

    public override void Init()
    {

    }
}
