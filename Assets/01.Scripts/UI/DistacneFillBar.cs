using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DistacneFillBar : MonoBehaviour
{
    private Image _fillContainer;

    [SerializeField] private Color _fillColor;
    [SerializeField] private Image[] _circles;
    int cnt = 0;

    private void Awake()
    {
        _fillContainer = GetComponent<Image>();
    }

    private void Update()
    {
        float normalizedDistance = GameManager.Instance.distanceToEarth / 100.0f;
        float invertedFillAmount = 1.0f - normalizedDistance;

        _fillContainer.fillAmount = Mathf.Clamp01(invertedFillAmount);

        float targetFill = (cnt + 1) / 6.0f;

        if (_fillContainer.fillAmount >= targetFill && cnt < 6)
        {
            FadeInCircleImage();
        }
    }

    public void FadeInCircleImage()
    {
        int prevCnt = cnt;
        _circles[cnt].DOColor(_fillColor, 0.5f);
        _circles[cnt].rectTransform.DOScale(1.25f, 0.5f);
        cnt++;
    }
}
