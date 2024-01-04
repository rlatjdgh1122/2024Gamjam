using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CometHit : MonoBehaviour
{
    [SerializeField] Vector3 _maxSize;
    [SerializeField] float _duration;
    [SerializeField] float _camShake;
    [SerializeField] float _camDuration;
    [SerializeField] TextMeshProUGUI _pressText;
    [SerializeField] GameObject _pressCanvas;
    
    private GameObject _pluto;
    private int _maxKeyCount = 20;
    private int _currentkeyCount = 0;
    private float _plutoDis;
    private float _clearTime = 7f;
    private float _currentTime = 0;

    float originSpeed;
    bool isEnd = false;
    bool checkSpeed = false;

    //private Sequence _seq;

    private void Awake()
    {
        //_seq = DOTween.Sequence();

        _currentTime = _clearTime;
    }

    private void Start()
    {
        _pluto = FindObjectOfType<PlayerMovement>().gameObject;
    }

    private void Update()
    {
        _plutoDis = Vector3.Distance(transform.position, _pluto.transform.position);
        
        if (_plutoDis <= 500f)
        {
            if (!checkSpeed)
            {
                originSpeed = PlayerManager.Instance.GetMoveToForward.MoveSpeed;
                checkSpeed = true; 
            }
            PlayerManager.Instance.GetMoveToForward.IsSpeed = false;
            PlayerManager.Instance.GetMoveToForward.MoveSpeed = 0;
            transform.DOScale(_maxSize, _duration);
            _pressCanvas.SetActive(true);

            
            FastTyping();
        }
    }

    private void FastTyping()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _currentkeyCount++;
                PlayerFollowCam.Instance.ShakeCam(_camDuration, _camShake);
            }

            if (_currentkeyCount == _maxKeyCount)
            {
                ClearGame();
            }
        }
        else
        {
            if (!isEnd)
            {
                GameOver();
                isEnd = true;
            }
        }
    }

    void ClearGame()
    {
        transform.DOScale(0, _duration);
        PlayerManager.Instance.GetMoveToForward.MoveSpeed = originSpeed;
        PlayerManager.Instance.GetMoveToForward.IsSpeed = true;
        _pressCanvas.SetActive(false);
        enabled = false;
    }

    void GameOver()
    {
        PlayerManager.Instance.GetPlayerDead.DeadImmedieatly();
        UIManager.Instance.DieReasonUI.UpdateIMG(DieReasonType.LoseKeyBoardBattle);
    }
}
