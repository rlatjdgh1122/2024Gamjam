using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscPanel : MonoBehaviour
{
    [SerializeField] private float _backFadeDuration;
    private Image _backImg;
    private RectTransform _visualSetting;
    private RectTransform _setting;

    private CanvasGroup _volumeSetting;
    private CanvasGroup _canvasGroup;

    private float x = 100;
    private float y = 100;

    private Sequence _seq;

    private bool _escPanel = false;
    private bool _volumePanel = false;

    private void Awake()
    {
        _seq = DOTween.Sequence();

        Transform esc = transform.Find("ESC");
        _backImg = esc.GetComponent<Image>();
        _visualSetting = esc.Find("VisualSetting").GetComponent<RectTransform>();
        _setting = esc.Find("Setting").GetComponent<RectTransform>();
        _volumeSetting = esc.Find("SoundSetting").GetComponent <CanvasGroup>();
        _canvasGroup = _setting.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!_escPanel)
            {
                ShowEsc();
            }
            if(_volumePanel)
            {
                ResetSetting();
            }
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }


    public void ShowEsc()
    {
        _escPanel = true;

        if (_seq != null && _seq.IsActive())
            return;

        _seq = DOTween.Sequence();
        _seq.PrependCallback(() => _backImg.gameObject.SetActive(true))
            .Append(_backImg.DOFade(0.8f, _backFadeDuration))
            .AppendCallback(() => _visualSetting.gameObject.SetActive(true))
            .AppendCallback(() =>
                _visualSetting.DOSizeDelta(new Vector2(550, 700), _backFadeDuration))
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

    public void Continue()
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

        _escPanel = false;
    }

    public void Setting()
    {
        _visualSetting.DOSizeDelta(new Vector2(850, 600), _backFadeDuration);
        _volumeSetting.gameObject.SetActive(true);
        _setting.gameObject.SetActive(false);
        _volumePanel = true;
    }
    public void ResetSetting()
    {
        _volumePanel = false;
        _visualSetting.DOSizeDelta(new Vector2(550, 700), _backFadeDuration);
        _volumeSetting.gameObject.SetActive(false);
        _setting.gameObject.SetActive(true);
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName.Intro);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
