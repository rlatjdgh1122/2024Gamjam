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
            //if (player.transform.root.TryGetComponent<PlayerManager>(out var Compo))
            {
                //Compo.GetMoveToForward;
            }
            PlayerManager.Instance.GetMoveToForward.ApplySpeed(lowerSpeed);

            //민교가 추가한거 
            /* PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-0.2f);

            float decreaseValue = PlayerManager.Instance.GetBuringSystem.BurningValue /
                                  (PlayerManager.Instance.GetMoveToForward.MoveSpeed / lowerSpeed);
            PlayerManager.Instance.GetBuringSystem.DecreaseBurningValue(decreaseValue); */

            //해보고 더 나은거 ㄱㄱ

            //내가 추가한거
            PlayerManager.Instance.GetDurabilitySystem.ChangeValue(-damage);
            Debug.Log(damage);

            PlayerFollowCam.Instance.ShakeTest();
            var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
            obj.Setting(transform, size / 2f);

            PoolManager.Instance.Push(this);
        }
    }

    public override void Init()
    {
    }
}
