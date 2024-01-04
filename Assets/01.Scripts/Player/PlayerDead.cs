using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] private float _shakeValue;
    [SerializeField] private float _shakeDelay;
    [SerializeField] private GameObject _visual;

    public bool GameOver;
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
        if (PlayerManager.Instance.IsDie && !GameOver)
        {
            _particleSystem.Stop();

            StartCoroutine(PlayParticle());
        }
    }

    public void DeadImmedieatly()
    {
        GameOver = true;
        PlayerFollowCam.Instance.ShakeCam(1f, 30);
        _visual.SetActive(false);
        _explosionParticle.Play();
    }

    private IEnumerator PlayParticle()
    {
        GameOver = true;
        yield return new WaitForSeconds(1f);
        PlayerFollowCam.Instance.ShakeCam(1f, 30);
        _visual.SetActive(false);
        _explosionParticle.Play();

        ScoreSystem.Instance.ScorePopUpOnOff();
    }
}
