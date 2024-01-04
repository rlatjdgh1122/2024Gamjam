using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityUI : MonoBehaviour
{
    private Image _fill;

    private void Awake()
    {
        _fill = GetComponent<Image>();
    }

    private void Update()
    {
        _fill.fillAmount = PlayerManager.Instance.GetDurabilitySystem.DurabilityValue * 0.8f;
    }
}
