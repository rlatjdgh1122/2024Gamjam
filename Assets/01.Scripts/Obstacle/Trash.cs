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
        //플레이어의 크기를 줄여줌
        //내 오브젝트는 죽고 부숴지는 파티클이 나와야함
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //내가 추가한거
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
