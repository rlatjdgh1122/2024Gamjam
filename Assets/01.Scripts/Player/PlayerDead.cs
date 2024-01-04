using DG.Tweening;
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
        PlayerManager.Instance.GetMoveToForward.MoveSpeed = 0;
        PlayerManager.Instance.GetMoveToForward.IsSpeed = false;
    }

    private IEnumerator PlayParticle()
    {
        GameOver = true;
        yield return new WaitForSeconds(1f);
        PlayerFollowCam.Instance.ShakeCam(1f, 30);
        _visual.SetActive(false);
        _explosionParticle.Play();
        
        PlayerManager.Instance.GetMoveToForward.IsSpeed = false;

        yield return new WaitForSeconds(2f);
        //ScoreSystem.Instance.ScorePopUpOnOff();
        yield return new WaitForSeconds(1f);
        DOTween.To(() => GameManager.Instance.TimeI, s => GameManager.Instance.TimeI = s, 0, 3f);
        yield return new WaitForSeconds(2f);
        DOTween.To(() => PlayerManager.Instance.GetDurabilitySystem.DurabilityValue, s => PlayerManager.Instance.GetDurabilitySystem.DurabilityValue = s, 0, 3f);
        yield return new WaitForSeconds(2f);
        DOTween.To(() => PlayerManager.Instance.GetMoveToForward.MoveSpeed, s => PlayerManager.Instance.GetMoveToForward.MoveSpeed = s, 0, 3f);
        Invoke("ShowPanel", 1.5f);
    }

    private void ShowPanel()
    {
        UIManager.Instance.DieReasonUI.UpdateIMG(DieReasonType.FireDead);
    }
}
