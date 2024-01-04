using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    private RectTransform _scorePopUI;

    private bool isOn = false;

    private PlayerFowardMover _playerFowardMover => PlayerManager.Instance.GetMoveToForward;
    private DurabilitySystem _durabilitySystem => PlayerManager.Instance.GetDurabilitySystem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ScorePopUpOnOff();
        }

        Debug.Log(isOn);
    }

    public void ScorePopUpOnOff()
    {
        if(isOn)
        {
            _scorePopUI.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InCubic);
        }
        else
        {
            _scorePopUI.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        isOn = !isOn;
    }

    private void FillStars()
    {

    }
}
