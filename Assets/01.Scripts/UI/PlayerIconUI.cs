using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconUI : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private RectTransform _target;

    private Image _icon;

    private void Awake()
    {
        _icon = GetComponent<Image>();
    }

    private void Update()
    {
        _icon.rectTransform.Rotate(Vector3.back * Time.deltaTime * _rotateSpeed);
        _icon.rectTransform.parent.Translate(Vector3.right * Time.deltaTime * _moveSpeed);
    }
}
