using System;
using System.Collections;
using UnityEngine;

public class BuringSystem : MonoBehaviour
{
    private bool wasDetectLastFrame = true;
    private bool IsDetectObstacle;

    ParticleSystem _fireParticle;

    [SerializeField]
    private PoolAbleParticle fireParticle;

    [SerializeField]
    private float overlapSphereRadius;

    [SerializeField]
    private float maxBurningValue = 2;
    public float MaxBurningValue => maxBurningValue;
    [SerializeField]
    private float burningValue;
    public float BurningValue
    {
        get
        {
            return burningValue;
        }
        set
        {
            burningValue = Mathf.Clamp(value, 0, maxBurningValue);
        }
    }

    private Coroutine burningCoroutine = null;

    private void Update()
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
        if(IsDetectObstacle)
        {
            IsDetectObstacle = false;
        }
    }

    private IEnumerator BuringCorou()
    {
        while(BurningValue <= maxBurningValue)
        {
            BurningValue += 0.1f;

            if(BurningValue >= 3.0f)
            {
                PoolAbleParticle particle = PoolManager.Instance.Pop(fireParticle.name) as PoolAbleParticle;
                _fireParticle = particle.GetComponent<ParticleSystem>();
                particle.gameObject.transform.SetParent(transform, false);
                particle.SetStartLifetime((maxBurningValue - BurningValue) * 0.1f);
            }

            yield return new WaitForSeconds(0.1f);
        }

        burningCoroutine = null;
    }

    public void DecreaseBurningValue(float minusValue)
    {
        BurningValue -= minusValue;
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
