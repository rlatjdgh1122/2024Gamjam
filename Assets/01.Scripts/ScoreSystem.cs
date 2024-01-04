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

    private bool isOn = false;

    [SerializeField]
    private ParticleImage sizeParticleIMG;
    [SerializeField]
    private ParticleImage speedParticleIMG;

    private float speed;
    private float size;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ScorePopUpOnOff();
        }

    }

    public void ScorePopUpOnOff()
    {
        speed = PlayerManager.Instance.GetMoveToForward.MoveSpeed;
        size = PlayerManager.Instance.GetDurabilitySystem.DurabilityValue;

        PlayerManager.Instance.GetMoveToForward.enabled = false; //일단 임시로 여기서 끔

        if (isOn)
        {
            _scorePopUI.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCubic);
        }
        else
        {
            _scorePopUI.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                // if(만약 클리어라면)
                starIMGList[0].DOFillAmount(1, 2.0f).OnComplete(() =>
                {
                    StartCoroutine(SetStars());
                });
                // else //게임오버면
                // StartCoroutine(SetPlanetScale());
            });

        }
        isOn = !isOn;
    }

    private IEnumerator SetStars()
    {
        if (size >= 1)
        {
            FillStars(1, 1f);
        }
        else
        {
            FillStars(1, size);
        }

        sizeParticleIMG.rateOverLifetime = size * 10f;
        sizeParticleIMG.Play();

        yield return new WaitForSeconds(2f);

        if (speed >= 100)
        {
            FillStars(2, 1f);
        }
        else
        {
            FillStars(2, speed * 0.01f);
        }

        speedParticleIMG.rateOverLifetime = speed * 0.1f;
        speedParticleIMG.Play();
    }


    private void FillStars(int idx, float fillValue)
    {
        starIMGList[idx].DOFillAmount(fillValue, 2f);
    }
}
