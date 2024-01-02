using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarUI : MonoBehaviour
{
    private Image _bar;

    private void Awake()
    {
        _bar = GetComponent<Image>();
    }

    private void Update()
    {
        _bar.fillAmount = PlayerManager.Instance.GetMoveToForward.MoveSpeed / 100;
    }
}
