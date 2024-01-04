using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurningSystem : MonoBehaviour
{
    private bool wasDetectLastFrame = true;
    private bool IsDetectObstacle;

    public bool CanFire = false;

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

        if (PlayerManager.Instance.GetMoveToForward.MoveSpeed >= (PlayerManager.Instance.GetMoveToForward.MaxSpeed * 0.7f))
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
        float elapsedTime = 0f;

        while (burningValue <= maxBurningValue)
        {
            elapsedTime += Time.deltaTime;

            burningValue += increaseValue * Time.deltaTime;

            if (canShotParticle)
            {
                ShotParticle();
            }

            yield return null; // or yield return new WaitForEndOfFrame(); if you want to sync with the end of the frame
        }
    }

    private void ShotParticle()
    {
        PoolAbleParticle particle = PoolManager.Instance.Pop(fireParticle.name) as PoolAbleParticle;
        _fireParticleList.Add(particle.GetComponent<ParticleSystem>());
        particle.gameObject.transform.SetParent(transform, false);
        particle.SetStartLifetime(burningValue * firstSettingValue);
    }

    public void DecreaseBurningValue(float minusValue)
    {
        burningValue -= minusValue;
        for(int i = 0; i < _fireParticleList.Count; i++)
        {
            _fireParticleList[i]?.Stop();
        }
        _fireParticleList.Clear();
        canShotParticle = false;
    }

    public void SetDetectObstacle()
    {
        IsDetectObstacle = true;
    }

    public void SetShotParticle()
    {
        canShotParticle = true;
    }
}
