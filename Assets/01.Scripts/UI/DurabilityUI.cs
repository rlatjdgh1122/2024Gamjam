using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityUI : MonoBehaviour
{
    private Image _fill;

    bool dead = false;

    private void Awake()
    {
        _fill = GetComponent<Image>();
    }

    public void IncreaseValue()
    {
        _fill.DOFillAmount(PlayerManager.Instance.GetDurabilitySystem.DurabilityValue * 0.75f, 0.75f);
    }

    private void Update()
    {
        if (PlayerManager.Instance.IsDie && !dead)
        {
            _fill.DOFillAmount(0, 0.5f);
            dead = true;
        }
    }
}
