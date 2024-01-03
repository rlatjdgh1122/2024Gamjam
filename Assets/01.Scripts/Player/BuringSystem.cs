using System;
using System.Collections;
using UnityEngine;

public class BuringSystem : MonoBehaviour
{
    private bool wasDetectLastFrame = true;
    private bool IsDetectObstacle;

    public bool CanFire = false;

    ParticleSystem _fireParticle;

    [SerializeField]
    private PoolAbleParticle fireParticle;

    [SerializeField]
    private float overlapSphereRadius;

    [SerializeField]
    private float firstSettingValue = 0.5f;
    [SerializeField]
    private float increaseValue;
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private float maxBurningValue = 2;
    public float MaxBurningValue => maxBurningValue;
    
    private float burningValue;
    public float BurningValue
    {
        get
        {
            return burningValue;
        }
        private set { }
    }

    private Coroutine burningCoroutine = null;

    private void Update()
    {

        if (CanFire)
        {
            if (wasDetectLastFrame && !IsDetectObstacle)
            {
                burningCoroutine = StartCoroutine(BuringCorou());
            }

            if (IsDetectObstacle && burningCoroutine != null)
            {
                StopCoroutine(burningCoroutine);
            }

            wasDetectLastFrame = IsDetectObstacle;
            if (IsDetectObstacle)
            {
                IsDetectObstacle = false;
            }
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

    private IEnumerator BuringCorou()
    {
        while(burningValue <= maxBurningValue)
        {
            burningValue += increaseValue;

            if(burningValue >= 3.0f)
            {
                ShotParticle();
            }
            
            yield return new WaitForSeconds(waitTime);
        }

    }

    private void ShotParticle()
    {
        PoolAbleParticle particle = PoolManager.Instance.Pop(fireParticle.name) as PoolAbleParticle;
        _fireParticle = particle.GetComponent<ParticleSystem>();
        particle.gameObject.transform.SetParent(transform, false);
        particle.SetStartLifetime(burningValue * firstSettingValue);
    }

    public void DecreaseBurningValue(float minusValue)
    {
        burningValue -= minusValue;
        if(_fireParticle != null)
        {
            _fireParticle.Stop();
        }
    }

    public void SetDetectObstacle()
    {
        IsDetectObstacle = true;
    }
}
