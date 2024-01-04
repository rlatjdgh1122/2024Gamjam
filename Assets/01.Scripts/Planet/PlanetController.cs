using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public ParticleLifeTimer _particle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerFollowCam.Instance.ShakeTest();

            SoundManager.Instance.PlaySFXSound(SFX.HeavyExplosion);

            if (_particle != null)
            {
                var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
                PlayerFollowCam.Instance.ShakeCam(.8f, 50f);
                obj.Setting(collision.contacts[0].point, 1200f);
            }
        }
    }
}
