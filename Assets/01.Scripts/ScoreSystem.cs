using AssetKits.ParticleImage;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("ScoreSystem is Multiple");
        }
        Instance = this;
    }

    [SerializeField]
    private RectTransform _scorePopUI;

    [SerializeField]
    private List<Image> starIMGList = new List<Image>();

    [SerializeField]
    private List<ParticleSystem> sparkleTrm = new List<ParticleSystem>();

    private bool isOn = false;

    [SerializeField]
    private ParticleImage sizeParticleIMG;
    [SerializeField]
    private ParticleImage speedParticleIMG;
    [SerializeField]
    private ParticleImage timeParticleIMG;  
    private float speed;
    private float size;
    private float time;

    public void ScorePopUpOnOff()
    {
        speed = PlayerManager.Instance.GetMoveToForward.MoveSpeed;
        size = PlayerManager.Instance.GetDurabilitySystem.DurabilityValue;
        time = GameManager.Instance.curTime;

        PlayerManager.Instance.GetMoveToForward.enabled = false; //일단 임시로 여기서 끔

        if (isOn)
        {
            _scorePopUI.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCubic);
        }
        else
        {
            _scorePopUI.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                StartCoroutine(SetStars());
            });

        }
        isOn = !isOn;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ScorePopUpOnOff();
        }
    }

    private IEnumerator SetStars()
    {
        timeParticleIMG.rateOverLifetime = (time / 60) * 0.1f;
        timeParticleIMG.Play();
        if (time <= 180f)
        {
            FillStars(0, 1f);
        }
        else if(time >= 780f)
        {
            FillStars(0, 0.1f);
        }
        else
        {
            FillStars(0, (time / 60) * 0.1f);
        }

        yield return new WaitForSeconds(2f);

        sizeParticleIMG.rateOverLifetime = size * 10f;
        sizeParticleIMG.Play();
        if (size >= 1)
        {
            FillStars(1, 1f);
        }
        else
        {
            FillStars(1, size);
        }

        yield return new WaitForSeconds(2f);

        speedParticleIMG.rateOverLifetime = speed * 0.1f;
        speedParticleIMG.Play();
        if (speed >= 100)
        {
            FillStars(2, 1f);
        }
        else
        {
            FillStars(2, speed * 0.01f);
        }
    }


    private void FillStars(int idx, float fillValue)
    {
        starIMGList[idx].DOFillAmount(fillValue, 2f);
    }
}
