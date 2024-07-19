using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscPanel : MonoBehaviour
{
    [SerializeField] private float _backFadeDuration = 0.5f;

    private Image _backImage;
    private RectTransform _visualSetting;
    private RectTransform _setting;
    private CanvasGroup _volumeSetting;
    private CanvasGroup _canvasGroup;

    private readonly Vector2 _visualSettingInitialSize = new Vector2(100, 100);
    private readonly Vector2 _visualSettingOpenSize = new Vector2(550, 700);
    private readonly Vector2 _visualSettingExpandedSize = new Vector2(850, 600);

    private Sequence _sequence;
    private bool _isEscPanelActive = false;
    private bool _isVolumePanelActive = false;

    private void Awake()
    {
        _sequence = DOTween.Sequence();

        Transform esc = transform.Find("ESC");
        _backImage = esc.GetComponent<Image>();
        _visualSetting = esc.Find("VisualSetting").GetComponent<RectTransform>();
        _setting = esc.Find("Setting").GetComponent<RectTransform>();
        _volumeSetting = esc.Find("SoundSetting").GetComponent<CanvasGroup>();
        _canvasGroup = _setting.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isEscPanelActive)
            {
                ShowEscPanel();
            }
            else if (_isVolumePanelActive)
            {
                ResetSettingPanel();
            }
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowEscPanel()
    {
        if (_sequence.IsActive())
            return;

        _isEscPanelActive = true;

        _sequence = DOTween.Sequence();
        _sequence.PrependCallback(() => _backImage.gameObject.SetActive(true))
                 .Append(_backImage.DOFade(0.8f, _backFadeDuration))
                 .AppendCallback(() => _visualSetting.gameObject.SetActive(true))
                 .Append(_visualSetting.DOSizeDelta(_visualSettingOpenSize, _backFadeDuration))
                 .AppendInterval(_backFadeDuration)
                 .AppendCallback(() =>
                 {
                     _setting.gameObject.SetActive(true);
                     _canvasGroup.alpha = 1f;
                 })
                 .OnComplete(() =>
                 {
                     Time.timeScale = 0;
                     _sequence.Kill();
                 });
    }

    public void ContinueGame()
    {
        if (_sequence.IsActive())
            return;

        Time.timeScale = 1f;

        _sequence = DOTween.Sequence();
        _sequence.PrependCallback(() =>
        {
            _canvasGroup.alpha = 0f;
            _setting.gameObject.SetActive(false);
        })
                 .Append(_canvasGroup.DOFade(0, _backFadeDuration))
                 .AppendInterval(_backFadeDuration)
                 .Append(_visualSetting.DOSizeDelta(_visualSettingInitialSize, _backFadeDuration))
                 .AppendCallback(() => _visualSetting.gameObject.SetActive(false))
                 .Append(_backImage.DOFade(0, _backFadeDuration))
                 .AppendCallback(() => _backImage.gameObject.SetActive(false))
                 .OnComplete(() => _sequence.Kill());

        _isEscPanelActive = false;
    }

    public void OpenSettings()
    {
        _visualSetting.DOSizeDelta(_visualSettingExpandedSize, _backFadeDuration);
        _volumeSetting.gameObject.SetActive(true);
        _setting.gameObject.SetActive(false);
        _isVolumePanelActive = true;
    }

    public void ResetSettingPanel()
    {
        _isVolumePanelActive = false;
        _visualSetting.DOSizeDelta(_visualSettingOpenSize, _backFadeDuration);
        _volumeSetting.gameObject.SetActive(false);
        _setting.gameObject.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Intro");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
