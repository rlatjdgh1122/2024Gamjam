using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticleEffect : MonoBehaviour
{
    public int Count = 10;
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        Apply();
    }
    public void Apply()
    {
        var ps = _ps.main;
        ps.maxParticles = (int)PlayerManager.Instance.GetMoveToForward.MoveSpeed * Count;
    }
}
