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

    public void SetStartLifetime(float value)
    {
        _mainModule.startSize = value;
        _mainModule.startLifetime = 0.04f;
    }

    public void SetSpaceShipSetting(float value)
    {
        _mainModule.startLifetime = value;
        _mainModule.startSize = 4f;
    }
}
