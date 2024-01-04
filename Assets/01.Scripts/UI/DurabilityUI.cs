using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityUI : MonoBehaviour
{
    private Image _fill;

    bool dead = false;

    Sequence fillSeq;

    private void Awake()
    {
        _fill = GetComponent<Image>();
        fillSeq = DOTween.Sequence();
    }

    public void IncreaseValue()
    {
        fillSeq.Append(_fill.DOFillAmount(PlayerManager.Instance.GetDurabilitySystem.DurabilityValue * 0.75f, 0.75f));
    }

    private void Update()
    {
        if (PlayerManager.Instance.IsDie && !dead)
        {
            fillSeq.Kill();
            fillSeq.Append(_fill.DOFillAmount(0, 0.5f));
            dead = true;
        }
    }
}
