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


            PlayerManager.Instance.GetBuringSystem.SetFalseVi();

            GameManager.Instance.isPlaying = false;

            if (_particle != null)
            {
                ScoreSystem.Instance.ScorePopUpOnOff();
                var obj = PoolManager.Instance.Pop(_particle.name) as ParticleLifeTimer;
                PlayerFollowCam.Instance.ShakeCam(.8f, 50f);
                obj.Setting(collision.contacts[0].point, 1200f);
            }
            else
            {
                // ���Ⱑ ���� �ƴѰ� ���⼭ ��� �߰� �ϸ� ��
                //�ڿ��� �װɷ�
                UIManager.Instance.DieReasonUI.UpdateIMG(DieReasonType.FireDead);
            }
        }
    }
}
