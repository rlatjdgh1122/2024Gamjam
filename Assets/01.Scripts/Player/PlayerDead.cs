using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] private float _shakeValue;
    [SerializeField] private float _shakeDelay;
    [SerializeField] private GameObject _visual;

    private ParticleSystem _particleSystem;
    private ParticleSystem _explosionParticle;

    private void Awake()
    {
        _particleSystem = transform.Find("SpeedLineEffect").GetComponent<ParticleSystem>();
        _explosionParticle = transform.Find("Impact02").GetComponent<ParticleSystem>();


        _explosionParticle.Stop();
    }

    private void Update()
    {
        //if(PlayerManager.Instance.IsDie && !GameManager.Instance.GameOver)
        //{
        //    _particleSystem.Stop();

        //    StartCoroutine(PlayParticle());
        //}
    }

    private IEnumerator PlayParticle()
    {
        GameManager.Instance.GameOver = true;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
        PlayerFollowCam.Instance.ShakeCam(1f, 30);
        _visual.SetActive(false);
        _explosionParticle.Play();
    }
}
