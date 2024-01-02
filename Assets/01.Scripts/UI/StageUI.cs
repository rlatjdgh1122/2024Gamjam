using UnityEngine;
using DG.Tweening;

public class StageUI : MonoBehaviour
{
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _waitTime;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void EnableUI()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_canvasGroup.DOFade(1, _fadeTime));
        seq.AppendInterval(_waitTime);
        seq.Append(_canvasGroup.DOFade(0, _fadeTime));
    }
}
