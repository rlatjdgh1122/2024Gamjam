using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurningSystem : MonoBehaviour
{
    public bool CanFire = false;

    [SerializeField]
    private PoolAbleParticle fireParticle;

    [SerializeField]
    private float overlapSphereRadius;

    [SerializeField]
    private float settingValue = 0.5f;
    [SerializeField]
    private float increaseValue;
    [SerializeField]
    private float maxBurningValue = 2;  
    public float MaxBurningValue => maxBurningValue;

    private List<ParticleSystem> _fireParticleList = new List<ParticleSystem>();

    private bool canShotParticle = false;


    private float burningValue;
    public float BurningValue
    {
        get
        {
            return burningValue;
        }
        private set { }
    }

    float timer = 0;
    bool sound = false;

    [SerializeField] private float _effectTimer = 0.5f;
    private float _increaseValue = 0;

    private void Update()
    {

        if (CanFire)
        {
            burningValue = PlayerManager.Instance.GetMoveToForward.MoveSpeed * 0.03f;
            _increaseValue += Time.deltaTime * settingValue;

            if (timer >= _effectTimer)
            {
                ShotParticle(_increaseValue);
                timer = 0;
            }

            timer += Time.deltaTime * increaseValue;
        }

        if (PlayerManager.Instance.GetMoveToForward.MoveSpeed >= (PlayerManager.Instance.GetMoveToForward.MaxSpeed * 0.6f))
        {
            CanFire = true;
        }
        else
        {
            CanFire = false;
        }
    }

    private void ShotParticle(float value)
    {
        PoolAbleParticle particle = PoolManager.Instance.Pop(fireParticle.name) as PoolAbleParticle;
        _fireParticleList.Add(particle.GetComponent<ParticleSystem>());
        particle.gameObject.transform.SetParent(transform, false);
        particle.SetStartLifetime(value);
    }

    public void DecreaseBurningValue()
    {
        _increaseValue = 0;
    }

    public void SetShotParticle()
    {
        canShotParticle = true;
    }
}
