using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    ParticleSystem _fireParticle;
    [SerializeField]
    private PoolAbleParticle fireParticle;

    [SerializeField] private Transform firePos;

    private Transform _target;

    bool IsLaunch = false;

    private void Awake()
    {
        _target = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _target.position) <= 900 && !IsLaunch)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, 3.5f);
            transform.Rotate(-_target.transform.position, 2);
            ShotParticle();
            if (Vector3.Distance(transform.position, _target.position) <= 50)
                IsLaunch = true;
        }   
    }

    private void ShotParticle()
    {
        PoolAbleParticle particle = PoolManager.Instance.Pop(fireParticle.name) as PoolAbleParticle;
        _fireParticle = particle.GetComponent<ParticleSystem>();
        particle.gameObject.transform.SetParent(firePos, false);
        particle.SetSpaceShipSetting(3);
    }
}
