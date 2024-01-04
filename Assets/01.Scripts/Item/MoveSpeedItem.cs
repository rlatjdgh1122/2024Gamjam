using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedItem : SpawnObstacle
{
    public float duration = 3f;
    public float value = 3f;

    public ParticleLifeTimer _particle;
    public override void CollisonEvent(Collision player)
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager.Instance.GetPlayerMovement.SetSpeed(duration, value);
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
