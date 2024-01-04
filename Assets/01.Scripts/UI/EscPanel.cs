using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EscPanel : MonoBehaviour
{
    [SerializeField] private float _backFadeDuration;
    private Image _backImg;
    private RectTransform _visualSetting;
    private RectTransform _setting;

    private CanvasGroup _canvasGroup;

    private float x = 100;
    private float y = 100;

    private Sequence _seq;

    private void Awake()
    {
        _seq = DOTween.Sequence();

        Transform esc = transform.Find("ESC");
        _backImg = esc.GetComponent<Image>();
        _visualSetting = esc.Find("VisualSetting").GetComponent<RectTransform>();
        _setting = esc.Find("Setting").GetComponent<RectTransform>();

        _canvasGroup = _setting.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowEsc();
    }

    public void ShowEsc()
    {
        if (_seq != null && _seq.IsActive())
            return;

        _seq = DOTween.Sequence();
        _seq.PrependCallback(() => _backImg.gameObject.SetActive(true))
            .Append(_backImg.DOFade(0.8f, _backFadeDuration))
            .AppendCallback(() => _visualSetting.gameObject.SetActive(true))
            .AppendCallback(() =>
                _visualSetting.DOSizeDelta(new Vector2(550, 600), _backFadeDuration))
            .AppendInterval(_backFadeDuration)
            .AppendCallback(() =>
            {
                _setting.gameObject.SetActive(true);
                _canvasGroup.alpha = 1f;
            })
            .OnComplete(() =>
            {
                _seq.Kill();
                Time.timeScale = 0;
                });
    }

    public void ResetESC()
    {
        if (_seq != null && _seq.IsActive())
            return;

        _seq = DOTween.Sequence();
        Time.timeScale = 1f;
        _seq.PrependCallback(() =>
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(0, _backFadeDuration);

            _setting.gameObject.SetActive(false);
        })
        .AppendInterval(_backFadeDuration)
        .AppendCallback(() =>
                _visualSetting.DOSizeDelta(new Vector2(x, y), _backFadeDuration))
        .AppendCallback(() => _visualSetting.gameObject.SetActive(false))
        .Append(_backImg.DOFade(0, _backFadeDuration))
        .AppendCallback(() =>
        {
            _backImg.gameObject.SetActive(false);
            })
        .OnComplete(() => _seq.Kill());
    }
}
