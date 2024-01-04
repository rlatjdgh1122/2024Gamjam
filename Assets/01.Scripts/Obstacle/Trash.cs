using UnityEngine;

public class Trash : SpawnObstacle
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
        //�÷��̾��� ũ�⸦ �ٿ���
        //�� ������Ʈ�� �װ� �ν����� ��ƼŬ�� ���;���
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //���� �߰��Ѱ�
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-damage);
            Debug.Log(damage);

            PlayerFollowCam.Instance.ShakeCam(.3f, 1f);

            if (_particle != null)
            {
                var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
                obj.Setting(transform, size / 2f);
            }

            PoolManager.Instance.Push(this);
        }
    }

    public override void Init()
    {

    }
}
