using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarUI : MonoBehaviour
{
    [SerializeField] private Color _overheatColor;
    [SerializeField] private TextMeshProUGUI _warningText;

    private Image _bar;
    private Sequence _warningSeq;

    bool changeColor = true;
    bool showWarning = true;

    private void Awake()
    {
        _bar = GetComponent<Image>();
        _warningSeq = DOTween.Sequence();
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
            _bar.DOColor(Color.white, 1.0f); 
            changeColor = true;
        }

        if (_bar.fillAmount >= 1.0f && showWarning)
        {
            _warningSeq.Append(_warningText.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo));
            showWarning = false;
        }

        if (_bar.fillAmount < 1.0f)
        {
            _warningSeq.Append(_warningText.DOFade(0, 0.5f));
            _warningSeq.Kill();
        }

        if (Input.GetKeyDown(KeyCode.G)) //디버그용
        {
            PlayerManager.Instance.GetMoveToForward.IsSpeed = true;
            PlayerManager.Instance.GetMoveToForward.MoveSpeed -= 20f;
        }
    }
}
