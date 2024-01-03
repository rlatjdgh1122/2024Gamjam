using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarUI : MonoBehaviour
{
    [SerializeField] private Color _overheatColor;
    [SerializeField] private TextMeshProUGUI _wariningText;

    private Image _bar;

    bool changeColor = true;

    private void Awake()
    {
        _bar = GetComponent<Image>();
    }

    private void Update()
    {
        _bar.fillAmount = PlayerManager.Instance.GetMoveToForward.MoveSpeed / 100;

        if (_bar.fillAmount >= 0.75f && changeColor)
        {
            _bar.DOColor(_overheatColor, 2.5f);
            changeColor = false;
        }
        
        if (_bar.fillAmount < 0.75f)
        {
            changeColor = true;
        }    

        if (_bar.fillAmount >= 1.0f)
        {
            _wariningText.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }
    }
}
