using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolAbleParticle : PoolableMono
{
    private ParticleSystem _particle;
    private ParticleSystem.MainModule _mainModule;

    public override void Init()
    {
        if (isActiveAndEnabled)
        {
            _particle.Play();
            StartCoroutine(DestroyCorou());
        }
    }

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _mainModule = _particle.main;
    }

    private IEnumerator DestroyCorou()
    {
        yield return new WaitForSeconds(_mainModule.duration);
        PoolManager.Instance.Push(this);
    }

    public void SetStartLifetime(float startLifeTime)
    {
        _mainModule.startLifetime = startLifeTime;
    }
}
