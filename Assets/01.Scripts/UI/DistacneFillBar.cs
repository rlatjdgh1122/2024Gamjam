using DG.Tweening;
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

    public UnityEvent FillEvent;

    bool canFill = true;

    private void Awake()
    {
        _fillContainer = GetComponent<Image>();
    }

    private void Update()
    {
        float normalizedDistance = GameManager.Instance.distanceToEarth / 100.0f;
        float invertedFillAmount = 1.0f - normalizedDistance;

        _fillContainer.fillAmount = Mathf.Clamp01(invertedFillAmount);

        if (_fillContainer.fillAmount >= 0.111f && canFill)
        {
            FillEvent?.Invoke();
            canFill = false;
        }

        if (_fillContainer.fillAmount >= 0.333f && !canFill)
        {
            FillEvent?.Invoke();
            canFill = true;
        }

        if (_fillContainer.fillAmount >= 0.555f && canFill)
        {
            FillEvent?.Invoke();
            canFill = false;
        }

        if (_fillContainer.fillAmount >= 0.777f && !canFill)
        {
            FillEvent?.Invoke();
            canFill = true;
        }

        if (_fillContainer.fillAmount >= 1f && canFill)
        {
            FillEvent?.Invoke();
            canFill = false;
        }
    }

    public void FadeInCircleImage()
    {
        _circles[cnt].DOColor(_fillColor, 0.5f);
        cnt++;
    }
}
