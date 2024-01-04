using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    private RectTransform _scorePopUI;

    private bool isOn = false;

    private PlayerFowardMover _playerFowardMover => PlayerManager.Instance.GetMoveToForward;
    private DurabilitySystem _durabilitySystem => PlayerManager.Instance.GetDurabilitySystem;

    private RenderTexture _renderTexture;

    [SerializeField]
    private Transform playerPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ScorePopUpOnOff();
            PlayerSetPos();
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


    private void PlayerSetPos()
    {
        PlayerFollowCam.Instance.gameObject.SetActive(false);
        PlayerManager.Instance.Player.transform.position = playerPos.position;
        PlayerManager.Instance.GetPlayerMovement.enabled = false;
        PlayerManager.Instance.GetMoveToForward.enabled = false;
        PlayerManager.Instance.GetBuringSystem.enabled = false;
    }
}
