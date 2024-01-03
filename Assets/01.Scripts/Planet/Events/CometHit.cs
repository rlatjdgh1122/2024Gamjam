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

    //private Sequence _seq;

    private void Awake()
    {
        //_seq = DOTween.Sequence();

        _currentTime = _clearTime;
    }

    private void Start()
    {
        _pluto = GameObject.Find("Target");
    }

    private void Update()
    {
        _plutoDis = Vector3.Distance(transform.position, _pluto.transform.position);
        
        if (_plutoDis < 10f)
        {
            transform.DOScale(_maxSize, _duration);
            _pressCanvas.SetActive(true);

            //_seq.Append(_pressText.DOFade(0, 0.5f))
            //    .Append(_pressText.DOFade(1, 0.5f));

            _pressText.DOFade(0, 0.5f).OnComplete(() =>
            {
                _pressText.DOFade(1, 0.5f).OnComplete(() =>
                {
                    _pressText.DOKill();
                });
            }); 
            

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
            GameOver();
        }
    }

    void ClearGame()
    {
        //내용을 채우시오~
        Debug.Log("게임 클리어!");
    }

    void GameOver()
    {
        //내용을 채우시오~
        Debug.Log("게임 오버!");
    }
}
