using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconUI : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _moveSpeed;

    [SerializeField] private RectTransform _parentTrm;
    [SerializeField] private RectTransform _startPos;
    [SerializeField] private RectTransform _endPos;

    private Image _icon;

    private void Awake()
    {
        _icon = GetComponent<Image>();
    }

    private void Start()  
    {
        _parentTrm.anchoredPosition = _startPos.anchoredPosition;
    }

    private void Update()
    {
        _icon.rectTransform.Rotate(Vector3.back * Time.deltaTime * _rotateSpeed);
        _parentTrm.Translate(Vector3.right * Time.deltaTime * PlayerManager.Instance.GetMoveToForward.MoveSpeed / 2000);
    }
}
