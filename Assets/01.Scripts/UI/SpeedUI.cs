using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{
    private TextMeshProUGUI _speedText;

    private void Awake()
    {
        _speedText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _speedText.text = (PlayerManager.Instance.GetMoveToForward.MoveSpeed * 150).ToString("N0") + " km/s";
    }

}